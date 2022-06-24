using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWindow : MonoBehaviour
{
    [SerializeField] PlayerController ctr;
    [SerializeField] Inventory currentItems;

    // Start is called before the first frame update
    void Awake()
    {
        ctr = GameObject.Find("Player").GetComponent<PlayerController>();
        currentItems = ctr.inventory;
    }


    void Redraw()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
