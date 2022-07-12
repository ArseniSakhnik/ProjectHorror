using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{

    private PlayerController ctr;
    public Item Ammo;

    [Header("References")]
    [SerializeField] private Transform cam;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    [Tooltip("In RPM")] public float fireRate;
    public float reloadTime;
    public bool reloading;
    [Header("Данные о местоположении")]
    public float defaultX;
    public float defaultY;
    public float defaultZ;
    public float targetX;
    public float targetY;
    public float targetZ;

    [Header("Состояния")]
    public bool onRecoil;
    public bool onZoom;

    public float timer;

    public bool ondelay = false;

    float timeSinceLastShot;

    private void Start()
    {
        ctr = GameObject.Find("Player").GetComponent<PlayerController>();



        defaultX = transform.localPosition.x;
        defaultY = transform.localPosition.y;
        defaultZ = transform.localPosition.z;
        


        GunInput.shootInput += Shoot;
        GunInput.reloadInput += StartReload;
    }

    private void OnDisable() => reloading = false;

    public void StartReload()
    {
        if (!reloading && this.gameObject.activeSelf && Ammo.Quantity!=0)
            StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        reloading = true;

        FindObjectOfType<AudioManager>().Play(gameObject.name + "Reload");

        yield return new WaitForSeconds(reloadTime);


        while (currentAmmo != magSize && Ammo.Quantity!=0)
        {
            currentAmmo++;
            ctr.inventory.RemoveItem(Ammo);
        }
        reloading = false;
    }

    private bool CanShoot() => !reloading && timeSinceLastShot > 1f / (fireRate / 60f);

    private void Shoot()
    {
        if (currentAmmo > 0)
        {
            if (CanShoot())
            {
                if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, maxDistance))
                {
                    Debug.Log("Попал");
                    if (hitInfo.collider.tag == "DoorLock")
                    {
                        Debug.Log("Попал в замок");
                        Debug.Log("Замок открылся");
                        hitInfo.rigidbody.useGravity = true;
                        hitInfo.rigidbody.isKinematic = false;
                        hitInfo.collider.GetComponent<DestroyableLock>().LockLogic();
                        StartCoroutine(LockDestroy(hitInfo));

                    }
                }
                currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
        }
        else {
            if (!ondelay)
            {
                FindObjectOfType<AudioManager>().Play(gameObject.name + "Empty");
                ondelay = true;
                StartCoroutine(WaitDelay());
            }
        }
    }

    IEnumerator WaitDelay()
    {
        yield return new WaitForSeconds(1);
        ondelay = false;
    }


    public IEnumerator LockDestroy(RaycastHit hitInfo)
    {
        yield return new WaitForSeconds(3);
        Destroy(hitInfo.collider.gameObject);
    }


    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        Debug.DrawRay(cam.position, cam.forward * maxDistance);
    }

    private void OnGunShot()
    {
        Recoil();
        FindObjectOfType<AudioManager>().Play(gameObject.name + "Shoot", null, true);
    }

    private void Recoil()
    {
        transform.localRotation = Quaternion.Euler(transform.rotation.x - 20, transform.rotation.y+10, transform.rotation.z);
    }

}
