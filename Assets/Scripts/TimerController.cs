using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour {
    public TextMeshProUGUI timerText;
    public float startTime; 
    public GameObject clockObject; 
    private GameOverTime gameOverTime;
    private NodeController nodeController;
    private float timeRemaining;
    private bool timerIsActive = false;

    void Start() {
        gameOverTime = FindObjectOfType<GameOverTime>();
        nodeController = FindObjectOfType<NodeController>();
        ResetTimer();
    }

    void Update() {
        if (timerIsActive && !nodeController.isGamePaused) {
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();

                if (timeRemaining <= 5f && timeRemaining > 0) {
                    timerText.color = Color.red;
                }
            } else {
                timerIsActive = false;
                timeRemaining = 0;
                gameOverTime.GameOverControl();
                UpdateTimerDisplay();
            }
        }
    }

    public void ResetTimer() {
        timeRemaining = startTime;
        timerIsActive = true;
        timerText.color = Color.white; 
        UpdateTimerDisplay();
    }

    public void AddTime(int additionalTime) {
        timeRemaining += additionalTime;
        if (!timerIsActive) {
            timerIsActive = true;
        }
        if (timeRemaining > 5) {
            timerText.color = Color.white;
        }

        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay() {
        timerText.text = $"{timeRemaining:0} seconds";
    }
}
