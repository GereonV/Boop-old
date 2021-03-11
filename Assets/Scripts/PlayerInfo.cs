using UnityEngine;

public class PlayerInfo : MonoBehaviour {

    [HideInInspector] public bool update;
    [HideInInspector] public int jumpsLeft;
    [HideInInspector] public bool dodge;
    [HideInInspector] public float boopMultiplier;
    [HideInInspector] public float boopedTime;
    [HideInInspector] public bool dodging;
    [HideInInspector] public bool booped;

    private void Start() {
        ResetValues();
    }

    public void ResetValues() {
        update = true;
        jumpsLeft = Movement.jumps;
        dodge = true;
        dodging = false;
        booped = false;
        boopMultiplier = Movement.boopMultiplier;
        boopedTime = Movement.boopedTime;
    }
}
