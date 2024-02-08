using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestNearTest : MonoBehaviour
{
    private bool canOpenChest = false;
    private bool isChestOpened = false;
    private Animator animator;

    [SerializeField] private float chestOpenTime = 2.0f;
    private float openingTime;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        openingTime = 0f;
        slider.maxValue = chestOpenTime;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.E) && (canOpenChest && !isChestOpened))
        {
            if (openingTime < chestOpenTime) { openingTime += Time.deltaTime; }
            if (openingTime >= chestOpenTime)
            {
                OpenChest();
            }
        }
        else if(Input.GetKeyDown(KeyCode.E) && (isChestOpened && canOpenChest))
        {
            OpenChest();
        }
        else
        {
            if (!isChestOpened && openingTime >= Time.deltaTime) { openingTime -= Time.deltaTime; }
        }
        slider.value = openingTime;
    }

    private void OpenChest()
    {
        isChestOpened = true;
        animator.SetBool("IsOpened", true);
        ItemDummyData.ChangeCurrentChest(gameObject.name[gameObject.name.Length - 1] - '0');
        Inventory.Instance.OpenChest.Invoke(ItemDummyData.CurrentChest);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Actor actor = other.GetComponent<Actor>();
        if (actor != null && actor == ActorController.Instance.CurrentActor)
        {
            canOpenChest = true; 
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Actor actor = other.GetComponent<Actor>();
        if (actor != null && actor == ActorController.Instance.CurrentActor)
        {
            canOpenChest = false; 
        }
    }
}
