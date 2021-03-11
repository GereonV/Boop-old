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
        RaycastHit2D hit = Physics2D.BoxCast(collider.bounds.center + Vector3.down * (collider.bounds.extents.y + 2 * extra), new Vector2(collider.bounds.size.x, extra), 0f, Vector2.zero, 0f);
        if(hit)
            movement.ResetJumps();
    }

    public bool CollisionTop() {
        return Physics2D.BoxCast(collider.bounds.center + Vector3.up * (collider.bounds.extents.y + 2 * extra), new Vector2(collider.bounds.size.x, extra), 0f, Vector2.zero, 0f);
    }
}
