using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavAgentController : MonoBehaviour
{
    NavMeshAgent myNavMeshAgent;
    void Start()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetDestinationToMousePosition();
        }
    }

    void SetDestinationToMousePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            myNavMeshAgent.SetDestination(hit.point);
        }
    }
}
