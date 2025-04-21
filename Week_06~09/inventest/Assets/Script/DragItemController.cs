using UnityEngine;
using UnityEngine.UI;

public class DragItemController : MonoBehaviour
{
    [SerializeField] private GameObject dragItemPrefab;
    [SerializeField] private Canvas parentCanvas;

    private GameObject draggedItem;
    private Image draggedItemImage;
    private Inventory playerInventory;

    private void Start()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        // �巡�� �̺�Ʈ ����
        InventorySlotUI.OnBeginDragEvent += OnBeginDrag;
        InventorySlotUI.OnEndDragEvent += OnEndDrag;
        InventorySlotUI.OnSwapEvent += OnSwapItems;

        // �巡�� ������ ����
        CreateDraggedItem();
    }

    private void OnDestroy()
    {
        // �̺�Ʈ ���� ����
        InventorySlotUI.OnBeginDragEvent -= OnBeginDrag;
        InventorySlotUI.OnEndDragEvent -= OnEndDrag;
        InventorySlotUI.OnSwapEvent -= OnSwapItems;
    }

    private void Update()
    {
        // �巡�� ���� ������ ��ġ ������Ʈ
        if (draggedItem.activeSelf)
        {
            draggedItem.transform.position = Input.mousePosition;
        }
    }

    private void CreateDraggedItem()
    {
        // �巡�� ������ ����
        draggedItem = Instantiate(dragItemPrefab, parentCanvas.transform);
        draggedItemImage = draggedItem.GetComponent<Image>();
        draggedItem.SetActive(false);
    }

    private void OnBeginDrag(InventorySlotUI slotUI)
    {
        // �巡�� ���� �� ������ ǥ��
        InventorySlot slot = playerInventory.GetInventorySlots()[slotUI.SlotIndex];
        if (!slot.IsEmpty())
        {
            draggedItemImage.sprite = slot.item.icon;
            draggedItemImage.enabled = true;
            draggedItem.SetActive(true);
        }
    }

    private void OnEndDrag(InventorySlotUI slotUI)
    {
        // �巡�� ���� �� ������ �����
        draggedItem.SetActive(false);
    }

    private void OnSwapItems(InventorySlotUI fromSlot, InventorySlotUI toSlot)
    {
        // ������ ����
        playerInventory.SwapItems(fromSlot.SlotIndex, toSlot.SlotIndex);

        // ���̶���Ʈ ������Ʈ
        fromSlot.SetHighlight(false);
        toSlot.SetHighlight(true);
    }
}