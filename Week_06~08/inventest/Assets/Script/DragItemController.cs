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

        // 드래그 이벤트 구독
        InventorySlotUI.OnBeginDragEvent += OnBeginDrag;
        InventorySlotUI.OnEndDragEvent += OnEndDrag;
        InventorySlotUI.OnSwapEvent += OnSwapItems;

        // 드래그 아이템 생성
        CreateDraggedItem();
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        InventorySlotUI.OnBeginDragEvent -= OnBeginDrag;
        InventorySlotUI.OnEndDragEvent -= OnEndDrag;
        InventorySlotUI.OnSwapEvent -= OnSwapItems;
    }

    private void Update()
    {
        // 드래그 중인 아이템 위치 업데이트
        if (draggedItem.activeSelf)
        {
            draggedItem.transform.position = Input.mousePosition;
        }
    }

    private void CreateDraggedItem()
    {
        // 드래그 아이템 생성
        draggedItem = Instantiate(dragItemPrefab, parentCanvas.transform);
        draggedItemImage = draggedItem.GetComponent<Image>();
        draggedItem.SetActive(false);
    }

    private void OnBeginDrag(InventorySlotUI slotUI)
    {
        // 드래그 시작 시 아이템 표시
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
        // 드래그 종료 시 아이템 숨기기
        draggedItem.SetActive(false);
    }

    private void OnSwapItems(InventorySlotUI fromSlot, InventorySlotUI toSlot)
    {
        // 아이템 스왑
        playerInventory.SwapItems(fromSlot.SlotIndex, toSlot.SlotIndex);

        // 하이라이트 업데이트
        fromSlot.SetHighlight(false);
        toSlot.SetHighlight(true);
    }
}