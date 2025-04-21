using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();

    // �̱��� ���� ����
    private static ItemDatabase instance;
    public static ItemDatabase Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<ItemDatabase>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("ItemDatabase");
                    instance = obj.AddComponent<ItemDatabase>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // ������ ������ �ε� (��: JSON ���Ͽ��� �ε�)
        LoadItems();
    }

    private void LoadItems()
    {
        // ���⼭�� ���÷� ���� �������� �����մϴ�.
        // ���� ���ӿ����� JSON�̳� ScriptableObject ���� ����Ͽ� �����͸� �����ϴ� ���� �����ϴ�.

        // ���� ������ ����
        items.Add(new Item(
            1,
            "ö��",
            "�⺻���� ö�� ���� ���Դϴ�.",
            Resources.Load<Sprite>("Icons/Sword"),
            Item.ItemType.Weapon
        ));

        // �� ������ ����
        items.Add(new Item(
            2,
            "���� ����",
            "�⺻���� ���� �����Դϴ�.",
            Resources.Load<Sprite>("Icons/Armor"),
            Item.ItemType.Armor
        ));

        // ���� ������ ���� (���� ����)
        items.Add(new Item(
            3,
            "ü�� ����",
            "ü���� 30 ȸ���մϴ�.",
            Resources.Load<Sprite>("Icons/HealthPotion"),
            Item.ItemType.Potion,
            true,
            10
        ));

        // ��� ������ ���� (���� ����)
        items.Add(new Item(
            4,
            "����",
            "�⺻���� ���� ����Դϴ�.",
            Resources.Load<Sprite>("Icons/Wood"),
            Item.ItemType.Material,
            true,
            50
        ));
    }

    public Item GetItemById(int id)
    {
        foreach (Item item in items)
        {
            if (item.id == id)
            {
                return item.Clone();
            }
        }

        Debug.LogWarning($"ID {id}�� �ش��ϴ� �������� ã�� �� �����ϴ�.");
        return null;
    }

    public List<Item> GetAllItems()
    {
        List<Item> itemsCopy = new List<Item>();
        foreach (Item item in items)
        {
            itemsCopy.Add(item.Clone());
        }
        return itemsCopy;
    }

    public List<Item> GetItemsByType(Item.ItemType type)
    {
        List<Item> filteredItems = new List<Item>();
        foreach (Item item in items)
        {
            if (item.itemType == type)
            {
                filteredItems.Add(item.Clone());
            }
        }
        return filteredItems;
    }
}