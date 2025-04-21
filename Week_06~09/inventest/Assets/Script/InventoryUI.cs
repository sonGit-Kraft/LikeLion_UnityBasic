using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform slotGrid;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject itemDetailPanel;

    [SerializeField] private Image detailIcon;
    [SerializeField] private TextMeshProUGUI detailName;
    [SerializeField] private TextMeshProUGUI detailDescription;
    [SerializeField] private TextMeshProUGUI detailType;

    private Inventory playerInventory;
    private List<InventorySlotUI> slotUIs = new List<InventorySlotUI>();
    private InventorySlotUI selectedSlot;

    private void Start()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged += UpdateInventoryUI;
            InitializeInventoryUI();
        }
        else
        {
            Debug.LogError("플레이어 인벤토리를 찾을 수 없습니다!");
        }

        // 초기에는 아이템 상세 패널 숨기기
        itemDetailPanel.SetActive(false);
    }

    private void Update()
    {
        // 인벤토리 토글 (예: I 키)
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    private void InitializeInventoryUI()
    {
        // 기존 슬롯 UI 제거
        foreach (Transform child in slotGrid)
        {
            Destroy(child.gameObject);
        }
        slotUIs.Clear();

        // 새 슬롯 UI 생성
        for (int i = 0; i < playerInventory.GetInventorySize(); i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotGrid);
            InventorySlotUI slotUI = slotGO.GetComponent<InventorySlotUI>();

            if (slotUI != null)
            {
                slotUI.SetSlotIndex(i);
                slotUI.OnSlotClicked += OnSlotSelected;
                slotUIs.Add(slotUI);
            }
        }

        UpdateInventoryUI(playerInventory.GetInventorySlots());
    }

    private void UpdateInventoryUI(List<InventorySlot> slots)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < slotUIs.Count)
            {
                slotUIs[i].UpdateSlot(slots[i]);
            }
        }
    }

    private void OnSlotSelected(InventorySlotUI slotUI)
    {
        selectedSlot = slotUI;

        // 선택된 슬롯에 아이템이 있으면 상세 정보 표시
        InventorySlot slot = playerInventory.GetInventorySlots()[slotUI.SlotIndex];
        if (!slot.IsEmpty())
        {
            ShowItemDetails(slot.item);
        }
        else
        {
            itemDetailPanel.SetActive(false);
        }
    }

    private void ShowItemDetails(Item item)
    {
        detailIcon.sprite = item.icon;
        detailName.text = item.itemName;
        detailDescription.text = item.description;
        detailType.text = "유형: " + item.itemType.ToString();

        itemDetailPanel.SetActive(true);
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);

        // 인벤토리를 닫을 때 상세 패널도 닫기
        if (!inventoryPanel.activeSelf)
        {
            itemDetailPanel.SetActive(false);
        }
    }

    public void UseSelectedItem()
    {
        if (selectedSlot != null)
        {
            // 여기에 아이템 사용 로직 구현
            // 예: playerInventory.UseItem(selectedSlot.SlotIndex);

            // 임시로 아이템 제거로 구현
            playerInventory.RemoveItem(selectedSlot.SlotIndex, 1);
        }
    }

    public void DropSelectedItem()
    {
        if (selectedSlot != null)
        {
            // 여기에 아이템 드롭 로직 구현
            // 예: playerInventory.DropItem(selectedSlot.SlotIndex);

            // 임시로 아이템 제거로 구현
            playerInventory.RemoveItem(selectedSlot.SlotIndex, 1);
        }
    }
}