using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;
public class PushCollectorAgentvsAgent : Agent
{
    Rigidbody rBody;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public Transform Resource;
    public Transform CollectionPoint;
    public Transform MyCollectionPoint;

    public Transform OtherAgent;

    public GameObject Floor;
    private Renderer FloorRenderer;
    private Material floor_mat;

    public Material score_mat;

    public float forceMultiplier = 10;
    BehaviorParameters m_BehaviorParameters;

    private bool firstTouched;
    public enum Team
    {
        Red = 0,
        Blue = 1
    }
    public Team team;

    public override void Initialize()
    {
        FloorRenderer = Floor.GetComponent<Renderer>();
        floor_mat = FloorRenderer.material;
    }

    public override void OnEpisodeBegin()
    {

        firstTouched = false;

        // Zero agent  momentum

        m_BehaviorParameters = gameObject.GetComponent<BehaviorParameters>();
        if (m_BehaviorParameters.TeamId == (int)Team.Blue)
        {
            team = Team.Blue;
            this.transform.localPosition = new Vector3(-40, 2, 0);
            this.transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            team = Team.Red;
            this.transform.localPosition = new Vector3(40, 2, 0);
            this.transform.localRotation = Quaternion.Euler(0, -90, 0);
        }

        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        Resource.localPosition = new Vector3(0,
                                           4f,
                                           Random.Range(-45f, 45f));
        Resource.localRotation = Quaternion.Euler(0, 0, 0);

        Resource.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Resource.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;



    }


    public override void CollectObservations(VectorSensor sensor)
    {
        // // Target and Agent positions
        // sensor.AddObservation(this.transform.localPosition);
        // sensor.AddObservation(Resource.localPosition);
        // sensor.AddObservation(OtherAgent.localPosition);
        // sensor.AddObservation(CollectionPoint.localPosition - this.transform.localPosition);
        // sensor.AddObservation(MyCollectionPoint.localPosition - this.transform.localPosition);
        // // sensor.AddObservation(OtherAgent.localPosition - this.transform.localPosition);
        // sensor.AddObservation(CollectionPoint.localPosition - this.transform.localPosition);
        // sensor.AddObservation(MyCollectionPoint.localPosition - this.transform.localPosition);
        // sensor.AddObservation(Resource.localPosition - this.transform.localPosition);
        // sensor.AddObservation(Resource.localPosition - CollectionPoint.localPosition);
        // sensor.AddObservation(Resource.localPosition - MyCollectionPoint.localPosition);




        // // Rotations
        // // sensor.AddObservation(Resource.localRotation.eulerAngles / 180.0f - Vector3.one);
        // sensor.AddObservation((this.transform.localRotation.eulerAngles / 180.0f - Vector3.one).y);
        // // Agent and Resource velocity
        // sensor.AddObservation(rBody.velocity);
        // sensor.AddObservation(Resource.gameObject.GetComponent<Rigidbody>().velocity);
        // sensor.AddObservation(OtherAgent.gameObject.GetComponent<Rigidbody>().velocity);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // Actions, size = 3
        // var rotateDir = Vector3.zero;
        // Vector3 controlSignal = Vector3.zero;
        // controlSignal.x = vectorAction[0];
        // controlSignal.z = vectorAction[1];
        // rotateDir.y = vectorAction[2];
        // rBody.AddForce(controlSignal * forceMultiplier);
        // transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);


        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;
        var action = vectorAction[0];
        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
            case 5:
                dirToGo = transform.right * -0.75f;
                break;
            case 6:
                dirToGo = transform.right * 0.75f;
                break;
        }
        transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
        rBody.AddForce(dirToGo * forceMultiplier,
            ForceMode.VelocityChange);

        // Fell off platform
        if (this.transform.localPosition.y < 0 || Resource.localPosition.y < 0 || Resource.localPosition.y > 20f)
        {
            EndEpisode();
        }

        AddReward(-1f / MaxStep);
    }

    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.collider.CompareTag("resource"))
    //     {
    //         if (firstTouched == false)
    //         {
    //             firstTouched = true;
    //             AddReward(0.5f);
    //         }
    //     }
    // }

    public void CollectedResource()
    {
        SetReward(2.0f);
        OtherAgent.gameObject.GetComponent<PushCollectorAgentvsAgent>().SetReward(-1.0f);
        OtherAgent.gameObject.GetComponent<PushCollectorAgentvsAgent>().EndEpisode();
        EndEpisode();
        StartCoroutine(GoalScoredSwapGroundMaterial(score_mat, 0.5f));

    }

    IEnumerator GoalScoredSwapGroundMaterial(Material mat, float time)
    {
        FloorRenderer.material = mat;
        yield return new WaitForSeconds(time); // Wait for 2 sec
        FloorRenderer.material = floor_mat;
    }

    public override void Heuristic(float[] actionsOut)
    {
        var discreteActionsOut = actionsOut;
        discreteActionsOut[0] = 0;
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
    }
}