using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerDoorInteractable : Interactable
{

    public bool isOpen = false, dir = false;
    public float t, zAngle = 120, angel;
    public Transform defTransf;
    public Item keyItem;
    public bool needKey;
    private PlayerController ctr;

    public override void OnFocus()
    {
    }

    public override void OnInteract()
    {
        ctr = GameObject.Find("Player").GetComponent<PlayerController>();


        if (needKey == false)
        {
            if (!isOpen) StartCoroutine(Opening());
            else StartCoroutine(Closig());
        }

        else if (ctr.inventory.CheckItem(keyItem) && needKey)
        {
            Debug.Log("Door Unlocked");
            ctr.inventory.RemoveItem(keyItem);
            needKey = false;
        }

        else
        {
            Debug.Log("Door is loocked");
        }


    }

    IEnumerator Closig()
    {
        if (!dir)
        {
            while (transform.localEulerAngles.y > 1)
            {
                t += Time.deltaTime;
                transform.localRotation = Quaternion.Slerp(defTransf.localRotation, Quaternion.Euler(transform.localRotation.eulerAngles.x, 0, 0), t * Time.deltaTime * 2);
                yield return null;
            }
        }

        else
        {
            while (transform.localEulerAngles.y > 0)
            {
                t += Time.deltaTime;
                transform.localRotation = Quaternion.Slerp(defTransf.localRotation, Quaternion.Euler(transform.localRotation.eulerAngles.x, 0, 360), t * Time.deltaTime * 2);
                yield return null;
            }
        }


        isOpen = false;
        t = 0;
    }

    IEnumerator Opening()
    {
        if (!dir)
        {
            while (transform.localEulerAngles.y < zAngle - 1)
            {
                t += Time.deltaTime;
                transform.localRotation = Quaternion.Slerp(defTransf.localRotation, Quaternion.Euler(transform.localRotation.eulerAngles.x, 0, zAngle), t * Time.deltaTime * 2);
                yield return null;
            }
        }
        else
        {
            while (transform.localEulerAngles.y - 180 >=  zAngle || transform.localEulerAngles.y <= 0)
            {
                t += Time.deltaTime;
                transform.localRotation = Quaternion.Slerp(defTransf.localRotation, Quaternion.Euler(transform.localRotation.eulerAngles.x, 0, -zAngle), t * Time.deltaTime * 2);
                yield return null;
            }
        }
        isOpen = true;
        t = 0;
        yield return null;

    }


    public override void OnLoseFocus()
    {
    }

    void Start()
    {
        defTransf = transform;
    }
}
