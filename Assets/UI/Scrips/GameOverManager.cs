using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;


public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    [Header("UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI resultText;     
    public TextMeshProUGUI finalScoreText;
    public UnityEngine.UI.Button retryButton;


    [Header("Win Condition")]
    public int scoreToWin = 1000;

    [Header("Text Pulse Animation")]
    public float pulseScale = 1.25f;
    public float pulseSpeed = 1.5f;

    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip winClip;
    public AudioClip loseClip;

    private Vector3 originalScale;
    private bool gameEnded = false;
    private Coroutine pulseCoroutine;

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start(){
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (resultText != null)
            originalScale = resultText.transform.localScale;

        if (retryButton != null)
            retryButton.onClick.AddListener(Retry);
    }

    public void TriggerGameOver(){
        if (gameEnded)
            return;

        gameEnded = true;

        //Freeze game
        Time.timeScale = 0f;

        // Show GameOver UI
        if (gameOverPanel != null){
            gameOverPanel.SetActive(true);
        }
        
        int finalScore = 0;

        if (ScoreManager.Instance != null){
            finalScore = ScoreManager.Instance.GetTotalScore();
        }

        bool win = finalScore >= scoreToWin;

        if (win){
            WinnerFunction();
        }else{
            LooserFunction();
        }

        pulseCoroutine = StartCoroutine(PulseText());
        if (finalScoreText != null){
            finalScoreText.text = $"Final Score: {finalScore}";
        }

        audioSource.Play();
    }

    // ---------------- RESULTS ---------------- //
    public void WinnerFunction(){
        resultText.text = "YOU WIN";
        resultText.color = Color.green;
        audioSource.clip = winClip;
    }

    public void LooserFunction(){
        resultText.text = "Game Over";
        resultText.color = Color.red;
        audioSource.clip = loseClip;
        
    }

    // ---------------- ANIMATION ---------------- //

    private IEnumerator PulseText(){
        float t = 0f;

        while (true){
            t += Time.unscaledDeltaTime * pulseSpeed;
            float scale = Mathf.Lerp(1f,pulseScale,(Mathf.Sin(t) + 1f) * 0.5f);

            resultText.transform.localScale = originalScale * scale;

            yield return null;
        }
    }

    // ---------------- BUTTONS ---------------- //

    private void Retry(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
