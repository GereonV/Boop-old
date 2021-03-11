using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Movement))]
public class PlayerCollisions : MonoBehaviour {

    private Movement movement;
    private Collider2D collider;
    private float extra = 0.01f;

    private void Start() {
        movement = GetComponent<Movement>();
        collider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(GetComponent<PlayerInfo>().dodging && collision.collider.tag.Equals("Player")) {
            movement.Boop(collision.collider.gameObject);
        } else {
            if(Physics2D.BoxCast(collider.bounds.center + Vector3.down * (collider.bounds.extents.y + 2 * extra), new Vector2(collider.bounds.size.x, extra), 0f, Vector2.zero, 0f))
                movement.ResetJumps();
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        OnCollisionEnter2D(collision);
    }

    public bool CollisionTop() {
        return Physics2D.BoxCast(collider.bounds.center + Vector3.up * (collider.bounds.extents.y + 2 * extra), new Vector2(collider.bounds.size.x, extra), 0f, Vector2.zero, 0f);
    }
}
