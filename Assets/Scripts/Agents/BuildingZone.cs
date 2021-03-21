using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingZone : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> resourceObjects;

    public int resourcesNeeded = 40;

    public GameObject buildingPrefab;

    public GameObject building;

    // Change this to a property later
    public bool built = false;
    public bool exploded = false;
    public GameObject resourcePrefab;

    private void Awake()
    {
        var buildingResources = Resources.LoadAll("New Origin Buildings", typeof(GameObject));
        buildingPrefab = buildingResources[Random.Range(0, buildingResources.Length)] as GameObject;

        var bounds = buildingPrefab.GetComponent<Renderer>().bounds;
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
        building = Instantiate(buildingPrefab, transform.position, Quaternion.Euler(buildingPrefab.transform.eulerAngles.x, transform.eulerAngles.y, buildingPrefab.transform.eulerAngles.z));
        GameObject.Find("Faction Director").GetComponent<FactionDirector>().UpdateBuiltZone(gameObject);


    }

    public void DestroyBuilding()
    {
        StartCoroutine(DestroyBuildingCoroutine());
    }



    IEnumerator DestroyBuildingCoroutine()
    {

        yield return new WaitForSeconds(Random.Range(3f, 9f));
        Destroy(building);
        built = true;

        var explodedResources = new List<GameObject>();
        for (int i = 0; i < 500; i++)
        {
            var resource = Instantiate(resourcePrefab, transform.position + new Vector3(0, 10, 0), Quaternion.identity);
            resource.GetComponent<Resource>().collected = true;
            resource.GetComponent<Rigidbody>().useGravity = false;
            explodedResources.Add(resource);

        }


        yield return new WaitForSeconds(Random.Range(1.5f, 3f));

        foreach (Transform child in transform)
        {
            exploded = true;
            child.GetComponent<BoxCollider>().enabled = false;
        }


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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("agent"))
        {
            other.GetComponent<NavCollector>().OnEnterZone(gameObject);
        }
    }
}