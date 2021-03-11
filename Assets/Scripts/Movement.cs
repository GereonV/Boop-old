using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerCollisions))]
public class Movement : MonoBehaviour {

    private InputActions1 inputs1;
    private InputActions2 inputs2;
    private Rigidbody2D rb;
    private bool update = true;
    private float movement;
    private int jumpsLeft;
    private bool dodge = true;

    [SerializeField] bool player1 = true;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 7f;
    [SerializeField] float dropVelocity = 20f;
    [SerializeField] int jumps = 3;
    [SerializeField] float dodgeDowntime = 2f;
    [SerializeField] float dodgeTime = 0.3f;
    [SerializeField] float dodgeRange = 4f;
    [SerializeField] bool dodgeResetsJump = true;
    [SerializeField] float fallMultiplier = 2.5f;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        inputs1 = new InputActions1();
        inputs2 = new InputActions2();
        if(player1) {
            inputs1.Game.Move.performed += input => movement = input.ReadValue<float>();
            inputs1.Game.Jump.performed += Jump;
            inputs1.Game.Drop.performed += Drop;
            inputs1.Game.Dodge.performed += Dodge;
        } else {
            inputs2.Game.Move.performed += input => movement = input.ReadValue<float>();
            inputs2.Game.Jump.performed += Jump;
            inputs2.Game.Drop.performed += Drop;
            inputs2.Game.Dodge.performed += Dodge;
        }
        ResetJumps();
    }

    private void FixedUpdate() {
        if(update) {
            Vector2 velocity = rb.velocity;
            if(velocity.y < 0)
                velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime * (fallMultiplier - 1);
            velocity.x = movement * moveSpeed;
            rb.velocity = velocity;
        }
    }

    private void Jump(CallbackContext obj) {
        if(jumpsLeft > 0 && !GetComponent<PlayerCollisions>().CollisionTop()) {
            Vector2 velocity = rb.velocity;
            velocity.y = jumpForce;
            rb.velocity = velocity;
            jumpsLeft--;
        }
    }

    private void Drop(CallbackContext obj) {
        rb.velocity += Vector2.down * dropVelocity;
    }

    public void Dodge(CallbackContext obj) {
        if(dodge && movement != 0f) {
            dodge = false;
            if(dodgeResetsJump)
                ResetJumps();

            StartCoroutine(StartDodge());
        }
    }

    public IEnumerator StartDodge() {
        update = false;
        rb.gravityScale = 0;
        rb.velocity = Vector2.right * (dodgeRange / dodgeTime) * movement;
        yield return new WaitForSeconds(dodgeTime);

        StartCoroutine(StopDodge());
    }
    public IEnumerator StopDodge() {
        update = true;
        rb.gravityScale = 1f;
        yield return new WaitForSeconds(dodgeDowntime);
        dodge = true;
    }

    public void ResetJumps() {
        jumpsLeft = jumps;
    }


    private void OnEnable() {
        inputs1.Enable();
        inputs2.Enable();
    }

    private void OnDisable() {
        inputs1.Disable();
        inputs2.Disable();
    }
}
