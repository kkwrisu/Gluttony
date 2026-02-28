using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    [Header("Stamina")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrain = 25f;
    [SerializeField] private float staminaRegen = 15f;
    [SerializeField] private float regenDelay = 1f;
    [SerializeField] private float runMultiplier = 1.8f;

    public bool canMove = true;

    private Rigidbody2D rb;
    private Animator anim;
    private PlayerInput playerInput;
    private Vector2 input;

    private Vector2 lastInput = Vector2.right;
    public Vector2 LastInput => lastInput;

    private float currentStamina;
    private float regenTimer;

    public float StaminaPercent => currentStamina / maxStamina;

    public bool IsHit
    {
        get
        {
            var state = anim.GetCurrentAnimatorStateInfo(0);
            return state.IsName("Hit") || state.IsName("Player_Hit");
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        currentStamina = maxStamina;
    }

    void Update()
    {
        if (!canMove || IsHit)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isWalking", false);
            return;
        }

        input = playerInput.actions["Move"].ReadValue<Vector2>();

        bool sprintPressed = playerInput.actions["Sprint"].IsPressed();
        bool isRunning = sprintPressed && currentStamina > 0 && input != Vector2.zero;

        float finalSpeed = moveSpeed;

        if (isRunning)
        {
            finalSpeed *= runMultiplier;
            currentStamina -= staminaDrain * Time.deltaTime;
            regenTimer = 0f;
        }
        else
        {
            regenTimer += Time.deltaTime;

            if (regenTimer >= regenDelay && currentStamina < maxStamina)
                currentStamina += staminaRegen * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        if (input != Vector2.zero)
        {
            anim.SetBool("isWalking", true);
            lastInput = input.normalized;

            anim.SetFloat("InputX", input.x);
            anim.SetFloat("InputY", input.y);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        rb.linearVelocity = input * finalSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            anim.SetFloat("LastInputX", lastInput.x);
            anim.SetFloat("LastInputY", lastInput.y);
        }
    }

    private void LateUpdate()
    {
#if UNITY_EDITOR
        if (Keyboard.current != null && Keyboard.current.f10Key.wasPressedThisFrame)
        {
            moveSpeed *= 3f;
        }
#endif
    }
}