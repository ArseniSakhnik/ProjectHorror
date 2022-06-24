using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [SerializeField] public List<Item> inventoryItems = new();

    // Start is called before the first frame update
    void Start() 
    {
        foreach (var item in inventoryItems)
        {
            item.Quantity = 1;
        }
    }

    public void AddItem(Item item)
    {
        if (inventoryItems.Contains(item))
        {
            inventoryItems[inventoryItems.IndexOf(item)].Quantity += 1;
            Debug.Log("Quantity++");
        }
        else
        {
            inventoryItems.Add(item);
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
