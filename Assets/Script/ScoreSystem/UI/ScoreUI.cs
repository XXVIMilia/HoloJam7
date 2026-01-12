using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class ScoreUI: MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;

    private void Start(){
        if(ScoreManager.Instance != null){
            UpdateScore(ScoreManager.Instance.GetTotalScore());
        }  
    }

    public void UpdateScore(int newScore){
        if(scoreText == null) return;
        scoreText.text =  $"Score: {newScore}";
        
    }













}
