using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceArea : MonoBehaviour
{


    private int resourcesInside = 10;

    public float respawnTime = 20f;

    public GameObject ResourcePrefab;
    void Start()
    {
        for (int i = 0; i < resourcesInside; i++)
        {
            SpawnResource();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DecreaseResource()
    {
        StartCoroutine(DecreaseResourceCoroutine());
    }
    IEnumerator DecreaseResourceCoroutine()
    {
        resourcesInside--;


        if (resourcesInside <= 0)
        {
            resourcesInside += 5;
            yield return new WaitForSeconds(respawnTime);
            SpawnResource();
            SpawnResource();
            SpawnResource();
            SpawnResource();
            SpawnResource();
        }


    }

    public void feed()
    {
        SpawnResource();
        SpawnResource();
        SpawnResource();
        SpawnResource();
        SpawnResource();
    }
    private void SpawnResource()
    {
        var resource = Instantiate(ResourcePrefab, new Vector3(Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2), Random.Range(transform.position.y - transform.localScale.y / 2, transform.position.y + transform.localScale.y / 2), Random.Range(transform.position.z - transform.localScale.z / 2, transform.position.z + transform.localScale.z / 2)), Quaternion.identity);
        resource.gameObject.GetComponent<Resource>().resourceArea = transform.gameObject;
    }


    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("resource") && !other.gameObject.GetComponent<Resource>().collected)
    //     {
    //         resourcesInside++;
    //     }
    // }

    // void OnTriggerStay(Collider other)
    // {
    //     if (other.CompareTag("resource") && !other.gameObject.GetComponent<Resource>().collected)
    //     {

    //     }
    // }
    // void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("resource") && !other.gameObject.GetComponent<Resource>().collected)
    //     {
    //         resourcesInside--;
    //     }

    //     if (resourcesInside < 5)
    //     {
    //         Instantiate(ResourcePrefab, new Vector3(Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2), Random.Range(transform.position.y - transform.localScale.y / 2, transform.position.y + transform.localScale.y / 2), Random.Range(transform.position.z - transform.localScale.z / 2, transform.position.z + transform.localScale.z / 2)), Quaternion.identity);
    //         Instantiate(ResourcePrefab, new Vector3(Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2), Random.Range(transform.position.y - transform.localScale.y / 2, transform.position.y + transform.localScale.y / 2), Random.Range(transform.position.z - transform.localScale.z / 2, transform.position.z + transform.localScale.z / 2)), Quaternion.identity);
    //         Instantiate(ResourcePrefab, new Vector3(Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2), Random.Range(transform.position.y - transform.localScale.y / 2, transform.position.y + transform.localScale.y / 2), Random.Range(transform.position.z - transform.localScale.z / 2, transform.position.z + transform.localScale.z / 2)), Quaternion.identity);
    //         Instantiate(ResourcePrefab, new Vector3(Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2), Random.Range(transform.position.y - transform.localScale.y / 2, transform.position.y + transform.localScale.y / 2), Random.Range(transform.position.z - transform.localScale.z / 2, transform.position.z + transform.localScale.z / 2)), Quaternion.identity);
    //         Instantiate(ResourcePrefab, new Vector3(Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2), Random.Range(transform.position.y - transform.localScale.y / 2, transform.position.y + transform.localScale.y / 2), Random.Range(transform.position.z - transform.localScale.z / 2, transform.position.z + transform.localScale.z / 2)), Quaternion.identity);

    //     }
    // }

}
