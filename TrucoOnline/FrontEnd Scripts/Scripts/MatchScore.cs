using TMPro;
using UnityEngine;

public class MatchScore : MonoBehaviour
{
    private TMP_Text text;
    public int team1;
    public int team2;

    public static MatchScore Instance;

    private void Awake() {
        Instance = this;
        text = GetComponent<TMP_Text>();
    }

    private void Start() {
        team1 = 0;
        team2 = 0;
    }

    void Update()
    {
        text.text = "Time 1: " + team1 + "\n" + "Time 2: " + team2;
    }

}
