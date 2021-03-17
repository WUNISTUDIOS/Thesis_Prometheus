using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FactionDirector : MonoBehaviour
{
    public GameObject agentPrefab;

    public List<GameObject> agents;

    public List<GameObject> buildingZones;

    public List<GameObject> completedBuildingZones;

    public GameObject supplyZone;


    public int NumOfAgents = 10;

    // Start is called before the first frame update
    void Start()
    {
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

    }

    public void UpdateBuiltZone(GameObject builtZone)
    {
        buildingZones.Remove(builtZone);
        completedBuildingZones.Add(builtZone);
        OrderAgentsToBuildNext();

        // Destroy buildings test 
        if (buildingZones.Count == 0)
        {
            foreach (GameObject building in completedBuildingZones)
            {
                building.GetComponent<BuildingZone>().DestroyBuilding();
            }
        }
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
