using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public GameObject IntentoryUI;
    public GameObject NotesUI;
    bool isMenuInventory = false;
    bool isMenuNotes = false;

    private void Start()
    {
        IntentoryUI.SetActive(false);
        NotesUI.SetActive(false);
    }

    private void Update()
    {
        StartMenu();
    }
    void StartMenu()
    {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
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
