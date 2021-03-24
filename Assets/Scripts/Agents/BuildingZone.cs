using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingZone : MonoBehaviour
{
    public List<GameObject> resourceObjects;

    public int teamID = 0;

    public int resourcesNeeded = 40;

    public GameObject buildingPrefab;

    public GameObject building;

    // Change this to a property later
    public bool built = false;
    public bool exploded = false;
    public GameObject resourcePrefab;
    public GameObject factionDirector;


    // Needs to be manually called because of script order stuff
    public void Init()
    {
        Object[] buildingResources;
        if (teamID == 0)
        {
            buildingResources = Resources.LoadAll("City Buildings", typeof(GameObject));

        }
        else
        {
            buildingResources = Resources.LoadAll("Country Buildings", typeof(GameObject));
            GetComponent<NavMeshObstacle>().size = new Vector3(0.4f, 0.4f, 0.4f);
        }
        buildingPrefab = buildingResources[Random.Range(0, buildingResources.Length)] as GameObject;

        var bounds = new Bounds();
        if (buildingPrefab.GetComponent<Renderer>())
        {
            bounds = buildingPrefab.GetComponent<Renderer>().bounds;
        }
        else
        {
            Renderer[] renderers = buildingPrefab.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                bounds.Encapsulate(renderer.bounds);
            }
        }
        transform.localScale = bounds.size;

    }

    void Update()
    {

        gameObject.transform.Find("Visualization").GetComponent<Renderer>().enabled = !built;

    }
    public void IncreaseResource(GameObject Resource)
    {
        Resource.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Resource.transform.parent = null;
        Resource.transform.localPosition = transform.position + new Vector3(0, 20, 0);
        Resource.GetComponent<Rigidbody>().useGravity = false;
        Resource.layer = 11;
        resourceObjects.Add(Resource);

        if (resourceObjects.Count > resourcesNeeded)
        {

            CreateBuilding();
        }

    }
    public void CreateBuilding()
    {
        built = true;
        foreach (var resource in resourceObjects)
        {
            Destroy(resource);
        }
        resourceObjects.Clear();
        // building = Instantiate(buildingPrefab, transform.position, Quaternion.Euler(buildingPrefab.transform.eulerAngles.x, transform.eulerAngles.y, buildingPrefab.transform.eulerAngles.z));
        building = Instantiate(buildingPrefab, transform.position, transform.rotation);
        factionDirector.GetComponent<FactionDirector>().UpdateBuiltZone(gameObject);
        SetColliders(false);

    }

    public void DestroyBuilding()
    {
        if (!exploded)
        {
            StartCoroutine(DestroyBuildingCoroutine());

        }
    }



    IEnumerator DestroyBuildingCoroutine()
    {

        yield return new WaitForSeconds(Random.Range(3f, 9f));
        Destroy(building);
        // Change to false to renable auto rebuild
        built = true;
        exploded = true;

        SetColliders(true);
        var explodedResources = new List<GameObject>();
        for (int i = 0; i < 500; i++)
        {
            var resource = Instantiate(resourcePrefab, transform.position + new Vector3(0, 10, 0), Quaternion.identity);
            resource.GetComponent<Resource>().collected = true;
            resource.GetComponent<Rigidbody>().useGravity = false;
            explodedResources.Add(resource);

        }


        yield return new WaitForSeconds(Random.Range(1.5f, 3f));
        SetColliders(false);

        yield return new WaitForSeconds(10f);

        foreach (GameObject resource in explodedResources)
        {
            Destroy(resource);
        }


        // Uncomment to auto regen buildings after 30s
        // yield return new WaitForSeconds(30f);
        // built = false;
        //GetComponent<Renderer>().enabled = false;
        // exploded = false;

    }

    private void SetColliders(bool value)
    {
        // gameObject.GetComponent<BoxCollider>().enabled = value;
        foreach (Transform child in transform)
        {
            if (child.GetComponent<BoxCollider>())
            {
                child.GetComponent<BoxCollider>().enabled = value;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("agent") && other.GetComponent<NavCollector>().teamID == teamID)
        {
            other.GetComponent<NavCollector>().OnEnterZone(gameObject);
        }
    }
}