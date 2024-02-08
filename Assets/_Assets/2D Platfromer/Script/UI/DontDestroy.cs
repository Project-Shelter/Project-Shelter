using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("UI Canvas").Length > 1) Destroy(GameObject.FindGameObjectWithTag("UI Canvas"));
        DontDestroyOnLoad(this.gameObject);
    }
}