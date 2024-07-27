using System;
using System.Collections;
using ItemContainer;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour, IInteractable
{
    private int chestNum;

    private PopupContainer chest;
    private PopupContainer inventory;

    [SerializeField] private Slider timeSlider;
    [SerializeField] private float chestOpenTime = 2.0f;
    private float openingTime = 0f;

    private bool isOpening = false;
    private bool canOpen = false;

    private Actor actor;

    private void Start()
    {
        GameObject root = Util.FindChild(Managers.UI.Root, "UI_Inventory", true);

        chestNum = int.Parse(gameObject.name.Substring(gameObject.name.Length - 1));
        timeSlider.maxValue = chestOpenTime;
        chest ??= root.transform.GetChild(3).GetComponent<PopupContainer>();
        inventory ??= root.transform.GetChild(2).GetComponent<PopupContainer>();
    }

    private void Update()
    {
        bool isOpened = chest.popup.gameObject.activeSelf;
        //print("isOpened: " + isOpened + " isOpening: " + isOpening + " openingTime: " + openingTime);
        if (!(isOpening || isOpened) && openingTime > 0) 
        {
            openingTime = Mathf.MoveTowards(openingTime, 0f, Time.deltaTime); 
            timeSlider.value = openingTime;
        }
    }

    public void StartInteract(Actor actor)
    {
        this.actor = actor;
        isOpening = true;
    }

    public void Interacting()
    {
        openingTime += Time.deltaTime;
        timeSlider.value = openingTime;
        if (openingTime >= chestOpenTime)
        {
            isOpening = false;
        }
    }

    public void StopInteract()
    {
        if (!isOpening)
        {
            OpenChest();
        }
        isOpening = false;
    }

    public bool CanKeepInteracting()
    {
        if (!InputHandler.ButtonE || !isOpening)
        {
            return false;
        }
        return true;
    }

    private void OpenChest()
    {
        UI_Chest.ChangeChest(chestNum);
        chest.Open();
        inventory.Open();
    }
}