using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionPointEvo : MonoBehaviour
{
    public PushCollectorAgentvsAgent agentToGivePoints;

    private void OnTriggerEnter(Collider col)
    {
        // Touched goal.
        if (col.gameObject.CompareTag("resource"))
        {
            agentToGivePoints.CollectedResource();
        }
    }
}
