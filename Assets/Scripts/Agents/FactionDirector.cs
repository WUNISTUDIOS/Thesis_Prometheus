using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FactionDirector : MonoBehaviour
{
    public GameObject agentPrefab;

    public GameObject buildingZonePrefab;

    public List<GameObject> agents;

    public List<GameObject> buildingZones;

    public List<GameObject> completedBuildingZones;

    public GameObject supplyZone;


    public int NumOfAgents = 10;

    // Start is called before the first frame update
    void Start()
    {


        int newBuildings = 3;

        for (int i = 0; i < newBuildings; i++)
        {
            PlaceNewBuildingZone();
        }
        // Will probably need to add a check for if the building is already built later
        buildingZones = new List<GameObject>(GameObject.FindGameObjectsWithTag("buildingzone"));
        for (int i = 0; i < NumOfAgents; i++)
        {
            var newAgent = Instantiate(agentPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newAgent.GetComponent<NavCollector>().CollectionPoint = buildingZones[0];
            newAgent.GetComponent<NavCollector>().supplyZone = supplyZone;
            newAgent.GetComponent<NavMeshAgent>().speed = Random.Range(70f, 120f);
            agents.Add(newAgent);

        }



    }

    // Update is called once per frame
    void Update()
    {
        UpdateBuildingList();
        OrderAgentsToBuildNext();

        // supplyZone.GetComponent<SupplyZone>().resourceObjects.Count > 30 && 
        if (buildingZones.Count <= 0)
        {
            PlaceNewBuildingZone();
        }
    }

    public void PlaceNewBuildingZone()
    {
        var buildingPlacementFound = false;
        LayerMask firstMask = LayerMask.GetMask("terrain");
        RaycastHit hit;
        int placementTries = 0;
        float x = 0;
        float z = 0;
        while (!buildingPlacementFound && placementTries < 100)
        {
            if (Physics.Raycast(transform.position + new Vector3(x, 200, z), transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, firstMask))
            {
                Debug.DrawRay(transform.position + new Vector3(x, 200, z), transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
                // Building floats 2.2 units up for some reason 
                LayerMask mask = ~LayerMask.GetMask("terrain");
                Collider[] hitColliders = Physics.OverlapBox(hit.point, buildingZonePrefab.GetComponent<Renderer>().bounds.size, Quaternion.FromToRotation(Vector3.up, hit.normal), mask);
                if (hitColliders.Length > 0)
                {
                    Debug.Log("try again");
                    x += Random.Range(-20, 20f);
                    z += Random.Range(-20, 20f);
                }
                else
                {
                    var building = Instantiate(buildingZonePrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(0, Random.Range(0f, 360f), 0));
                    Debug.Log("success");
                    buildingPlacementFound = true;
                }

            }
            placementTries++;
        }
    }
    public void UpdateBuiltZone(GameObject builtZone)
    {
        buildingZones.Remove(builtZone);
        completedBuildingZones.Add(builtZone);
        OrderAgentsToBuildNext();

        // Destroy buildings test 
        // if (buildingZones.Count == 0)
        // {
        //     foreach (GameObject building in completedBuildingZones)
        //     {
        //         building.GetComponent<BuildingZone>().DestroyBuilding();
        //     }
        // }
    }

    public void UpdateBuildingList()
    {
        // Probably really inefficient replace this later
        var foundBuildings = new List<GameObject>(GameObject.FindGameObjectsWithTag("buildingzone"));

        foreach (GameObject building in foundBuildings)
        {
            if (!building.GetComponent<BuildingZone>().built && !buildingZones.Contains(building))
            {
                buildingZones.Add(building);
            }

        }
    }
    public void OrderAgentsToBuildNext()
    {
        foreach (GameObject agent in agents)
        {
            if (buildingZones.Count > 0)
            {
                agent.GetComponent<NavCollector>().CollectionPoint = buildingZones[0];

            }
            else
            {
                agent.GetComponent<NavCollector>().CollectionPoint = supplyZone;
            }

        }

    }
}
