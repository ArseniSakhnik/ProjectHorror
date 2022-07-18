using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnder : Interactable
{
    public PlayerController ctr;
    public Inventory inv;

    public override void OnFocus()
    {
    }

    public override void OnInteract()
    {
        if (inv.phase == 3)
        {
            PlayerController.isblockInteraction = true;
            PlayerController.isblockInventory = true;
            PlayerController.isblockReading = true;
            PlayerController.isblockShooting = true;
            ctr.isDead = true;
            ctr.StartCoroutine(ctr.DeathHandler("You Survived!"));
        }
        else {
            GameObject.Find("SubtitlesInfo").GetComponent<TMPro.TextMeshProUGUI>().text = "I dont fell safe";
            StartCoroutine(ExecuteAfterTime(2));
        }
    }

    public IEnumerator ExecuteAfterTime(float timeInSec)
    {
        yield return new WaitForSeconds(timeInSec);
        if (GameObject.Find("SubtitlesInfo").GetComponent<TMPro.TextMeshProUGUI>().text == "I dont fell safe")
        {
            GameObject.Find("SubtitlesInfo").GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }
    }


    public override void OnLoseFocus()
    {
    }

    void Start()
    {
        ctr = GameObject.Find("Player").GetComponent<PlayerController>();
        inv = GameObject.Find("Player").GetComponent<Inventory>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
