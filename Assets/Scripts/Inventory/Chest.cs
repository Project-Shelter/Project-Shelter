using System;
using System.Collections;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2.Requests;
using ItemContainer;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour, IInteractable
{
    private int chestNum;
    private bool isChestOpened = false;
    private bool isChestOpening = false;

    private PopupContainer chest;
    private PopupContainer inventory;

    [SerializeField] private Slider timeSlider;
    [SerializeField] private float chestOpenTime = 2.0f;
    private float openingTime = 0f;

    private Actor actor;
    private Coroutine tryOpenCoroutine;

    private void Start()
    {
        GameObject root = Util.FindChild(Managers.UI.Root, "UI_Inventory", true);

        chestNum = int.Parse(gameObject.name.Substring(gameObject.name.Length - 1));
        timeSlider.maxValue = chestOpenTime;
        chest ??= go.transform.GetChild(3).GetComponent<PopupContainer>();
        inventory ??= go.transform.GetChild(2).GetComponent<PopupContainer>();
    }

    private void Update()
    {
        if (!(isChestOpening || isChestOpened) && openingTime > 0) 
        {
            openingTime = Mathf.MoveTowards(openingTime, 0f, Time.deltaTime); 
            timeSlider.value = openingTime;
        }
    }

    public void Interact(Actor actor)
    {
        this.actor = actor;
        isChestOpening = true;
        tryOpenCoroutine = actor.StartCoroutine(TryToOpenChest());
    }

    public void StopInteract()
    {
        CloseChest();
        actor.StopCoroutine(tryOpenCoroutine);
    }

    private IEnumerator TryToOpenChest()
    {
        while (isChestOpening)
        {
            openingTime += Time.deltaTime;
            timeSlider.value = openingTime;
            if (openingTime >= chestOpenTime)
            {
                OpenChest();
                isChestOpening = false;
            }
            yield return null;
        }
    }

    private void OpenChest()
    {
        isChestOpened = true;
        UI_Chest.ChangeChest(chestNum);
        chest.Open();
        inventory.Open();
    }

    private void CloseChest()
    {
        isChestOpening = false;
        isChestOpened = false;
        chest.Close();
        inventory.Close();
    }
}