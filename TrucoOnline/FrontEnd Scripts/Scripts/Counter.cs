using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    private TMP_Text text;
    public int team1;
    public int team2;
    public static Counter Instance;

    private void Awake() {
        Instance = this;
        text = GetComponent<TMP_Text>();
    }

    private void Start() {
        team1 = 0;
        team2 = 0;
    }

    public void RestartScore(){
        team1 = 0;
        team2 = 0;
    }

    void Update()
    {
        text.text = "T1 " + team1 + " x " + team2 + " T2";
    }

}
