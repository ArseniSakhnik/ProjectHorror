using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidvigInteractable : Interactable
{
    public float defZ, endZ;
    public bool isOpen, inProcess;
    float t = 0f;
    AudioSource source;
    public override void OnFocus()
    {
    }

    public override void OnInteract()
    {
        if (inProcess)
        {
            return;
        }

        if (!isOpen)
            StartCoroutine(Opening());
        else StartCoroutine(Closig());
        FindObjectOfType<AudioManager>().Play("Yashik", source);

    }

    public override void OnLoseFocus()
    {
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
        defZ = transform.localPosition.z;
        endZ = defZ + 0.8f;
    }


    IEnumerator Closig()
    {
        inProcess = true;
        while (transform.localPosition.z > defZ)
        {
            t += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, defZ), t);
            yield return null;
        }
        isOpen = false;
        t = 0;
        inProcess = false;
    }

    IEnumerator Opening()
    {
        inProcess = true;
        while (transform.localPosition.z < endZ)
        {
            t += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, endZ), t);
            yield return null;
        }
        isOpen = true;
        t = 0;
        inProcess = false;
    }
}
