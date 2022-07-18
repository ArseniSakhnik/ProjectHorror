using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int layr;
    [Header("���������")]
    [SerializeField] public static bool isblockShooting = false;
    [SerializeField] public static bool isblockInventory = false;
    [SerializeField] public static bool isblockInteraction = false;
    [SerializeField] public static bool isblockReading = false;
    public bool isDead = false;

    [SerializeField] public GameObject hand;

    [Header("���������")]
    [SerializeField] public int playerHealth = 100; // health will not exceed 100

    [SerializeField] public Inventory inventory; // field for inventory

    [SerializeField] private Transform playerCamera; // camera object
    [SerializeField] private Light flashlight; // camera object
    [SerializeField] private float mouseSensitivity = 3.5f; // sensetivity

    [SerializeField] private float walkSpeed = 6.0f; // speed
    [SerializeField] private float gravity = -13.0f; // gravity for fall
    [SerializeField][Range(0.0f, 0.5f)] private float moveSmoothTime = 0.3f; // smooth moving vector
    [SerializeField][Range(0.0f, 0.5f)] private float mouseSmoothTime = 0.03f; // smooth sense vector
    [SerializeField] private float sprintAccel = 2.0f;
    [SerializeField] private float sprintDuration = 10.0f;

    [SerializeField] private bool lockCursor = true;

    [SerializeField] private bool isMoving;
    [SerializeField] private bool isSprinted;
    [SerializeField] private bool isSprintOnCooldown;


    [Header("��������� �������")]
    [SerializeField] private float crouchHeight = 1f;

    [SerializeField] private float standingHeight = 2.0f;
    [SerializeField] private float timeToCrouch = 0.3f;
    [SerializeField] private Vector3 crouchingCenter = new(0, 1f, 0);
    [SerializeField] private Vector3 standingCenter = new(0, 0, 0);
    public bool crouchmode;


    [Header("��������� �����������")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float bobcooldown = 0;
    [SerializeField] private float walkBobAmount = 0.02f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.04f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.025f;

    [Header("��������� �����")]
    [SerializeField] private bool risingLight = false;

    [Header("��������� ����� �����")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultipler = 1.5f;
    [SerializeField] private float sprintStepMultipler = 0.7f;
    [SerializeField] private float footstepTimer = 0f;
    [SerializeField] private float GetCurrentOfstet => crouchmode ? baseStepSpeed * crouchStepMultipler : isSprinted ? baseStepSpeed * sprintStepMultipler : baseStepSpeed;

    private float cameraPitch;

    private CharacterController controller;

    private Vector2 currentDir = Vector2.zero;
    private Vector2 currentDirVelocity = Vector2.zero;

    private Vector2 currentMouseDelta = Vector2.zero;
    private Vector2 currentMouseDeltaVelocity = Vector2.zero;
    private float defaultXPos;
    private float defaultYPos;
    private bool duringCrouchAnimation;
    private float timer;
    private float velocityY;
    public float transparency;
    
    IEnumerator DeathHandler()
    {
        yield return new WaitForSeconds(2);
        GameObject.Find("SubtitlesInfo").GetComponent<TMPro.TextMeshProUGUI>().text = "Press E to Restart";

        while (!Input.GetKeyDown(KeyCode.E))
        {
            yield return null;
        }

        isblockInteraction = false;
        isblockInventory = false;
        isblockReading = false;
        isblockShooting = false;
        SceneManager.LoadScene(0);
    }

    public void GetDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        playerHealth = playerHealth - damage;
        if (playerHealth <= 0)
        {
            FindObjectOfType<AudioManager>().Play("PlayerDeath");
            isblockInteraction = true;
            isblockInventory = true;
            isblockReading = true;
            isblockShooting = true;
            isDead = true;
            StartCoroutine(DeathHandler());
        }
        FindObjectOfType<AudioManager>().Play("PlayerDamage");

    }


    public bool IsCrouching =>
        Input.GetKey(KeyCode.LeftControl) && !duringCrouchAnimation &&
        controller.isGrounded; // ������ ������ ��� ������� ������� ������� � ��������� ������


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        defaultYPos = playerCamera.transform.localPosition.y;
        defaultXPos = playerCamera.transform.localPosition.x;
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    private void Update()
    {
        if (isDead)
        {
            transparency += Time.deltaTime;
            GameObject.Find("BlackScreen").GetComponent<RawImage>().color = new Color(0, 0, 0, transparency);
            return;
        }

        GameObject.Find("RedScreen").GetComponent<RawImage>().color = new Color(255, 0, 0, 1 - (float)playerHealth/90);


        if (Time.timeScale !=0f)
        {
            UpdateMouseLook();
        }
        HandleCrouch();
        Headbobhandler();
        UpdateMovement();
        HandleFootsteps();
        FlashlightHandler();
    }


    private void HandleFootsteps()
    {
        if (!controller.isGrounded)
        {
            return;
        }

        if (!isMoving)
        {
            return;
        }
        footstepTimer -= Time.deltaTime;

        if (footstepTimer<=0)
        {
            footstepTimer = GetCurrentOfstet;

            layr = LayerMask.GetMask("Player");

            if (Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3, ~layr))
            {
                Debug.Log("Get Rekt " + hit.collider.name);
                switch (hit.collider.tag)
                {
                    case "SnowMat":
                        FindObjectOfType<AudioManager>().PlayWalkSound("FootSnow");
                        break;
                    default:
                        FindObjectOfType<AudioManager>().PlayWalkSound("WoodFootStep");
                        break;
                }
            }
        }
    }



    private void FlashlightHandler()
    {
        if (risingLight == true)
        {
            flashlight.intensity += Time.deltaTime * 10;
        }

        if (Input.GetKeyDown(KeyCode.F) && flashlight.intensity >= 3)
        {
            flashlight.intensity = 0;
            risingLight = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.F) && flashlight.intensity <= 3)
        {
            if (flashlight.intensity < 3)
            {
                risingLight = true;
            }
        }
        if (flashlight.intensity >= 3)
        {
            risingLight = false;
        }
    }

    private void Headbobhandler()
    {
        Vector3 target;


        if (!isMoving && bobcooldown > 300)
        {
            timer += Time.deltaTime * walkBobSpeed;
            target = new Vector3(
            defaultXPos + Mathf.Sin(timer / 8) * walkBobAmount,
            defaultYPos + Mathf.Sin(timer / 4) * walkBobAmount,
            playerCamera.transform.localPosition.z);
            playerCamera.transform.localPosition = target;
            hand.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, playerCamera.transform.localPosition.y - 1, playerCamera.transform.localPosition.z);

        }


        else if (isMoving)
        {
            timer += Time.deltaTime * (IsCrouching ? crouchBobSpeed : isSprinted ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                defaultXPos + Mathf.Sin(timer / 2) * (IsCrouching ? crouchBobAmount * 6 :
                    isSprinted ? sprintBobAmount * 8 : walkBobAmount * 6),
                defaultYPos + Mathf.Sin(timer) * (IsCrouching ? crouchBobAmount * 3 :
                    isSprinted ? sprintBobAmount * 4 : walkBobAmount * 3),
                playerCamera.transform.localPosition.z);
            bobcooldown = 0;
            hand.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, playerCamera.transform.localPosition.y - 1, playerCamera.transform.localPosition.z);
        }


        else
        {
            if (playerCamera.transform.localPosition == new Vector3 (defaultXPos,defaultYPos, 0))
            {
                bobcooldown++;
                timer = 0;
                return;
            }
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, new Vector3(defaultXPos, defaultYPos, 0), walkBobSpeed * Time.deltaTime);
            hand.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, playerCamera.transform.localPosition.y - 1, playerCamera.transform.localPosition.z);
        };
    }


    private void HandleCrouch() // ���������� ��������
    {
        if (IsCrouching) StartCoroutine(CrouchStand());
    }

    private IEnumerator CrouchStand() // ������ ����������
    {
        if (crouchmode && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1.5f))
            yield break;


        duringCrouchAnimation = true;

        float timeElapsed = 0;
        var targetHeight = crouchmode ? standingHeight : crouchHeight;
        var currentHeight = controller.height;
        var targetCenter = crouchmode ? standingCenter : crouchingCenter;
        var currentCenter = controller.center;

        while (timeElapsed < timeToCrouch)
        {
            controller.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            controller.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            if (crouchmode)
            {
                controller.transform.position = new Vector3(controller.transform.position.x, controller.transform.position.y + 0.1f, controller.transform.position.z);
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        controller.height = targetHeight;
        controller.center = targetCenter;

        crouchmode = !crouchmode;

        duringCrouchAnimation = false;
    }

    private void UpdateMouseLook()
    {
        var targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity,
            mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }


    private void UpdateMovement()
    {
        var targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        if (controller.velocity.sqrMagnitude > 0.1f) // �������� �� ��������
            isMoving = true;
        else isMoving = false;


        if (controller.isGrounded) // �������� �� �������
            velocityY = 0.0f;

        velocityY += gravity * Time.deltaTime;


        var velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed +
                       Vector3.up * velocityY;

        if (Input.GetKey(KeyCode.LeftShift) && !crouchmode && !IsCrouching && controller.isGrounded &&
            !isSprintOnCooldown && sprintDuration > 0 &&
            (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)) // if sprinting
        {
            isSprinted = true;
            sprintDuration -= 0.5f * Time.timeScale / 100;
            if (sprintDuration < 0) isSprintOnCooldown = true;
        }

        else // if not sprinting
        {
            isSprinted = false;
            if (sprintDuration < 10.0f)
            {
                sprintDuration += 0.1f * Time.timeScale / 100;
                if (sprintDuration > 5) isSprintOnCooldown = false;
            }
        }

        if (crouchmode) velocity *= 0.25f;
        if (isSprinted) velocity *= sprintAccel;
        controller.Move(velocity * Time.deltaTime);
    }
}