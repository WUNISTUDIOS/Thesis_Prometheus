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


    public int NumOfAgents = 10;

    // Start is called before the first frame update
    void Start()
    {
        buildingZones = new List<GameObject>(GameObject.FindGameObjectsWithTag("buildingzone"));
        for (int i = 0; i < NumOfAgents; i++)
        {
            var newAgent = Instantiate(agentPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newAgent.GetComponent<NavCollector>().CollectionPoint = buildingZones[0];
            newAgent.GetComponent<NavMeshAgent>().speed = Random.Range(70f, 120f);
            agents.Add(newAgent);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateBuiltZone(GameObject builtZone)
    {
        buildingZones.Remove(builtZone);
        completedBuildingZones.Add(builtZone);

        foreach (GameObject agent in agents)
        {
            if (buildingZones.Count > 0)
            {
                agent.GetComponent<NavCollector>().CollectionPoint = buildingZones[0];

            }
            else
            {
                agent.GetComponent<NavCollector>().CollectionPoint = null;
                agent.GetComponent<NavCollector>().state = "idle";
            }

        }


        // if (buildingZones.Count == 0)
        // {
        //     foreach (GameObject building in completedBuildingZones)
        //     {
        //         building.GetComponent<BuildingZone>().DestroyBuilding();
        //     }
        // }
    }
}
