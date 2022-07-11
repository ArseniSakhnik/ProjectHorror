using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [SerializeField] public List<Item> inventoryItems = new();
    [SerializeField] public List<Item> notesItems = new();
    [SerializeField] public List<GameObject> Weapons = new();
    [SerializeField] public GameObject NOTEUI;
    [SerializeField] public int pgc;



    [SerializeField] List<Item> allObjects = new();     // to-do Â‡ÎËÁÓ‚‡Ú¸ ÒÚË‡ÌËÂ

    public string GetInfoAmmo(string WeaponName)
    {
        if (WeaponName == "Mosin Rifle")
        {
            return Weapons[0].GetComponent<WeaponScript>().currentAmmo + " / " + Weapons[0].GetComponent<WeaponScript>().magSize;
        }
        else if(WeaponName == "Nagan")
        {
            return Weapons[1].GetComponent<WeaponScript>().currentAmmo + " / " + Weapons[1].GetComponent<WeaponScript>().magSize;
        }
        return "0";
    }

    public void SetWeapon(string WeaponName)
    {
        if (WeaponName == "Mosin Rifle")
        {
            Weapons[0].SetActive(!Weapons[0].activeInHierarchy);
            Weapons[1].SetActive(false);
        }
        else if(WeaponName == "Nagan")
        {
            Weapons[1].SetActive(!Weapons[1].activeInHierarchy);
            Weapons[0].SetActive(false);
        }
        var obj1 = Weapons[0];
        var obj2 = Weapons[1];
        print("¬»Õ“Œ¬ ¿ "+obj1.GetComponent<WeaponScript>().currentAmmo + " / " + obj1.GetComponent<WeaponScript>().magSize);

        print("Õ¿√¿Õ "+obj2.GetComponent<WeaponScript>().currentAmmo + " / " + obj2.GetComponent<WeaponScript>().magSize);

    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in inventoryItems)
        {
            item.Quantity = 1;
        }
    }


    public IEnumerator ExitFromReading()
    {
        FindObjectOfType<AudioManager>().Play("Page");
        int currentpage = 1;

        while (!Input.GetKeyDown(KeyCode.R))
        {
            pgc = GameObject.Find("NotesText").GetComponent<TMPro.TextMeshProUGUI>().textInfo.pageCount;

            if (pgc > 1)    GameObject.Find("PageCounter").GetComponent<TMPro.TextMeshProUGUI>().text = currentpage + " / " + pgc;
            else GameObject.Find("PageCounter").GetComponent<TMPro.TextMeshProUGUI>().text = "";

            if (currentpage < pgc && Input.GetKeyDown(KeyCode.D))
            {
                currentpage++;
                GameObject.Find("NotesText").GetComponent<TMPro.TextMeshProUGUI>().pageToDisplay++;
            }

            else if (currentpage > 1 && Input.GetKeyDown(KeyCode.A))
            {
                currentpage--;
                GameObject.Find("NotesText").GetComponent<TMPro.TextMeshProUGUI>().pageToDisplay--;
            }
            yield return null;
        }

        GameObject.Find("NotesText").GetComponent<TMPro.TextMeshProUGUI>().pageToDisplay = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameObject.Find("NotesText").GetComponent<TMPro.TextMeshProUGUI>().text = "";
        GameObject.Find("PageCounter").GetComponent<TMPro.TextMeshProUGUI>().text = "";
        NOTEUI.SetActive(false);
        Time.timeScale = 1f;
        PlayerController.isblockInventory = false;
        PlayerController.isblockInteraction = false;
        PlayerController.isblockShooting = false;
        pgc = 0;

    }


    public void ReadNote(Item item)
    {
        if (PlayerController.isblockReading) return;
        NOTEUI.SetActive(true);
        string noteText = item.File.ToString();
        GameObject.Find("NotesText").GetComponent<TMPro.TextMeshProUGUI>().text = noteText;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0f;
        PlayerController.isblockInventory = true;
        PlayerController.isblockInteraction = true;
        PlayerController.isblockShooting = true;
        StartCoroutine(ExitFromReading());
        Debug.Log(item);


    }

    public void AddItem(Item item, int amount = 1)
    {

        if (item.Type == ItemType.Note)
        {
            notesItems.Add(item);
            Debug.Log("+Note");
            return;
        }

        if (inventoryItems.Contains(item))
        {

            inventoryItems[inventoryItems.IndexOf(item)].Quantity += amount;
            Debug.Log("Quantity++");
        }
        else
        {
            inventoryItems.Add(item);
            inventoryItems[inventoryItems.IndexOf(item)].Quantity += amount;
            Debug.Log("Add");
        }
    }

    public bool CheckItem(Item item)
    {
        return inventoryItems.Contains(item);
    }

    public void RemoveItem(Item item)
    {
        inventoryItems[inventoryItems.IndexOf(item)].Quantity -= 1;
        Debug.Log("Quantity--");

        if (inventoryItems[inventoryItems.IndexOf(item)].Quantity == 0)
        {
            inventoryItems.Remove(item);
            Debug.Log("Remove");
        }
    }

}
