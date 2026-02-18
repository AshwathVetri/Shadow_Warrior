using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float crouchSpeed = 2.5f;

    [Header("Jump Settings")]
    public float jumpHeight = 1.6f;
    public float gravity = -30f;
    public float fallMultiplier = 2.2f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    private float originalHeight;

    private float verticalVelocity;

    private CharacterController controller;
    private Animator animator;

    private bool isCrouching;

    // 🔥 Combo System
    private int comboStep = 0;
    private float comboTimer = 0f;
    public float comboResetTime = 1f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        originalHeight = controller.height;
    }

    void Update()
    {
        ApplyGravity();
        Movement();
        Jump();
        Crouch();
        ComboAttack();
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // stick to ground
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;

            if (verticalVelocity < 0)
                verticalVelocity += gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float currentSpeed = isCrouching ? crouchSpeed : moveSpeed;

        Vector3 move = transform.right * x + transform.forward * z;

        Vector3 finalMove = move * currentSpeed;
        finalMove.y = verticalVelocity;

        controller.Move(finalMove * Time.deltaTime);

        animator.SetBool("isWalking", move.magnitude > 0.1f);
        animator.SetBool("isCrouching", isCrouching);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && controller.isGrounded && !isCrouching)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump");
        }
    }
    

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;
            controller.height = crouchHeight;
            controller.center = new Vector3(0, crouchHeight / 2f, 0);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
            controller.height = originalHeight;
            controller.center = new Vector3(0, originalHeight / 2f, 0);
        }
    }

    void ComboAttack()
    {
        if (comboStep > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer > comboResetTime)
                comboStep = 0;
        }

        if (Input.GetMouseButtonDown(0))
        {
            comboTimer = 0f;
            comboStep++;

            if (comboStep == 1)
                animator.SetTrigger("Attack1");
            else if (comboStep == 2)
                animator.SetTrigger("Attack2");
            else if (comboStep == 3)
            {
                animator.SetTrigger("Attack3");
                comboStep = 0;
            }
        }
    }
}
