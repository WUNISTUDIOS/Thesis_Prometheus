using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavCollector : MonoBehaviour
{

    UnityEngine.AI.NavMeshAgent myNavMeshAgent;

    public GameObject CollectionPoint;
    public GameObject Resource;

    public string state = "idle";
    public bool carryingObject = false;
    public bool goingToBuildingZone = false;


    public GameObject SpotLight;

    void Start()
    {
        myNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (CollectionPoint.GetComponent<BuildingZone>())
        {
            goingToBuildingZone = true;
        }

    }
    void Update()
    {
        if (state == "collecting")
        {
            if (Resource.GetComponent<Resource>().collected)
            {
                state = "idle";
            }

            if (Resource)
            {
                // Set destination to resource
                myNavMeshAgent.SetDestination(Resource.transform.position);

            }
            else
            {
                state = "idle";
            }



        }
        else if (state == "idle")
        {

            if (CollectionPoint != null && carryingObject)
            {
                state = "returning";
            }
            else if (!carryingObject)
            {
                Resource = findNearestResource();

                if (Resource != null)
                {
                    state = "collecting";
                }
            }
            else
            {
                // Uncomment to disable idle buzzing
                // myNavMeshAgent.isStopped = true;
                // myNavMeshAgent.ResetPath();
            }

        }


        else if (state == "returning")
        {
            Vector3 direction = ((CollectionPoint.transform.position + new Vector3(10, 0, 0)) - transform.position).normalized;
            float distance = Vector3.Distance(CollectionPoint.transform.position, transform.position);

            myNavMeshAgent.SetDestination(CollectionPoint.transform.position);

            SpotLight.GetComponent<Light>().color = new Color(0, 1, 0.5960785f, 1f);

            if (distance < 15)
            {
                Resource.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                Resource.transform.parent = null;
                Resource.transform.localPosition = CollectionPoint.transform.position + new Vector3(0, 20, 0);
                Resource.GetComponent<Rigidbody>().useGravity = false;
                Resource.layer = 11;
                if (goingToBuildingZone)
                {
                    CollectionPoint.GetComponent<BuildingZone>().IncreaseResource(Resource);
                }
                Resource = null;
                state = "idle";
                carryingObject = false;
                SpotLight.GetComponent<Light>().color = new Color(1f, 1f, 1f, 1f);

            }
        }

    }

    private GameObject findNearestResource()
    {
        GameObject[] resources = GameObject.FindGameObjectsWithTag("resource");
        GameObject closest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject resource in resources)
        {
            if (!resource.gameObject.GetComponent<Resource>().collected)
            {
                Vector3 diff = resource.transform.position - transform.position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = resource;
                    distance = curDistance;
                }
            }

        }
        return closest;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("resource") && state == "collecting" && other.collider.gameObject == Resource)
        {
            other.transform.parent = transform;
            other.transform.localPosition = new Vector3(0, 5f, 0);
            other.rigidbody.constraints = RigidbodyConstraints.FreezePosition;
            other.gameObject.GetComponent<Resource>().collected = true;
            other.gameObject.GetComponent<Resource>().resourceArea.GetComponent<ResourceArea>().DecreaseResource();
            carryingObject = true;

            if (CollectionPoint)
            {
                state = "returning";

            }
            else
            {
                state = "idle";
            }

        }
    }

}
