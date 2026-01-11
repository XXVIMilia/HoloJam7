using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] private ScoreUI scoreUI;

    [Header("Scoring Settings")]
    public float pointPerDistance = 10f;
    public float excellentMultiplier = 1.5f;
    public float goodMultiplier = 1.2f;
    public float fairMultiplier = 1.0f;
    public float poorMultiplier = 0.8f;
    
    public static ScoreManager Instance { get; private set; }

    private int totalscore;

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void addDeliveryScore(Vector3 pickupPos, Vector3 dropoffPos, float meltPercent){
        float distance = Vector3.Distance(pickupPos, dropoffPos);
        int baseScore = Mathf.RoundToInt(distance * pointPerDistance);

        float meltMultiplier = GetMeltMultiplier(meltPercent);
        int finalScore = Mathf.RoundToInt(baseScore * meltMultiplier);

        totalscore += finalScore;
        Debug.Log($"Delivery Score: {finalScore} (Base: {baseScore}, Melt Multiplier: {meltMultiplier}) | Total Score: {totalscore}");

        // Notify UI
        if(scoreUI != null){
            scoreUI.UpdateScore(totalscore);
        }
    }

    private float GetMeltMultiplier(float meltPercent){
        if (meltPercent >= 90f) return excellentMultiplier;      // Excellent
        else if (meltPercent >= 70f) return goodMultiplier;      // Good
        else if (meltPercent >= 50f) return fairMultiplier;      // Fair
        else return poorMultiplier;                              // Poor
    }

    public int GetTotalScore(){
        return totalscore;
    }
    
}
