using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{

    public bool collected = false;
    public bool held = false;
    public GameObject resourceArea;

    AudioSource OnStartSound;

    private void Awake()
    {
        OnStartSound = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter()
    {
        OnStartSound.Play();
    }
}
