using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraPosition : MonoBehaviour {

    Camera camera;

    [SerializeField] Transform player1;
    [SerializeField] Transform player2;
    [SerializeField] float smoothness = 0.05f;

    private void Start() {
        camera = GetComponent<Camera>();
    }

    private void FixedUpdate() {
        Vector3 newPos = (player1.position + player2.position) / 2;
        newPos.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, newPos, smoothness);

        camera.orthographicSize = Mathf.Max(Vector2.Distance(player1.position, player2.position), 7f);
    }
}
