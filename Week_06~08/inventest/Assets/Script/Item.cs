using UnityEngine;

[System.Serializable]
public class Item
{
    public int id;
    public string itemName;
    public string description;
    public Sprite icon;
    public ItemType itemType;
    public bool isStackable;
    public int maxStackSize = 1;

    public enum ItemType
    {
        Weapon,
        Armor,
        Potion,
        Material,
        Quest,
        Misc
    }

    public Item(int id, string name, string desc, Sprite icon, ItemType type, bool stackable = false, int maxStack = 1)
    {
        this.id = id;
        this.itemName = name;
        this.description = desc;
        this.icon = icon;
        this.itemType = type;
        this.isStackable = stackable;
        this.maxStackSize = maxStack;
    }

    // 아이템 복제 메서드
    public Item Clone()
    {
        return new Item(id, itemName, description, icon, itemType, isStackable, maxStackSize);
    }
}