using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyZone : MonoBehaviour
{

    public List<GameObject> resourceObjects;

    public void IncreaseResource(GameObject Resource)
    {
        Resource.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Resource.transform.parent = null;
        Resource.transform.localPosition = transform.position + new Vector3(0, 20, 0);
        Resource.GetComponent<Rigidbody>().useGravity = false;
        Resource.layer = 11;
        resourceObjects.Add(Resource);
    }

    public GameObject DecreaseResource()
    {
        var resource = resourceObjects[Random.Range(0, resourceObjects.Count)];
        resourceObjects.Remove(resource);
        return resource;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("agent"))
        {
            other.GetComponent<NavCollector>().OnEnterZone(gameObject);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("agent"))
        {
            other.GetComponent<NavCollector>().OnEnterZone(gameObject);
        }
    }
}
