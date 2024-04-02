using System.Collections;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour {
    public TextMeshProUGUI timerText;
    public float startTime; 
    public GameObject clockObject; 
    public GameOver gameOver;
    private float timeRemaining;
    private bool timerIsActive = false;
    private bool isShaking = false;
    private NodeController nodeController;

    void Start() {
        nodeController = FindObjectOfType<NodeController>();
        gameOver = FindObjectOfType<GameOver>();
        ResetTimer();
    }

    void Update() {
        if (timerIsActive) {
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();

                if (timeRemaining <= 5f && timeRemaining > 0) {
                    if (!isShaking) {
                        isShaking = true;
                        StartCoroutine(ShakeGameObject(clockObject, 5f, 0.1f));
                    }
                    timerText.color = Color.red;
                }
            } else {
                timerIsActive = false;
                timeRemaining = 0;
                gameOver.GameOverControl();
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

    private void UpdateTimerDisplay() {
        timerText.text = $"{timeRemaining:0} seconds";
    }

    IEnumerator ShakeGameObject(GameObject obj, float duration, float magnitude) {
        Debug.Log("ShakeGameObject started");

        Vector3 originalPosition = obj.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration) {
            float x = originalPosition.x + Random.Range(-1f, 1f) * magnitude;
            float y = originalPosition.y + Random.Range(-1f, 1f) * magnitude;

            obj.transform.localPosition = new Vector3(x, y, originalPosition.z);
            elapsed += Time.deltaTime;

            yield return null;
        }

        obj.transform.localPosition = originalPosition;
        Debug.Log("ShakeGameObject ended");
    }
}
