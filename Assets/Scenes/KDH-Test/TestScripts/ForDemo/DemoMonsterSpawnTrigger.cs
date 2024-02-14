using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoMonsterSpawnTrigger : MonoBehaviour
{
    private GameObject[] monsters;
    private void Awake()
    {
        monsters = new GameObject[transform.childCount];
        for(int count = 0; count < transform.childCount; count++)
        {
            monsters[count] = transform.GetChild(count).gameObject;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach(GameObject monster in monsters)
            {
                monster.SetActive(true);
            }
        }
    }
}
