using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceArea : MonoBehaviour
{


    private int resourcesInside = 10;

    public float respawnTime = 20f;

    public GameObject ResourcePrefab;
    IEnumerator Start()
    {
        for (int i = 0; i < resourcesInside; i++)
        {
            yield return new WaitForSeconds(Random.Range(0f, 5f));
            SpawnResource();
        }
        StartCoroutine(PassiveResourceSpawn());
    }
    IEnumerator RandomlySpawnFive()
    {
        resourcesInside += 5;
        SpawnResource();
        yield return new WaitForSeconds(Random.Range(0f, 5f));
        SpawnResource();
        yield return new WaitForSeconds(Random.Range(0f, 5f));
        SpawnResource();
        yield return new WaitForSeconds(Random.Range(0f, 5f));
        SpawnResource();
        yield return new WaitForSeconds(Random.Range(0f, 5f));
        SpawnResource();
    }
    IEnumerator PassiveResourceSpawn()
    {

        yield return new WaitForSeconds(30);
        StartCoroutine(RandomlySpawnFive());
        PassiveResourceSpawn();
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

            yield return new WaitForSeconds(respawnTime);
            StartCoroutine(RandomlySpawnFive());
        }


    }

    public void feed()
    {
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
