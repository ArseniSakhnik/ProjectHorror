using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableLock : MonoBehaviour
{
    public GameObject doorToOpen;

    public void LockLogic()
    {

        if (doorToOpen.TryGetComponent(out LockerDoorInteractable locker))
        {
            locker.needKey = false;
        }
        else if (doorToOpen.TryGetComponent(out DoorInteractable door))
        {
            door.NeedKey = false;
        }
    }
}
