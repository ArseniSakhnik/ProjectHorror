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
        ctr = GameObject.Find("Player").GetComponent<PlayerController>();
        ctr.inventory.AddItem(item, amount);
        Destroy(gameObject);
    }

    public override void OnLoseFocus()
    {
    }

}
