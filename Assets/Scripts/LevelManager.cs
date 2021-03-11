using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;

    [SerializeField] Transform player1;
    [SerializeField] Transform player2;
    [SerializeField] Text text1;
    [SerializeField] Text text2;

    Vector3 startPos1;
    Vector3 startPos2;

    public int score1 = 0;
    public int score2 = 0;

    private void Start() {
        instance = this;
        startPos1 = player1.position;
        startPos2 = player2.position;
    }

    public void Restart() {
        player1.position = startPos1;
        player1.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player1.gameObject.GetComponent<PlayerInfo>().ResetValues();
        player2.position = startPos2;
        player2.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player2.gameObject.GetComponent<PlayerInfo>().ResetValues();

        text1.text = score1.ToString();
        text2.text = score2.ToString();
    }
}
