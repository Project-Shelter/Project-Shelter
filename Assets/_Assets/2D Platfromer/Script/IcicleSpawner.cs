using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleSpawner : MonoBehaviour
{
    public GameObject IciclePrefab;
    public GameObject Icicle;
    public Vector3 spawnPoint;
    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position + new Vector3(0f, 0.75f, 0f);
        StartSpawning();
    }

    // InvokeRepeating
    void StartSpawning()
    {
        InvokeRepeating("Spawn", Random.Range(1f, 10f), 5f);
    }

    // Spawn Icicle
    void Spawn()
    {
        if (Icicle != null) return;
        {
            Icicle = Instantiate(IciclePrefab, spawnPoint, Quaternion.identity) as GameObject;
            Icicle.GetComponent<IcicleController>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // SetUp Icicle
        if (Icicle != null)
        {
            float step = Speed * Time.deltaTime;
            Icicle.transform.position = Vector3.MoveTowards(Icicle.transform.position, transform.position, step);
            if (Icicle.transform.position == transform.position)
            {
                Icicle.GetComponent<IcicleController>().enabled = true;
            }
        }
    }
}