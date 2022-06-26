using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public GameObject IntentoryUI;
    public GameObject NotesUI;
    bool isMenuInventory = false;
    bool isMenuNotes = false;
    [SerializeField] private PlayerController ctr;
    [SerializeField] private Inventory inventory;
    [SerializeField] GameObject centerImage;
    [SerializeField] GameObject centerImageName;
    [SerializeField] GameObject centerImageDescr;
    [SerializeField] GameObject centerImageAmount;
    [SerializeField] GameObject nextImage;
    [SerializeField] GameObject prevImage;
    [SerializeField] private int maxCells;
    [SerializeField] private int centralcell;
    [SerializeField] private int nextcell;
    [SerializeField] private int prevcell;
    [SerializeField] private Item currentItem;
    [SerializeField] GameObject AmountText;
    [SerializeField] List<Item> itemList;




    private void Start()
    {
        ctr = GameObject.Find("Player").GetComponent<PlayerController>();
        inventory = ctr.inventory;
        itemList = inventory.inventoryItems;

        RefreshInventory();

        IntentoryUI.SetActive(false);
    }



    private void RefreshInventory()
    {



        if (itemList.Count == 0 )
        {
            prevImage.SetActive(false);
            nextImage.SetActive(false);
            centerImage.SetActive(false);
            AmountText.GetComponent<TMPro.TextMeshProUGUI>().text = "";

            centerImageName.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            centerImageDescr.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            centerImageAmount.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            return;
        }

        AmountText.GetComponent<TMPro.TextMeshProUGUI>().text = "Количество:";

        if (maxCells != itemList.Count)
        {
            maxCells = itemList.Count;              // set cells
            centralcell = itemList.Count / 2;
            prevcell = centralcell - 1;
            nextcell = centralcell + 1;
        }


        centerImage.GetComponent<UnityEngine.UI.Image>().sprite = itemList[centralcell].Icon; // set images
        centerImageName.GetComponent<TMPro.TextMeshProUGUI>().text = itemList[centralcell].name;
        centerImageDescr.GetComponent<TMPro.TextMeshProUGUI>().text = itemList[centralcell].Description;
        centerImageAmount.GetComponent<TMPro.TextMeshProUGUI>().text = itemList[centralcell].Quantity.ToString();

        if (maxCells != 1)
        {
            if (maxCells == 2)
            {
                nextcell = prevcell;
            }
            prevImage.GetComponent<UnityEngine.UI.Image>().sprite = itemList[prevcell].Icon;
            nextImage.GetComponent<UnityEngine.UI.Image>().sprite = itemList[nextcell].Icon;
        }

        if (maxCells == 1)
        {
            centerImage.SetActive(true);
            prevImage.SetActive(false);
            nextImage.SetActive(false);
        }
        else
        {
            centerImage.SetActive(true);
            prevImage.SetActive(true);
            nextImage.SetActive(true);
        }

    }

    public void MoveRight()
    {

        prevcell = centralcell;
        centralcell = nextcell;
        nextcell++;
        if (nextcell == maxCells)
        {
            nextcell = 0;
        }
        RefreshInventory();
    }

    public void MoveLeft()
    {
        nextcell = centralcell;
        centralcell = prevcell;
        prevcell--;
        if (prevcell < 0)
        {
            prevcell = maxCells - 1;
        }
        RefreshInventory();
    }

    public void UseItem()
    {
        if (itemList[centralcell].Type == ItemType.Note)
        {
            PlayerController.isblockReading = false;
            PlayerController.isblockInventory = true;
            IntentoryUI.SetActive(false);
            inventory.ReadNote(itemList[centralcell]);
        }

        switch (centerImageName.GetComponent<TMPro.TextMeshProUGUI>().text)
        {
            case "HealthDrink":                         // drink to restore health
                ctr.playerHealth += 40;
                if (ctr.playerHealth > 100)
                {
                    ctr.playerHealth = 100;
                }
                inventory.RemoveItem(itemList[centralcell]);
                break;

            default:
                break;
        }
        RefreshInventory();
    }

    private void Update()
    {
        if (PlayerController.isblockInventory)
        {
            return;

        }
        StartMenu();

        if (inventory.inventoryItems.Count == 0) // если ноль то ничего не прожимаем
        {
            return;
        }

        if (isMenuInventory || isMenuNotes)
        {
            if (inventory.inventoryItems.Count != 1)
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    MoveRight();
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    MoveLeft();
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                UseItem();
            }
        }
    }


    void StartMenu()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PlayerController.isblockInteraction = true;
            PlayerController.isblockReading = true;
            RefreshInventory();
            if (isMenuNotes == false)
            {
                isMenuInventory = !isMenuInventory;
            }

        }
        if (isMenuInventory || isMenuNotes)
        {
            IntentoryUI.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }

        else
        {
            IntentoryUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
            PlayerController.isblockInteraction = false;
            PlayerController.isblockReading = false;

        }
    }

    // Методы для инспектора
    public void MenuUnpause()
    {
        isMenuInventory = false;
        isMenuNotes = false;
    }
    public void SetInventory()
    {
        itemList = inventory.inventoryItems;
        RefreshInventory();

        isMenuInventory = true;
        isMenuNotes = false;
    }
    public void SetNotes()
    {
        itemList = inventory.notesItems;
        RefreshInventory();

        isMenuInventory = false;
        isMenuNotes = true;
    }
}
