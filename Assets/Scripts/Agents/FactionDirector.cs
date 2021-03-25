using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FactionDirector : MonoBehaviour
{
    public int NumOfAgents = 10;
    public int teamID = 0;
    public List<GameObject> agents;

    public List<GameObject> buildingZones;

    public List<GameObject> completedBuildingZones;

    public GameObject supplyZone;

    public GameObject agentPrefab;

    public GameObject buildingZonePrefab;

    // Start is called before the first frame update
    void Start()
    {

        // Currently bugged when placing more than one building at a time, ignores eachothers colliders
        // int startingBuildings = 3;

        // for (int i = 0; i < startingBuildings; i++)
        // {
        PlaceNewBuildingZone();

        // }
        // Will probably need to add a check for if the building is already built later
        for (int i = 0; i < NumOfAgents; i++)
        {
            var newAgent = Instantiate(agentPrefab, transform.position, Quaternion.identity);
            newAgent.GetComponent<NavCollector>().CollectionPoint = buildingZones[0];
            newAgent.GetComponent<NavCollector>().supplyZone = supplyZone;
            newAgent.GetComponent<NavMeshAgent>().speed = Random.Range(70f, 120f);
            newAgent.GetComponent<NavCollector>().teamID = teamID;
            agents.Add(newAgent);

        }



    }

    // Update is called once per frame
    void Update()
    {
        UpdateBuildingList();
        OrderAgentsToBuildNext();

        // supplyZone.GetComponent<SupplyZone>().resourceObjects.Count > 30 && 
        if (supplyZone.GetComponent<SupplyZone>().resourceObjects.Count >= 0 && buildingZones.Count <= 0)
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
        var building = Instantiate(buildingZonePrefab, new Vector3(10000, 10000, 10000), Quaternion.Euler(0, Random.Range(0f, 360f), 0));
        building.GetComponent<BuildingZone>().teamID = teamID;
        building.GetComponent<BuildingZone>().factionDirector = gameObject;
        building.GetComponent<BuildingZone>().Init();
        while (!buildingPlacementFound && placementTries < 200)
        {
            if (Physics.Raycast(transform.position + new Vector3(x, 200, z), transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, firstMask))
            {
                // Yes I know this is starting to become if statement hell
                var hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                if (hitRotation.eulerAngles.x >= 30 || hitRotation.eulerAngles.x <= -30 || hitRotation.eulerAngles.z >= 30 || hitRotation.eulerAngles.z <= -30)
                {
                    Debug.Log("go next");
                    x += Random.Range(-20, 20f);
                    z += Random.Range(-20, 20f);
                }
                else
                {
                    Debug.DrawRay(transform.position + new Vector3(x, 200, z), transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
                    LayerMask mask = ~LayerMask.GetMask("terrain");
                    // Everything
                    // LayerMask mask = ~0;
                    Collider[] hitColliders;
                    if (teamID == 0)
                    {

                        hitColliders = Physics.OverlapBox(hit.point + new Vector3(0, building.transform.localScale.y / 2, 0), building.transform.localScale / 2, building.transform.rotation * Quaternion.FromToRotation(Vector3.up, hit.normal), mask, QueryTriggerInteraction.Collide);
                    }
                    else
                    {
                        hitColliders = Physics.OverlapBox(hit.point + new Vector3(0, building.transform.localScale.y / 2, 0), building.transform.localScale / 4, building.transform.rotation * Quaternion.FromToRotation(Vector3.up, hit.normal), mask, QueryTriggerInteraction.Collide);

                    }
                    if (hitColliders.Length > 0)
                    {
                        Debug.Log("go next");
                        x += Random.Range(-20, 20f);
                        z += Random.Range(-20, 20f);
                    }
                    else
                    {
                        // if (teamID == 1)
                        // {
                        //     building.transform.position = hit.point + new Vector3(0, building.transform.localScale.y / 2, 0);

                        // }
                        // else
                        // {

                        //     building.transform.position = hit.point;
                        // }
                        // Check if building placement is reachable
                        var path = new NavMeshPath();
                        NavMesh.CalculatePath(supplyZone.transform.position, hit.point, NavMesh.AllAreas, path);
                        if (path.status == NavMeshPathStatus.PathComplete)
                        {
                            building.transform.position = hit.point;
                            building.transform.rotation = building.transform.rotation * hitRotation;
                            building.GetComponent<BuildingZone>().OnPlaced();
                            Debug.Log("success");
                            buildingPlacementFound = true;
                            buildingZones.Add(building);
                        }
                        else
                        {
                            Debug.Log("go next");
                            x += Random.Range(-20, 20f);
                            z += Random.Range(-20, 20f);
                        }

                    }
                }


            }
            placementTries++;
        }

        if (!buildingPlacementFound)
        {
            Destroy(building);
        }
    }
    public void UpdateBuiltZone(GameObject buildZone)
    {
        buildingZones.Remove(buildZone);
        completedBuildingZones.Add(buildZone);
        OrderAgentsToBuildNext();

        // Destroy buildings test 
        // if (completedBuildingZones.Count >= 10)
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
            if (!building.GetComponent<BuildingZone>().built && !buildingZones.Contains(building) && building.GetComponent<BuildingZone>().teamID == teamID)
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
