using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavCollector : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent myNavMeshAgent;
    public int teamID = 0;

    public GameObject CollectionPoint;
    public GameObject Resource;

    public string state = "idle";
    public bool carryingObject = false;

    public GameObject supplyZone;

    public GameObject SpotLight;

    AudioSource[] AudioSources;

    void Start()
    {
        myNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        AudioSources = GetComponents<AudioSource>();





        // StartCoroutine(TimeToDie());

    }

    IEnumerator TimeToDie()
    {
        yield return new WaitForSeconds(Random.Range(300f, 360f));
        // yield return new WaitForSeconds(Random.Range(5f, 10f));
        // AudioSources[1].pitch = Random.Range(-1f, 2f);
        AudioSources[1].Play();
        state = "dying";
        myNavMeshAgent.isStopped = true;
        myNavMeshAgent.enabled = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        iTween.RotateTo(gameObject, new Vector3(transform.rotation.x, transform.rotation.y, 90), 1.5f);
        yield return new WaitForSeconds(5);
        Destroy(gameObject);

    }

    void Update()
    {
        if (state == "collecting")
        {


            if (CollectionPoint.GetComponent<BuildingZone>())
            {
                if (supplyZone.GetComponent<SupplyZone>().resourceObjects.Count > 0)
                {
                    myNavMeshAgent.SetDestination(supplyZone.transform.position);
                }
                else
                {
                    // These are the same

                    Resource = findNearestResource();
                    if (Resource)
                    {


                        if (Resource.GetComponent<Resource>().collected)
                        {
                            state = "idle";
                        }
                        else
                        {
                            // Set destination to resource
                            myNavMeshAgent.SetDestination(Resource.transform.position);
                            SpotLight.GetComponent<Light>().color = new Color(1f, 0f, 0f, 1f);
                        }

                    }
                    else
                    {
                        state = "idle";
                    }
                }
            }
            else
            {
                // These are the same
                Resource = findNearestResource();
                if (Resource)
                {

                    if (Resource.GetComponent<Resource>().collected)
                    {
                        state = "idle";
                    }
                    else
                    {
                        // Set destination to resource
                        myNavMeshAgent.SetDestination(Resource.transform.position);
                        SpotLight.GetComponent<Light>().color = new Color(1f, 0f, 0f, 1f);
                    }

                }
                else
                {
                    state = "idle";
                }
            }





        }
        else if (state == "idle")
        {

            SpotLight.GetComponent<Light>().color = new Color(1f, 1f, 1f, 1f);
            myNavMeshAgent.SetDestination(supplyZone.transform.position + new Vector3(Random.Range(-120, 120), 0, Random.Range(-120, 120)));
            if (CollectionPoint != null && carryingObject)
            {
                state = "returning";
            }
            else if (!carryingObject)
            {
                if (CollectionPoint.GetComponent<BuildingZone>())
                {
                    if (supplyZone.GetComponent<SupplyZone>().resourceObjects.Count > 0)
                    {
                        state = "collecting";
                    }

                }
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
            if (Resource == null)
            {
                state = "idle";
                carryingObject = false;
                return;
            }
            Vector3 direction = ((CollectionPoint.transform.position + new Vector3(10, 0, 0)) - transform.position).normalized;
            float distance = Vector3.Distance(CollectionPoint.transform.position, transform.position);

            myNavMeshAgent.SetDestination(CollectionPoint.transform.position);

            SpotLight.GetComponent<Light>().color = new Color(0, 1, 0.5960785f, 1f);
        }

    }

    public void OnEnterZone(GameObject zone)
    {
        if (state == "returning" && zone == CollectionPoint)
        {
            DropOffResource();
        }

        if (state == "collecting" && zone == supplyZone && CollectionPoint != zone)
        {
            if (zone.GetComponent<SupplyZone>().resourceObjects.Count > 0)
            {
                CarryResource(zone.GetComponent<SupplyZone>().DecreaseResource());

            }
        }
        // if (state == "collecting" && zone == supplyZone && !carryingObject && supplyZone.GetComponent<SupplyZone>().resourceObjects.Count > 0)
        // {
        //     var resource = zone.GetComponent<SupplyZone>().DecreaseResource();
        //     CarryResource(resource);
        // }
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
    private void DropOffResource()
    {
        if (Resource == null) return;
        if (CollectionPoint.GetComponent<BuildingZone>())
        {
            CollectionPoint.GetComponent<BuildingZone>().IncreaseResource(Resource);
        }
        else
        {
            CollectionPoint.GetComponent<SupplyZone>().IncreaseResource(Resource);
        }
        Resource = null;
        state = "idle";
        carryingObject = false;
    }
    private void CarryResource(GameObject resource)
    {
        resource.transform.parent = transform;
        resource.transform.localPosition = new Vector3(0, 5f, 0);
        resource.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        resource.gameObject.GetComponent<Resource>().collected = true;

        if (resource.gameObject.GetComponent<Resource>().resourceArea)
        {
            resource.gameObject.GetComponent<Resource>().resourceArea.GetComponent<ResourceArea>().DecreaseResource();
            resource.gameObject.GetComponent<Resource>().resourceArea = null;
        }
        Resource = resource;
        carryingObject = true;
        state = "returning";

    }

    private void OnCollisionEnter(Collision other)
    {


        if (other.collider.CompareTag("resource") && state == "collecting" && other.collider.gameObject == Resource)
        {

            CarryResource(other.gameObject);


        }

    }

}
