using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicInteractbleObject : Interactable
{
    
    public override void OnFocus()
    {
        print(gameObject.name + "in focus");
    }

    public override void OnInteract()
    {
        print(gameObject.name + "is interacted");
    }

    public override void OnLoseFocus()
    {
        print(gameObject.name + "lose focus");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
