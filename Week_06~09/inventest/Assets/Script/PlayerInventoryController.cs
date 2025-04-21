using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    private Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        if (inventory == null)
        {
            inventory = gameObject.AddComponent<Inventory>();
        }
    }

    private void Start()
    {
        // 테스트용 아이템 추가
        AddTestItems();
    }

    private void Update()
    {
        // 테스트용 키 입력 처리
        HandleTestInputs();
    }

    private void AddTestItems()
    {
        // 테스트용 아이템 추가
        if (ItemDatabase.Instance != null)
        {
            // 무기 추가
            Item sword = ItemDatabase.Instance.GetItemById(1);
            if (sword != null)
            {
                inventory.AddItem(sword);
            }

            // 방어구 추가
            Item armor = ItemDatabase.Instance.GetItemById(2);
            if (armor != null)
            {
                inventory.AddItem(armor);
            }

            // 포션 추가 (여러 개)
            Item potion = ItemDatabase.Instance.GetItemById(3);
            if (potion != null)
            {
                inventory.AddItem(potion, 5);
            }

            // 재료 추가 (여러 개)
            Item wood = ItemDatabase.Instance.GetItemById(4);
            if (wood != null)
            {
                inventory.AddItem(wood, 20);
            }
        }
    }

    private void HandleTestInputs()
    {
        // 1~4 키를 눌러 아이템 추가 테스트
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddItemById(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddItemById(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AddItemById(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AddItemById(4);
        }
    }

    private void AddItemById(int itemId)
    {
        if (ItemDatabase.Instance != null)
        {
            Item item = ItemDatabase.Instance.GetItemById(itemId);
            if (item != null)
            {
                bool added = inventory.AddItem(item);
                if (added)
                {
                    Debug.Log($"{item.itemName}을(를) 인벤토리에 추가했습니다.");
                }
                else
                {
                    Debug.Log("인벤토리가 가득 찼습니다.");
                }
            }
        }
    }

    // 아이템 사용 메서드 (아이템 타입에 따라 다른 효과 적용)
    public void UseItem(int slotIndex)
    {
        InventorySlot slot = inventory.GetInventorySlots()[slotIndex];
        if (!slot.IsEmpty())
        {
            Item item = slot.item;

            switch (item.itemType)
            {
                case Item.ItemType.Weapon:
                    // 무기 장착 로직
                    Debug.Log($"{item.itemName}을(를) 장착했습니다.");
                    break;

                case Item.ItemType.Armor:
                    // 방어구 장착 로직
                    Debug.Log($"{item.itemName}을(를) 장착했습니다.");
                    break;

                case Item.ItemType.Potion:
                    // 포션 사용 로직 (예: 체력 회복)
                    Debug.Log($"{item.itemName}을(를) 사용했습니다. 체력이 회복되었습니다.");
                    // 사용 후 아이템 제거
                    inventory.RemoveItem(slotIndex, 1);
                    break;

                case Item.ItemType.Material:
                    // 재료는 직접 사용할 수 없음
                    Debug.Log($"{item.itemName}은(는) 직접 사용할 수 없는 아이템입니다.");
                    break;

                case Item.ItemType.Quest:
                    // 퀘스트 아이템 사용 로직
                    Debug.Log($"{item.itemName}을(를) 사용했습니다. 퀘스트 진행 상황이 업데이트되었습니다.");
                    break;

                default:
                    // 기타 아이템
                    Debug.Log($"{item.itemName}을(를) 사용했습니다.");
                    break;
            }
        }
    }

    // 아이템 드롭 메서드
    public void DropItem(int slotIndex)
    {
        InventorySlot slot = inventory.GetInventorySlots()[slotIndex];
        if (!slot.IsEmpty())
        {
            // 여기에 아이템을 월드에 생성하는 로직 추가
            Debug.Log($"{slot.item.itemName}을(를) 버렸습니다.");

            // 인벤토리에서 아이템 제거
            inventory.RemoveItem(slotIndex, 1);
        }
    }
}