using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int inventorySize = 20;
    [SerializeField] private List<InventorySlot> slots;

    public event Action<List<InventorySlot>> OnInventoryChanged;

    private void Awake()
    {
        InitializeInventory();
    }

    private void InitializeInventory()
    {
        slots = new List<InventorySlot>(inventorySize);
        for (int i = 0; i < inventorySize; i++)
        {
            slots.Add(new InventorySlot());
        }
    }

    public bool AddItem(Item item, int amount = 1)
    {
        // ���� �̹� �ִ� �����ۿ� ���� �������� Ȯ��
        if (item.isStackable)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].CanAddItem(item))
                {
                    slots[i].AddItem(item, amount);
                    OnInventoryChanged?.Invoke(slots);
                    return true;
                }
            }
        }

        // �� ���� ã��
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].IsEmpty())
            {
                slots[i].AddItem(item, amount);
                OnInventoryChanged?.Invoke(slots);
                return true;
            }
        }

        // �κ��丮�� ���� á�� ���
        Debug.Log("�κ��丮�� ���� á���ϴ�!");
        return false;
    }

    public void RemoveItem(int slotIndex, int amount = 1)
    {
        if (slotIndex >= 0 && slotIndex < slots.Count && !slots[slotIndex].IsEmpty())
        {
            slots[slotIndex].RemoveItem(amount);
            OnInventoryChanged?.Invoke(slots);
        }
    }

    public void SwapItems(int fromSlotIndex, int toSlotIndex)
    {
        if (fromSlotIndex >= 0 && fromSlotIndex < slots.Count &&
            toSlotIndex >= 0 && toSlotIndex < slots.Count)
        {
            InventorySlot tempSlot = new InventorySlot(slots[fromSlotIndex].item, slots[fromSlotIndex].amount);
            slots[fromSlotIndex] = slots[toSlotIndex];
            slots[toSlotIndex] = tempSlot;
            OnInventoryChanged?.Invoke(slots);
        }
    }

    public List<InventorySlot> GetInventorySlots()
    {
        return slots;
    }

    public int GetInventorySize()
    {
        return inventorySize;
    }
}