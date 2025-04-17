using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int amount;

    public InventorySlot()
    {
        item = null;
        amount = 0;
    }

    public InventorySlot(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public bool IsEmpty()
    {
        return item == null;
    }

    public bool CanAddItem(Item newItem)
    {
        if (IsEmpty())
            return true;

        if (item.id == newItem.id && item.isStackable && amount < item.maxStackSize)
            return true;

        return false;
    }

    public void AddItem(Item newItem, int count = 1)
    {
        if (IsEmpty())
        {
            item = newItem.Clone();
            amount = count;
        }
        else if (item.id == newItem.id && item.isStackable)
        {
            amount = Mathf.Min(amount + count, item.maxStackSize);
        }
    }

    public void RemoveItem(int count = 1)
    {
        amount -= count;
        if (amount <= 0)
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        item = null;
        amount = 0;
    }
}