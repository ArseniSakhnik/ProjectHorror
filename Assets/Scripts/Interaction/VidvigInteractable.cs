using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidvigInteractable : Interactable
{
    public float defZ, endZ;
    public bool isOpen;
    float t = 0f;
    public override void OnFocus()
    {
    }

    public override void OnInteract()
    {
        if (!isOpen)
            StartCoroutine(Opening());
        else StartCoroutine(Closig());
    }

    public override void OnLoseFocus()
    {
    }

    void Start()
    {
        defZ = transform.localPosition.z;
        endZ = defZ + 0.8f;
    }


    IEnumerator Closig()
    {
        while (transform.localPosition.z > defZ)
        {
            t += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, defZ), t);
            yield return null;
        }
        isOpen = false;
        t = 0;
    }

    IEnumerator Opening()
    {
        while (transform.localPosition.z < endZ)
        {
            t += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, endZ), t);
            yield return null;
        }
        isOpen = true;
        t = 0;
    }
}
