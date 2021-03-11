using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(PlayerInfo), typeof(PlayerCollisions), typeof(Health))]
public class Movement : MonoBehaviour {

    private PlayerInfo info;
    private InputActions1 inputs1;
    private InputActions2 inputs2;
    private Rigidbody2D rb;
    private float movement;

    [SerializeField] public bool player1 = true;
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] public static int jumps = 3;
    [SerializeField] float jumpForce = 7f;
    [SerializeField] float dropVelocity = 20f;
    [SerializeField] public static float boopMultiplier = 1.1f;
    [SerializeField] public static float boopedTime = 0.3f;
    [SerializeField] float dodgeDowntime = 1.2f;
    [SerializeField] float dodgeTime = 0.2f;
    [SerializeField] float dodgeRange = 4.5f;
    [SerializeField] bool dodgeResetsJump = true;
    [SerializeField] float fallMultiplier = 2.5f;

    void Awake() {
        info = GetComponent<PlayerInfo>();
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
        Vector2 velocity = rb.velocity;
        if(velocity.y < 0)
            velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime * (fallMultiplier - 1);

        if(info.update) {
            velocity.x = movement * moveSpeed;
        }

        rb.velocity = velocity;
    }

    private void Jump(CallbackContext obj) {
        if(info.jumpsLeft > 0 && !GetComponent<PlayerCollisions>().CollisionTop() && !info.booped) {
            Vector2 velocity = rb.velocity;
            velocity.y = jumpForce;
            rb.velocity = velocity;
            info.jumpsLeft--;
        }
    }

    private void Drop(CallbackContext obj) {
        if(!info.booped)
            rb.velocity += Vector2.down * dropVelocity;
    }

    public void Boop(GameObject otherPlayer) {
        StopCoroutine(StartDodge());
        otherPlayer.GetComponent<Movement>().StartBooped((Vector2.right * rb.velocity.x).normalized);
        StartCoroutine(StopDodge());
    }

    public void StartBooped(Vector2 direction) {
        info.update = false;
        info.booped = true;
        rb.velocity = direction * (dodgeRange / dodgeTime) * GetComponent<PlayerInfo>().boopMultiplier;
        GetComponent<Health>().TakeHit();

        StartCoroutine(StopBooped());
    }

    public IEnumerator StopBooped() {
        yield return new WaitForSeconds(GetComponent<PlayerInfo>().boopedTime);
        info.update = true;
        info.booped = false;
    }

    public void Dodge(CallbackContext obj) {
        if(info.dodge && movement != 0f && !info.booped) {
            info.dodge = false;
            if(dodgeResetsJump)
                ResetJumps();

            StartCoroutine(StartDodge());
        }
    }

    public IEnumerator StartDodge() {
        info.update = false;
        rb.gravityScale = 0;
        info.dodging = true;
        rb.velocity = Vector2.right * (dodgeRange / dodgeTime) * movement;
        yield return new WaitForSeconds(dodgeTime);

        StartCoroutine(StopDodge());
    }
    public IEnumerator StopDodge() {
        info.update = true;
        rb.gravityScale = 1f;
        info.dodging = false;
        yield return new WaitForSeconds(dodgeDowntime);
        info.dodge = true;
    }

    public void ResetJumps() {
        info.jumpsLeft = jumps;
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
