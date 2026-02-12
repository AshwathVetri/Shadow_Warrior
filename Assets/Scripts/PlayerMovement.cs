using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private CharacterController controller;
    private Animator animator;

    // 🔥 Combo System
    private int comboStep = 0;
    private float comboTimer = 0f;
    public float comboResetTime = 1f; // time before combo resets
    private bool canClick = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
        ComboAttack();
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        animator.SetBool("isWalking", move.magnitude > 0.1f);
    }

    void ComboAttack()
    {
        // reset combo timer
        if (comboStep > 0)
        {
            comboTimer += Time.deltaTime;

            if (comboTimer > comboResetTime)
            {
                comboStep = 0;
            }
        }

        if (Input.GetMouseButtonDown(0) && canClick)
        {
            comboTimer = 0f;
            comboStep++;

            if (comboStep == 1)
            {
                animator.SetTrigger("Attack1");
            }
            else if (comboStep == 2)
            {
                animator.SetTrigger("Attack2");
            }
            else if (comboStep == 3)
            {
                animator.SetTrigger("Attack3");
                comboStep = 0; // reset after final combo
            }
        }
    }
}
