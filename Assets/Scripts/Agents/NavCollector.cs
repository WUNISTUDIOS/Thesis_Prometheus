using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavCollector : MonoBehaviour
{

    UnityEngine.AI.NavMeshAgent myNavMeshAgent;

    public GameObject CollectionPoint;
    public GameObject Resource;

    public string state = "seeking";
    public bool carryingObject = false;

    public GameObject SpotLight;

    void Start()
    {
        myNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

    }
    void Update()
    {

        if (state == "collecting")
        {
            if (Resource.GetComponent<Resource>().collected)
            {
                state = "seeking";
            }

            // Set destination to resource
            myNavMeshAgent.SetDestination(Resource.transform.position);

            Resource = findNearestResource();


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
                Resource = null;
                state = "seeking";
                carryingObject = false;
                SpotLight.GetComponent<Light>().color = new Color(1f, 1f, 1f, 1f);
            }
        }

        else if (state == "seeking")
        {
            Resource = findNearestResource();
            if (Resource != null)
            {
                state = "collecting";
            }

            if (Resource == null)
            {
                state = "idle";
            }
        }
        else if (state == "idle")
        {
            Vector3 direction = (CollectionPoint.transform.position - transform.position).normalized;
            float distance = Vector3.Distance(CollectionPoint.transform.position, transform.position);

            myNavMeshAgent.SetDestination(CollectionPoint.transform.position);
            Resource = findNearestResource();

            if (Resource != null)
            {
                state = "collecting";
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
        if (other.collider.CompareTag("resource") && !carryingObject && other.collider.gameObject == Resource)
        {
            // if (!other.gameObject.GetComponent<Resource>().collected)
            // {
            state = "returning";
            other.transform.parent = transform;
            // other.transform.localPosition = new Vector3(0, 2, 0);
            other.transform.localPosition = new Vector3(0, 3.8f, 0);
            other.rigidbody.constraints = RigidbodyConstraints.FreezePosition;
            other.gameObject.GetComponent<Resource>().collected = true;
            other.gameObject.GetComponent<Resource>().resourceArea.GetComponent<ResourceArea>().DecreaseResource();
            carryingObject = true;
            // }

        }
    }

}
