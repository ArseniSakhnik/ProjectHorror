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




    private void Start()
    {
        ctr = GameObject.Find("Player").GetComponent<PlayerController>();
        inventory = ctr.inventory;

        RefreshInventory();

        IntentoryUI.SetActive(false);
        NotesUI.SetActive(false);
    }

    

    private void RefreshInventory()
    {
        if (maxCells!=inventory.inventoryItems.Count)
        {
            maxCells = inventory.inventoryItems.Count;              // set cells
            centralcell = inventory.inventoryItems.Count / 2;
            prevcell = centralcell - 1;
            nextcell = centralcell + 1;
        }

        centerImage.GetComponent<UnityEngine.UI.Image>().sprite = inventory.inventoryItems[centralcell].Icon; // set images
        prevImage.GetComponent<UnityEngine.UI.Image>().sprite = inventory.inventoryItems[prevcell].Icon;
        nextImage.GetComponent<UnityEngine.UI.Image>().sprite = inventory.inventoryItems[nextcell].Icon;

        centerImageName.GetComponent<TMPro.TextMeshProUGUI>().text = inventory.inventoryItems[centralcell].name;
        centerImageDescr.GetComponent<TMPro.TextMeshProUGUI>().text = inventory.inventoryItems[centralcell].Description;
        centerImageAmount.GetComponent<TMPro.TextMeshProUGUI>().text = inventory.inventoryItems[centralcell].Quantity.ToString();
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
    }

    public void MoveLeft()
    {
        nextcell = centralcell;
        centralcell = prevcell;
        prevcell--;
        if (prevcell < 0)
        {
            prevcell = maxCells-1;
        }
    }

    public void UseItem()
    {
        switch (centerImageName.GetComponent<TMPro.TextMeshProUGUI>().text)
        {
            case "HealthDrink":                         // drink to restore health
                ctr.playerHealth += 40;
                if (ctr.playerHealth > 100)
                {
                    ctr.playerHealth = 100;
                }
                inventory.RemoveItem(inventory.inventoryItems[centralcell]);         
                break;

            default:
                break;
        }
        RefreshInventory();
    }

    private void Update()
    {
        StartMenu();

        if (isMenuInventory)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log("VPravo");
                MoveRight();
                RefreshInventory();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("VPLevo");
                MoveLeft();
                RefreshInventory();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                UseItem();
            }
        }
    }


    void StartMenu()
    {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                RefreshInventory();
                if (isMenuNotes == false)
                {
                    isMenuInventory = !isMenuInventory;
                }
                if (isMenuNotes == true)
                {
                    isMenuNotes = !isMenuNotes;
                }

            }
            if (isMenuInventory)
            {
                IntentoryUI.SetActive(true);
                NotesUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            
                Time.timeScale = 0f;
            }
            else if (isMenuNotes)
            {
                NotesUI.SetActive(true);
                IntentoryUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                Time.timeScale = 0f;
            }
            else
            {
                IntentoryUI.SetActive(false);
                NotesUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
            }
    }

    // ExitButton
    public void MenuUnpause()
    {
        isMenuInventory = false;
        isMenuNotes = false;
    }
    public void SetInventory()
    {
        isMenuInventory = true;
        isMenuNotes = false;
    }
    public void SetNotes()
    {
        isMenuInventory = false;
        isMenuNotes = true;
    }
}