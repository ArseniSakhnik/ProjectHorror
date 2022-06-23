using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [SerializeField] List<Item> inventoryItems = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(Item item)
    {
        inventoryItems.Add(item);
    }

    public bool CheckItem(Item item)
    {
        return inventoryItems.Contains(item);
    }

}
