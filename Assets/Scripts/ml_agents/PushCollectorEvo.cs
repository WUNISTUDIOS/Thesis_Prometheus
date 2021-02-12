using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PushCollectorEvo : Agent
{
    Rigidbody rBody;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public Transform Resource;
    public Transform CollectionPoint;
    public GameObject Floor;
    private Renderer FloorRenderer;
    private Material floor_mat;
    public Material collected_mat;
    public Material failed_mat;




    // private Vector3 StartingPosition;

    public float forceMultiplier = 10;

    private bool firstTouched;

    public override void Initialize()
    {
        FloorRenderer = Floor.GetComponent<Renderer>();
        floor_mat = FloorRenderer.material;
    }

    public override void OnEpisodeBegin()
    {

        firstTouched = false;

        // Zero agent  momentum
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(0, 2, 0);
        this.transform.localRotation = Quaternion.Euler(0, 0, 0);

        Resource.localPosition = new Vector3(Random.Range(-20f, 20f),
                                           1.5f,
                                           Random.Range(-20f, 20f));
        Resource.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Resource.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;


        // StartingPosition = Resource.localPosition;
        switch (Random.Range(0, 4))
        {
            case 0:
                CollectionPoint.localPosition = new Vector3(30, 2.5f, Random.Range(-30f, 30f));
                break;
            case 1:
                CollectionPoint.localPosition = new Vector3(-30, 2.5f, Random.Range(-30f, 30f));

                break;
            case 2:
                CollectionPoint.localPosition = new Vector3(Random.Range(-30f, 30f), 2.5f, -30);

                break;
            case 3:
                CollectionPoint.localPosition = new Vector3(Random.Range(-30f, 30f), 2.5f, 30);
                break;
        }
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Resource.localPosition);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(CollectionPoint.localPosition);


        // Rotations
        sensor.AddObservation(Resource.localRotation);
        sensor.AddObservation(this.transform.localRotation);
        // Agent velocity
        sensor.AddObservation(rBody.velocity);
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
        if (this.transform.localPosition.y < 0 || Resource.localPosition.y < 0 || Resource.localPosition.y > 9)
        {
            EndEpisode();
            StartCoroutine(GoalScoredSwapGroundMaterial(failed_mat, 0.5f));
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
        SetReward(5.0f);
        EndEpisode();
        StartCoroutine(GoalScoredSwapGroundMaterial(collected_mat, 0.5f));
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