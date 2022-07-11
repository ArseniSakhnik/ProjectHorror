using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : Interactable
{
    [SerializeField] private bool IsOpen = false;
    [SerializeField] public bool NeedKey = false;
    [SerializeField] private float dot;
    [SerializeField] private PlayerController ctr;
    [SerializeField] private Item key;
    private Animator anim;
    AudioSource source;

    private void Start()
    {
       anim = GetComponent<Animator>();
       source = gameObject.AddComponent<AudioSource>();
    }

    public override void OnFocus()
    {
    }

    public override void OnInteract() 
    {
        ctr = GameObject.Find("Player").GetComponent<PlayerController>();

        if (!NeedKey)
        {
            IsOpen = !IsOpen;
            Vector3 doorTransformDirection = transform.TransformDirection(Vector3.left);
            Vector3 playerTransformDirection = ctr.transform.position - transform.position;
            dot = Vector3.Dot(doorTransformDirection, playerTransformDirection);
           
            anim.SetFloat("dot", dot);
            anim.SetBool("isOpen", IsOpen);
            FindObjectOfType<AudioManager>().Play("Door Open", source);
        }

        else if (ctr.inventory.CheckItem(key) && NeedKey)
        {
            FindObjectOfType<AudioManager>().Play("Use Key", source);
            ctr.inventory.RemoveItem(key);
            NeedKey = false;
        }

        else
        {
            Debug.Log("Door is loocked");
            FindObjectOfType<AudioManager>().Play("Need Key", source);
        }

    }


    public override void OnLoseFocus()
    {
    }

}
