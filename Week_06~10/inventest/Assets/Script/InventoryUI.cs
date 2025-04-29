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
            Debug.LogError("�÷��̾� �κ��丮�� ã�� �� �����ϴ�!");
        }

        // �ʱ⿡�� ������ �� �г� �����
        itemDetailPanel.SetActive(false);
    }

    private void Update()
    {
        // �κ��丮 ��� (��: I Ű)
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    private void InitializeInventoryUI()
    {
        // ���� ���� UI ����
        foreach (Transform child in slotGrid)
        {
            Destroy(child.gameObject);
        }
        slotUIs.Clear();

        // �� ���� UI ����
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

        // ���õ� ���Կ� �������� ������ �� ���� ǥ��
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
        detailType.text = "����: " + item.itemType.ToString();

        itemDetailPanel.SetActive(true);
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);

        // �κ��丮�� ���� �� �� �гε� �ݱ�
        if (!inventoryPanel.activeSelf)
        {
            itemDetailPanel.SetActive(false);
        }
    }

    public void UseSelectedItem()
    {
        if (selectedSlot != null)
        {
            // ���⿡ ������ ��� ���� ����
            // ��: playerInventory.UseItem(selectedSlot.SlotIndex);

            // �ӽ÷� ������ ���ŷ� ����
            playerInventory.RemoveItem(selectedSlot.SlotIndex, 1);
        }
    }

    public void DropSelectedItem()
    {
        if (selectedSlot != null)
        {
            // ���⿡ ������ ��� ���� ����
            // ��: playerInventory.DropItem(selectedSlot.SlotIndex);

            // �ӽ÷� ������ ���ŷ� ����
            playerInventory.RemoveItem(selectedSlot.SlotIndex, 1);
        }
    }
}