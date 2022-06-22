using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : Interactable
{
    [SerializeField] private bool IsOpen = false;
    [SerializeField] private bool NeedKey = false;
    [SerializeField] private float dot;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void OnFocus()
    {
    }

    public override void OnInteract()
    {
        if (!NeedKey)
        {
            IsOpen = !IsOpen;
            Vector3 doorTransformDirection = transform.TransformDirection(Vector3.left);
            Vector3 playerTransformDirection = PlayerController.instance.transform.position - transform.position;
            dot = Vector3.Dot(doorTransformDirection, playerTransformDirection);
           


            anim.SetFloat("dot", dot);
            anim.SetBool("isOpen", IsOpen);

        }
    }

    public override void OnLoseFocus()
    {
    }

}
