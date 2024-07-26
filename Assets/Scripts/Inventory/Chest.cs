using System;
using System.Collections;
using System.Collections.Generic;
using ItemContainer;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    private bool canOpenChest = false;
    private bool isChestOpened = false;
    private Animator animator;
    private PopupContainer chest;
    private PopupContainer inventory;

    [SerializeField] private float chestOpenTime = 2.0f;
    private float openingTime;
    [SerializeField] private Slider slider;
    private int number;
    private void Awake()
    {
        GameObject go = GameObject.Find("UI_Inventory");
        chest ??= go.transform.GetChild(3).GetComponent<PopupContainer>();
        inventory ??= go.transform.GetChild(2).GetComponent<PopupContainer>();
        animator = GetComponent<Animator>();
        number = int.Parse(gameObject.name.Substring(gameObject.name.Length - 1));
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

        UI_Chest.ChangeChest(number);
        chest.Open();
        inventory.Open();
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