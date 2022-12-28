using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropletGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] droplets;
    [SerializeField] List<float> dropletTimers;

    [SerializeField] GameObject dropletObject;

    private void Start()
    {
        dropletTimers = new List<float>();
        for (int i = 0; i < droplets.Length; i++)
        {
            dropletTimers.Add(0f);
        }

        for (int i = 0; i < dropletTimers.Count; i++)
        {
            dropletTimers[i] = Random.Range(1f, 3f);
        }
    }

    private void Update()
    {
        for (int i = 0; i < dropletTimers.Count; i++)
        {
            dropletTimers[i] -= Time.deltaTime;
            if(dropletTimers[i] <= 0)
            {
                droplets[i].SetActive(true);
                dropletTimers[i] = dropletTimers[i] = Random.Range(1f, 3f);
            }
        }
    }
}
