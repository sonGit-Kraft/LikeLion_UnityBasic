using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;

    public GameObject inventoryPanel;
    bool ActiveInventory = false;

    public Slot[] slots;
    public Transform slotHolder;

    private void Start()
    {
        inven = Inventory.instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inven.onSlotCountChange += SlotChange;
        inven.onChangeItem += RedrawSlotUI;
        inventoryPanel.SetActive(ActiveInventory);
    }

    private void SlotChange(int val)
    {
        for(int i =0; i<slots.Length;i++)
        {
            slots[i].slotnum = i;
            if(i<inven.SlotCnt)
            {
                slots[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                slots[i].GetComponent<Button>().interactable = false;
            }           
        }
    }

    public void AddSlot()
    {
        inven.SlotCnt++;
    }



    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            ActiveInventory = !ActiveInventory;
            inventoryPanel.SetActive(ActiveInventory);
        }

    }

    void RedrawSlotUI()
    {
        for(int i=0; i<slots.Length;i++)
        {
            slots[i].RemoveSlot();
        }
        for(int i =0;i<inven.items.Count; i++)
        {
            slots[i].item = inven.items[i];
            slots[i].UpdateSlotUI();
        }
    }


}
