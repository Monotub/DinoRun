using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float gravity = -15f;
    [SerializeField] bool canDie = true;

    PlayerInputActions controls;
    CharacterController controller;
    Vector3 velocity;
    Animator anim;

    float baseY;
    float jumpY;
    float jumpValue;
    bool isDead = false;


    private void Awake()
    {
        controls = new PlayerInputActions();
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        jumpY = 3.9f;

        controls.Dino.Jump.performed += ctx => HandleJump(ctx);
        controls.Dino.Restart.performed += ctx => RestartScene();
        controls.Dino.Enable();
    }

    private void OnDisable()
    {
        controls.Dino.Jump.performed -= ctx => HandleJump(ctx);
        controls.Dino.Restart.performed -= ctx => RestartScene();
        controls.Dino.Disable();
    }

    private void Update()
    {
        baseY = transform.position.y - 1;   // Magic number to account for start y value of dino
        jumpValue = baseY / jumpY;
        anim.SetFloat("JumpHeight", jumpValue);


        if (!controller.isGrounded)
            ApplyGravity();

        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            Debug.Log("Done with death animation!");
        }
    }

    private void ApplyGravity()
    {

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleJump(InputAction.CallbackContext ctx)
    {
        bool jumpHeld = ctx.ReadValue<float>() > 0 ? true : false;

        if (controller.isGrounded && jumpHeld)
        {
            anim.SetTrigger("Jump");
            velocity.y = jumpHeight;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    void HandleDuck()
    {
        Debug.LogWarning("Duck not implemented");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canDie || isDead) return;

        if(other.tag == "Hazard")
        {
            FindObjectOfType<ChunkSpawner>().StopSpawning();
            ObjectPool.SharedInstance.SetAllChunkSpeed(0);
            anim.SetTrigger("Die");
            Debug.LogWarning("DOOM!");
            isDead = true;
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
}
