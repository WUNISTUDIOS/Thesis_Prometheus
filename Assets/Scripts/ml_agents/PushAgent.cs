using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PushAgent : Agent
{
    Rigidbody rBody;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void Initialize()
    {
        this.MaxStep = 10000;
    }

    public Transform Resource;
    public Transform CollectionPoint;

    private Vector3 StartingPosition;


    private bool firstTouched;
    public override void OnEpisodeBegin()
    {

        firstTouched = false;

        // If the Agent fell, zero its momentum
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(0, 0.5f, 0);
        this.transform.localRotation = Quaternion.Euler(0, 0, 0);

        // Move the target to a new spot
        Resource.localPosition = new Vector3(Random.value * 8 - 4,
                                           0.5f,
                                           Random.value * 8 - 4);
        Resource.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        StartingPosition = Resource.localPosition;
        // Move the collection point to a new spot

        switch (Random.Range(0, 4))
        {
            case 0:
                CollectionPoint.localPosition = new Vector3(0f,
                                               0.5f,
                                               12);

                CollectionPoint.localRotation = Quaternion.Euler(0,
                0f,
                0f);
                break;
            case 1:
                CollectionPoint.localPosition = new Vector3(-12f,
                                              0.5f,
                                              0f);

                CollectionPoint.localRotation = Quaternion.Euler(0,
                90f,
                0f);
                break;
            case 2:
                CollectionPoint.localPosition = new Vector3(0f,
                                             0.5f,
                                             -12);

                CollectionPoint.localRotation = Quaternion.Euler(0,
                0f,
                0f);
                break;
            case 3:
                CollectionPoint.localPosition = new Vector3(12f,
                                             0.5f,
                                             0);

                CollectionPoint.localRotation = Quaternion.Euler(0,
                90f,
                0f);
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
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public float forceMultiplier = 10;
    public override void OnActionReceived(float[] vectorAction)
    {
        // Actions, size = 3
        var rotateDir = Vector3.zero;
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rotateDir.y = vectorAction[2];
        rBody.AddForce(controlSignal * forceMultiplier);
        transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
        // Rewards
        float distanceToTarget = Vector3.Distance(CollectionPoint.localPosition, Resource.localPosition);
        // Reached target
        // if (distanceToTarget < 1.42f)
        // {
        //     SetReward(5.0f);
        //     EndEpisode();
        // }

        // Fell off platform
        if (this.transform.localPosition.y < 0 || Resource.localPosition.y < 0)
        {
            SetReward(-0.05f);
            EndEpisode();
        }

        // Debug.Log(-1f * (Vector3.Distance(Resource.localPosition, CollectionPoint.localPosition) / Vector3.Distance(StartingPosition, CollectionPoint.localPosition)));
        AddReward((-2f * (Vector3.Distance(Resource.localPosition, CollectionPoint.localPosition) / Vector3.Distance(StartingPosition, CollectionPoint.localPosition))) / MaxStep);
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("resource"))
        {
            if (firstTouched == false)
            {
                firstTouched = true;
                AddReward(0.1f);
            }
        }
    }

    // private void OnCollisionStay(Collision other)
    // {
    //     if (other.collider.CompareTag("resource"))
    //     {
    //         SetReward(0.01f);
    //     }
    // }

    public void CollectedResource()
    {
        SetReward(5.0f);
        EndEpisode();
    }
    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }
}