using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class DropletGenerator : MonoBehaviour
{
    [SerializeField] GameObject dropletsParent;
    [SerializeField] List<GameObject> droplets;
    List<float> dropletTimers;
    List<Vector2> startPos;
    List<float> posOffset;

    [SerializeField] float minTime = .2f;
    [SerializeField] float maxTime = 1f;
    [SerializeField] float maxOffset;

    //[SerializeField] GameObject[] dropletObject;
    [SerializeField] RuntimeAnimatorController[] animators;

    private void Start()
    {
        droplets = new List<GameObject>();
        for (int i = 0; i < dropletsParent.transform.childCount; i++)
        {
            droplets.Add(dropletsParent.transform.GetChild(i).gameObject);
        }

        dropletTimers = new List<float>();
        for (int i = 0; i < droplets.Count; i++)
        {
            dropletTimers.Add(0f);
        }

        posOffset = new List<float>();
        for (int i = 0; i < dropletTimers.Count; i++)
        {
            posOffset.Add(0f);
        }

        for (int i = 0; i < dropletTimers.Count; i++)
        {
            dropletTimers[i] = Random.Range(minTime, maxTime);
        }

        startPos = new List<Vector2>();
        for (int i = 0; i < posOffset.Count; i++)
        {
            startPos.Add(dropletsParent.transform.GetChild(i).position);
        }
    }

    private void Update()
    {
        for (int i = 0; i < dropletTimers.Count; i++)
        {
            dropletTimers[i] -= Time.deltaTime;
            if (dropletTimers[i] <= 0 && droplets[i].activeSelf == false)
            {
                droplets[i].GetComponent<Animator>().runtimeAnimatorController = animators[Random.Range(0, animators.Length)];
                posOffset[i] = Random.Range(-maxOffset, maxOffset);
                droplets[i].transform.position = new(startPos[i].x + posOffset[i], startPos[i].y + posOffset[i]);
                droplets[i].SetActive(true);
                dropletTimers[i] = dropletTimers[i] = Random.Range(minTime, maxTime);
            }
        }
    }
}
