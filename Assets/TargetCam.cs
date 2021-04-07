using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCam : MonoBehaviour
{
    Transform targetPos;
    void Start()
    {
        InvokeRepeating("FindNewTarget", 10f, 120f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = targetPos.position;
    }


    void FindNewTarget()
    {
        var agents = FindObjectsOfType<NavCollector>();
        targetPos = agents[Random.Range(0, agents.Length)].gameObject.transform;
    }
}
