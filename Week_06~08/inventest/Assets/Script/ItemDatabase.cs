using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();

    // 싱글톤 패턴 구현
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

        // 아이템 데이터 로드 (예: JSON 파일에서 로드)
        LoadItems();
    }

    private void LoadItems()
    {
        // 여기서는 예시로 직접 아이템을 생성합니다.
        // 실제 게임에서는 JSON이나 ScriptableObject 등을 사용하여 데이터를 관리하는 것이 좋습니다.

        // 무기 아이템 예시
        items.Add(new Item(
            1,
            "철검",
            "기본적인 철로 만든 검입니다.",
            Resources.Load<Sprite>("Icons/Sword"),
            Item.ItemType.Weapon
        ));

        // 방어구 아이템 예시
        items.Add(new Item(
            2,
            "가죽 갑옷",
            "기본적인 가죽 갑옷입니다.",
            Resources.Load<Sprite>("Icons/Armor"),
            Item.ItemType.Armor
        ));

        // 포션 아이템 예시 (스택 가능)
        items.Add(new Item(
            3,
            "체력 포션",
            "체력을 30 회복합니다.",
            Resources.Load<Sprite>("Icons/HealthPotion"),
            Item.ItemType.Potion,
            true,
            10
        ));

        // 재료 아이템 예시 (스택 가능)
        items.Add(new Item(
            4,
            "나무",
            "기본적인 제작 재료입니다.",
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

        Debug.LogWarning($"ID {id}에 해당하는 아이템을 찾을 수 없습니다.");
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