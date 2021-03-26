using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingZone : MonoBehaviour
{
    public List<GameObject> resourceObjects;

    public int teamID = 0;

    public int resourcesNeeded;

    public GameObject buildingPrefab;

    public GameObject building;

    // Change this to a property later
    public bool built = false;
    public bool exploded = false;
    public GameObject resourcePrefab;
    public GameObject factionDirector;

    private GameObject Visualization;
    // Needs to be manually called because of script order stuff
    public void Init()
    {

        resourcesNeeded = 3;


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
        Visualization = gameObject.transform.Find("Visualization").gameObject;

        if (teamID == 0)
        {
            //     iTween.MoveTo(Visualization, new Hashtable(){
            // {"isLocal", true},
            // {"position", new Vector3(Visualization.transform.localPosition.x, 0.4f, Visualization.transform.localPosition.z)},
            // {"time", Random.Range(8f, 14f)}
            // });
        }
        else
        {

            Visualization.SetActive(false);
        }

    }

    public void OnPlaced()
    {
        if (teamID == 0)
        {

        }
        else
        {

            building = Instantiate(buildingPrefab, transform.position, transform.rotation);
            building.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    void Update()
    {

        // gameObject.transform.Find("Visualization").GetComponent<Renderer>().enabled = !built;

    }
    public void IncreaseResource(GameObject Resource)
    {
        Resource.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Resource.transform.parent = null;
        Resource.transform.localPosition = transform.position + new Vector3(0, 1, 0);
        Resource.GetComponent<Rigidbody>().useGravity = false;
        Resource.layer = 11;
        Resource.GetComponent<Renderer>().enabled = false;
        resourceObjects.Add(Resource);


        if (teamID == 0)
        {
            var increaseBy = 1f / resourcesNeeded;
            iTween.MoveTo(Visualization, new Hashtable(){
        {"isLocal", true},
        {"position", new Vector3(Visualization.transform.localPosition.x,
        Visualization.transform.localPosition.y + increaseBy, Visualization.transform.localPosition.z)},
        {"time", 1}
        });
        }
        else
        {
            var increaseBy = 1f / resourcesNeeded;
            iTween.ScaleTo(building, new Vector3(building.transform.localScale.x + increaseBy, building.transform.localScale.y + increaseBy, building.transform.localScale.z + increaseBy), 1);
        }

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
        if (teamID == 0)
        {
            building = Instantiate(buildingPrefab, transform.position, transform.rotation);
            building.transform.position = building.transform.position - new Vector3(0, transform.localScale.y, 0);
            iTween.MoveTo(building, transform.position, Random.Range(6f, 9f));

        }
        else
        {
            // building.transform.localScale = new Vector3(0, 0, 0);
            // iTween.ScaleTo(building, new Vector3(1, 1, 1), Random.Range(6f, 9f));

        }
        factionDirector.GetComponent<FactionDirector>().UpdateBuiltZone(gameObject);

        iTween.MoveTo(Visualization, new Hashtable(){
        {"isLocal", true},
        {"position", new Vector3(Visualization.transform.localPosition.x, -0.591f, Visualization.transform.localPosition.z)
        },
        {"time", Random.Range(8f, 14f)}
        });
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
            var resource = Instantiate(resourcePrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
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