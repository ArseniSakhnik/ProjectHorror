using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Consumable,
    Note,
    KeyItem,
    Ammo,
    Weapon

}
[CreateAssetMenu(fileName = "ItemData")]


public class Item : ScriptableObject
{
    public string Name = "Item";
    public string Description = "Description";
    public int Quantity = 1;
    public ItemType Type;

    public Sprite Icon;

}
