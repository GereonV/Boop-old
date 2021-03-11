using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Health : MonoBehaviour{

    Movement movement;

    [SerializeField] float minPos = -10f;
    [SerializeField] float addedMultiplier = 0.2f;
    [SerializeField] float addedTime = 0.15f;

    private void Start() {
        movement = GetComponent<Movement>();
    }

    public void TakeHit() {
        movement.boopMultiplier += addedMultiplier;
        movement.boopedTime += addedTime;
    }

    private void FixedUpdate() {
        if(transform.position.y < minPos)
            Die();
    }

    void Die() {

    }
}
