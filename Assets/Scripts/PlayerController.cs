using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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


    [Header("Параметры приседа")]
    [SerializeField] private float crouchHeight = 1f;

    [SerializeField] private float standingHeight = 2.0f;
    [SerializeField] private float timeToCrouch = 0.3f;
    [SerializeField] private Vector3 crouchingCenter = new(0, 1f, 0);
    [SerializeField] private Vector3 standingCenter = new(0, 0, 0);
    public bool crouchmode;


    [Header("Параметры покачивания")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float bobcooldown = 0;
    [SerializeField] private float walkBobAmount = 0.02f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.04f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.025f;

    [Header("Параметры света")]
    [SerializeField] private bool risingLight = false;


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

    public bool IsCrouching =>
        Input.GetKey(KeyCode.LeftControl) && !duringCrouchAnimation &&
        controller.isGrounded; // Работа прыжка при условии нажатия клавиши и активации прыжка


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
        UpdateMouseLook();
        HandleCrouch();
        Headbobhandler();
        UpdateMovement();
        FlashlightHandler();

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
        };
    }


    private void HandleCrouch() // Управление приседом
    {
        if (IsCrouching) StartCoroutine(CrouchStand());
    }

    private IEnumerator CrouchStand() // Логика приседаний
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

        if (controller.velocity.sqrMagnitude > 0.1f) // проверка на движение
            isMoving = true;
        else isMoving = false;


        if (controller.isGrounded) // проверка на стояние
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