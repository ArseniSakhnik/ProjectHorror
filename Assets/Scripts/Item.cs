using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData")]
public class Item : ScriptableObject
{
    public string Name = "Item";
    public string Description = "Description";
    public int Quantity = 1;
    public enum ItemType 
    {
        Consumable,
        Note,
        KeyItem

    }
    public Sprite Icon;

}
