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


    public GameObject resourcePrefab;


    void Start()
    {
        var buildingResources = Resources.LoadAll("correctBuildings", typeof(GameObject));
        buildingPrefab = buildingResources[Random.Range(0, buildingResources.Length)] as GameObject;
    }
    public void IncreaseResource(GameObject Resource)
    {
        resourceObjects.Add(Resource);
        if (resourceObjects.Count >= resourcesNeeded)
        {
            CreateBuilding();
        }
    }


    public void CreateBuilding()
    {
        foreach (var resource in resourceObjects)
        {
            Destroy(resource);
        }
        resourceObjects.Clear();
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
        building = Instantiate(buildingPrefab, transform.position + new Vector3(0, bounds.size.y, 0) / 2, Quaternion.Euler(Vector3.zero));
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
        for (int i = 0; i < 500; i++)
        {
            var resource = Instantiate(resourcePrefab, transform.position + new Vector3(0, 10, 0), Quaternion.identity);
            resource.GetComponent<Rigidbody>().useGravity = false;
        }


        yield return new WaitForSeconds(Random.Range(1.5f, 3f));

        foreach (Transform child in transform)
        {
            child.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
