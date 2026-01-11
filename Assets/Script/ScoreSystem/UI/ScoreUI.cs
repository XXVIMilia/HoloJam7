using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreUI: MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;

    [Header("Count Animation")]
    public float countDuration = 0.3f;
    private int displayedScore = 0;
    private Coroutine countCoroutine;

    [Header("Popup Settings")]
    public RectTransform popupSpawnPoint;
    public ScorePopup popupPrefab;  

    private void Awake(){
        if(scoreText == null){
            Debug.LogError("ScoreUI: scoreText not assigned!");
            return; 
        }
    
    }

    private void Start(){
        if(ScoreManager.Instance != null){
            displayedScore = ScoreManager.Instance.GetTotalScore();
            UpdateText(displayedScore);
        }  
    }

    public void OnScoreAdded(int addedScore, int targetScore){
        SpawnPopup(addedScore);

        if (countCoroutine != null)
            StopCoroutine(countCoroutine);

        countCoroutine = StartCoroutine(CountScore(displayedScore, targetScore));
    }

    private IEnumerator CountScore(int from, int to){
        float time = 0f;

        while (time < countDuration){
            time += Time.deltaTime;
            float t = time / countDuration;

            displayedScore = Mathf.RoundToInt(Mathf.Lerp(from, to, t));
            UpdateText(displayedScore);

            yield return null;
        }

        displayedScore = to;
        UpdateText(displayedScore);
       
    }

    private void SpawnPopup(int amount){
        if (popupPrefab == null || popupSpawnPoint == null) return;

        ScorePopup popup = Instantiate(popupPrefab, popupSpawnPoint);
        popup.Play(amount);
    }

    private void UpdateText(int value){
        scoreText.text = $"Funds: ${value}";
    }

}
