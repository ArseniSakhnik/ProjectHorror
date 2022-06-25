using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : Interactable
{
    [SerializeField] Item item;
    [SerializeField] public PlayerController ctr;
    [SerializeField] public int amount;


    public override void OnFocus()
    {
    }

    public override void OnInteract()
    {
        GameObject.Find("SubtitlesInfo").GetComponent<TMPro.TextMeshProUGUI>().text = "Picked Up <color=green>" + item.name + "</color>";
        ctr = GameObject.Find("Player").GetComponent<PlayerController>();
        ctr.inventory.AddItem(item, amount);
        StartCoroutine(ExecuteAfterTime(3));
        gameObject.transform.position = new Vector3(0,-100,0);
    }

    public IEnumerator ExecuteAfterTime(float timeInSec)
    {
        yield return new WaitForSeconds(timeInSec);
        if (GameObject.Find("SubtitlesInfo").GetComponent<TMPro.TextMeshProUGUI>().text == "Picked Up <color=green>" + item.name + "</color>")
        {
            GameObject.Find("SubtitlesInfo").GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }
        Destroy(gameObject);
    }

    public override void OnLoseFocus()
    {
    }
    

}
