using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiddleNumLock : Interactable
{
    public GameObject RiddleUI;
    public string solution;
    public GameObject targetDoor;



    public override void OnFocus()
    {
    }

    public override void OnInteract()
    {
        if (RiddleUI.GetComponent<UIRiddleNum>().parentObject != gameObject)
        {
            RiddleUI.GetComponent<UIRiddleNum>().SetParentData(gameObject);
        }


        RiddleUI.SetActive(true);
        StartCoroutine(WaitForInput());
        PlayerController.isblockInteraction = true;
        PlayerController.isblockReading = true;
        PlayerController.isblockInventory = true;
        PlayerController.isblockShooting = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    IEnumerator WaitForInput()
    {
        while (!Input.GetKeyDown(KeyCode.R))
        {
            yield return null;
        }
        PlayerController.isblockInteraction = false;
        PlayerController.isblockReading = false;
        PlayerController.isblockInventory = false;
        PlayerController.isblockShooting = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        RiddleUI.SetActive(false);
    }

    public override void OnLoseFocus()
    {
    }



}
