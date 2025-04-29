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
        // �׽�Ʈ�� ������ �߰�
        AddTestItems();
    }

    private void Update()
    {
        // �׽�Ʈ�� Ű �Է� ó��
        HandleTestInputs();
    }

    private void AddTestItems()
    {
        // �׽�Ʈ�� ������ �߰�
        if (ItemDatabase.Instance != null)
        {
            // ���� �߰�
            Item sword = ItemDatabase.Instance.GetItemById(1);
            if (sword != null)
            {
                inventory.AddItem(sword);
            }

            // �� �߰�
            Item armor = ItemDatabase.Instance.GetItemById(2);
            if (armor != null)
            {
                inventory.AddItem(armor);
            }

            // ���� �߰� (���� ��)
            Item potion = ItemDatabase.Instance.GetItemById(3);
            if (potion != null)
            {
                inventory.AddItem(potion, 5);
            }

            // ��� �߰� (���� ��)
            Item wood = ItemDatabase.Instance.GetItemById(4);
            if (wood != null)
            {
                inventory.AddItem(wood, 20);
            }
        }
    }

    private void HandleTestInputs()
    {
        // 1~4 Ű�� ���� ������ �߰� �׽�Ʈ
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
                    Debug.Log($"{item.itemName}��(��) �κ��丮�� �߰��߽��ϴ�.");
                }
                else
                {
                    Debug.Log("�κ��丮�� ���� á���ϴ�.");
                }
            }
        }
    }

    // ������ ��� �޼��� (������ Ÿ�Կ� ���� �ٸ� ȿ�� ����)
    public void UseItem(int slotIndex)
    {
        InventorySlot slot = inventory.GetInventorySlots()[slotIndex];
        if (!slot.IsEmpty())
        {
            Item item = slot.item;

            switch (item.itemType)
            {
                case Item.ItemType.Weapon:
                    // ���� ���� ����
                    Debug.Log($"{item.itemName}��(��) �����߽��ϴ�.");
                    break;

                case Item.ItemType.Armor:
                    // �� ���� ����
                    Debug.Log($"{item.itemName}��(��) �����߽��ϴ�.");
                    break;

                case Item.ItemType.Potion:
                    // ���� ��� ���� (��: ü�� ȸ��)
                    Debug.Log($"{item.itemName}��(��) ����߽��ϴ�. ü���� ȸ���Ǿ����ϴ�.");
                    // ��� �� ������ ����
                    inventory.RemoveItem(slotIndex, 1);
                    break;

                case Item.ItemType.Material:
                    // ���� ���� ����� �� ����
                    Debug.Log($"{item.itemName}��(��) ���� ����� �� ���� �������Դϴ�.");
                    break;

                case Item.ItemType.Quest:
                    // ����Ʈ ������ ��� ����
                    Debug.Log($"{item.itemName}��(��) ����߽��ϴ�. ����Ʈ ���� ��Ȳ�� ������Ʈ�Ǿ����ϴ�.");
                    break;

                default:
                    // ��Ÿ ������
                    Debug.Log($"{item.itemName}��(��) ����߽��ϴ�.");
                    break;
            }
        }
    }

    // ������ ��� �޼���
    public void DropItem(int slotIndex)
    {
        InventorySlot slot = inventory.GetInventorySlots()[slotIndex];
        if (!slot.IsEmpty())
        {
            // ���⿡ �������� ���忡 �����ϴ� ���� �߰�
            Debug.Log($"{slot.item.itemName}��(��) ���Ƚ��ϴ�.");

            // �κ��丮���� ������ ����
            inventory.RemoveItem(slotIndex, 1);
        }
    }
}