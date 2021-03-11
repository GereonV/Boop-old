using UnityEngine.SceneManagement;

public class LevelManager {

    public static void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
