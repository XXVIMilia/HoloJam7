using UnityEngine;
using TMPro;
using System.Collections;
using System.Reflection.Emit;

public class TimerUI : MonoBehaviour{
    public static TimerUI Instance { get; private set; }

    [Header("UI Reference")]
    public TextMeshProUGUI timerText;

    [Header("Colors")]
    public Color normalColor = Color.green;
    public Color halfTimeColor = Color.yellow;
    public Color warningColor = Color.red;

    [Header("Warning Animation")]
    public float pulseScale = 1.25f;
    public float pulseDuration = 0.15f;

    private Vector3 originalScale;
    private int lastSecondDisplayed = -1;
    private Coroutine pulseCoroutine;

    private void Awake(){
        // enforce singleton
        if (Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (timerText == null)
        {
            Debug.LogError("TimerUI: timerText not assigned!");
            enabled = false;
            return;
        }

        originalScale = timerText.transform.localScale;
    }

    private void Update(){
        if (GameTimer.Instance == null)
            return;

        float timeRemaining = GameTimer.Instance.GetTimeRemaining();
        UpdateTimer(timeRemaining);
    }

    private void UpdateTimer(float time){
        time = Mathf.Max(0, time);

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";

        UpdateColor(time);
        HandleLastSecondsPulse(seconds, time);
    }

    #region PopupSupport

    [Header("Popup Settings")]
    public RectTransform popupSpawnPoint;
    public TimePopup popupPrefab;

    public void SpawnTimePopup(float secondsAdded)
    {
        if (popupPrefab == null || popupSpawnPoint == null) return;
        TimePopup popup = Instantiate(popupPrefab, popupSpawnPoint);
        popup.Play(secondsAdded);
    }

    #endregion

    // ---------------- COLOR LOGIC ---------------- //

    private void UpdateColor(float timeRemaining){
        float halfTime = GameTimer.Instance.matchDuration * 0.5f;

        if (timeRemaining <= 10f){
            timerText.color = warningColor;


        }
        else if (timeRemaining <= halfTime){
            timerText.color = halfTimeColor;
            // Debug.Log("Color Cambiado: Media Vida");            
        }
        else{
            timerText.color = normalColor;
            // Debug.Log("Color Cambiado");            
        }
    }

    // ---------------- ANIMATION ---------------- //

    private void HandleLastSecondsPulse(int currentSecond, float timeRemaining){
        if (timeRemaining > 10f){
            lastSecondDisplayed = currentSecond;
            return;
        }

        if (currentSecond != lastSecondDisplayed){
            lastSecondDisplayed = currentSecond;
            PlayPulse();
        }
    }

    private void PlayPulse(){
        if (pulseCoroutine != null)
            StopCoroutine(pulseCoroutine);

        pulseCoroutine = StartCoroutine(PulseRoutine());
    }

    private IEnumerator PulseRoutine(){
        timerText.transform.localScale = originalScale * pulseScale;

        yield return new WaitForSeconds(pulseDuration);

        timerText.transform.localScale = originalScale;
        pulseCoroutine = null;
    }
    
}
