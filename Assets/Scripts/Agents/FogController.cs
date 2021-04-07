using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class FogController : MonoBehaviour
{
    public void OnBuilding()
    {
        var buildingCount = GetComponent<FactionDirector>().completedBuildingZones.Count;

    }
}
