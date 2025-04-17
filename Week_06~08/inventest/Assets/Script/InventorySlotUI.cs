using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private GameObject highlightImage;

    private InventorySlot slot;
    private int slotIndex;

    public int SlotIndex => slotIndex;
    public event Action<InventorySlotUI> OnSlotClicked;
    public static event Action<InventorySlotUI> OnBeginDragEvent;
    public static event Action<InventorySlotUI> OnEndDragEvent;
    public static event Action<InventorySlotUI, InventorySlotUI> OnSwapEvent;

    private static InventorySlotUI draggedSlot;

    private void Awake()
    {
        // 초기화
        if (highlightImage != null)
            highlightImage.SetActive(false);
    }

    public void SetSlotIndex(int index)
    {
        slotIndex = index;
    }

    public void UpdateSlot(InventorySlot newSlot)
    {
        slot = newSlot;

        if (slot.IsEmpty())
        {
            itemIcon.enabled = false;
            amountText.gameObject.SetActive(false);
        }
        else
        {
            itemIcon.sprite = slot.item.icon;
            itemIcon.enabled = true;

            if (slot.amount > 1)
            {
                amountText.text = slot.amount.ToString();
                amountText.gameObject.SetActive(true);
            }
            else
            {
                amountText.gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSlotClicked?.Invoke(this);

        // 하이라이트 표시
        if (highlightImage != null)
            highlightImage.SetActive(true);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!slot.IsEmpty())
        {
            draggedSlot = this;
            OnBeginDragEvent?.Invoke(this);

            // 드래그 시작 시 아이콘 반투명하게
            itemIcon.color = new Color(1, 1, 1, 0.5f);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 중 로직 (필요시 구현)
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggedSlot == this)
        {
            draggedSlot = null;
            OnEndDragEvent?.Invoke(this);

            // 드래그 종료 시 아이콘 원래대로
            itemIcon.color = new Color(1, 1, 1, 1f);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (draggedSlot != null && draggedSlot != this)
        {
            OnSwapEvent?.Invoke(draggedSlot, this);
        }
    }

    public void SetHighlight(bool active)
    {
        if (highlightImage != null)
            highlightImage.SetActive(active);
    }
}