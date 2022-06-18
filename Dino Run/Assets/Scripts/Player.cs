using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float gravity = -15f;
    [SerializeField] bool canDie = true;

    public bool IsDead => isDead;

    PlayerInputActions controls;
    CharacterController controller;
    InputAction jumpKey;
    Vector3 velocity;
    Animator anim;

    float baseY;
    float jumpY = 3.9f;
    float jumpValue;
    bool isDead = false;



    private void Awake()
    {
        controls = new PlayerInputActions();
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        jumpKey = controls.FindAction("Jump");
    }

    private void OnEnable()
    {
        controls.Dino.Jump.performed += ctx => HandleJump(ctx);
        controls.Dino.Restart.performed += ctx => GameManager.Instance.ResetHighscore();
        controls.Dino.Enable();
    }

    private void OnDisable()
    {
        controls.Dino.Jump.performed -= ctx => HandleJump(ctx);
        controls.Dino.Restart.performed -= ctx => GameManager.Instance.ResetHighscore();
        controls.Dino.Disable();
    }

    private void Update()
    {
        if (GameManager.Instance.gamePaused)
        {
            anim.SetTrigger("IdleExtras");   
        }
        else
        {
            anim.SetTrigger("StartRunning");
            ProcessJumpAnimation();

            if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                Debug.Log("Done with death animation!");
            }
        }

        if (!controller.isGrounded)
            ApplyGravity();
    }


    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleJump(InputAction.CallbackContext ctx)
    {
        if (isDead) return;
        if (GameManager.Instance.gamePaused) GameManager.Instance.StartGame();

        if (controller.isGrounded)
        {
            anim.SetTrigger("Jump");
            velocity.y = jumpHeight;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    private void ProcessJumpAnimation()
    {
        // Magic number to account for start y value of dino
        baseY = transform.position.y - 1;   
        jumpValue = baseY / jumpY;
        anim.SetFloat("JumpHeight", jumpValue);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canDie || isDead) return;

        if(other.tag == "Hazard")
            HandleDeath();
    }

    private void HandleDeath()
    {
        isDead = true;
        anim.SetTrigger("Die");

        GameManager.Instance.PauseGame();
        FindObjectOfType<ChunkSpawner>().StopSpawning();
        ObjectPool.SharedInstance.SetAllChunkSpeed(0);
        UIManager.Instance.ShowRestartUI();
    }



    /// <summary>
    /// Used in running animation event
    /// </summary>
    private void PlayFootSound()
    {
        //Debug.Log("Stomp");
        // TODO: Send event to audio manager for foot sounds
        // added to test to see if this worked for downloaded model/animations
    }
}
