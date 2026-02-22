using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [Header("Match Settings")]
    [Tooltip("Match Duration is in Seconds")]
    public float matchDuration = 180f;   

    private float timeRemaining;
    private bool gameEnded = false;

    public static GameTimer Instance { get; private set; }

    private void Awake(){
        if (Instance != null){
            Destroy(gameObject);
            return;
        }

        Instance = this;
        timeRemaining = matchDuration;
    }

    private void Update(){
        if (gameEnded)
            return;

        timeRemaining -= Time.deltaTime;

        // ⏱ UI hook futuro
        // OnTimeUpdated(timeRemaining);

        if (timeRemaining <= 0f && !gameEnded){
            gameEnded = true;
            timeRemaining = 0f;

            if (GameOverManager.Instance != null){
                GameOverManager.Instance.TriggerGameOver();
            }
        }
    }

    public float GetTimeRemaining(){
        return timeRemaining;
    }

    public bool IsMatchEnded(){
        return gameEnded;
    }

    // Adds seconds to the remaining match time. Ignored if the match already ended.
    public void AddTime(float seconds)
    {
        if (gameEnded)
            return;

        // Only allow positive additions
        if (seconds <= 0f)
            return;

        timeRemaining += seconds;
    }
    


}

