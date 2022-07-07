using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerDoorInteractable : Interactable
{

    public bool isOpen = false, dir = false;
    public float t, zAngle = 120, angel;
    public Transform defTransf;

    public override void OnFocus()
    {
    }

    public override void OnInteract()
    {
        if (!isOpen) StartCoroutine(Opening());
        else StartCoroutine(Closig());
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



        print("Buu");
        isOpen = true;
        t = 0;
        yield return null;

    }


    public override void OnLoseFocus()
    {
    }

    private void Update()
    {
        angel = transform.localEulerAngles.y;
    }

    void Start()
    {
        defTransf = transform;
    }
}
