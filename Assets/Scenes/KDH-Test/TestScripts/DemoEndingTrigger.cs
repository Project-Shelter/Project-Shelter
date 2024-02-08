using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoEndingTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Managers.Instance.GameOverAction?.Invoke();
        }
    }
}
