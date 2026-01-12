using UnityEngine;
using UnityEngine.UI;
using System;

public class Slot : MonoBehaviour{
    
    [Header("UI References")]
    public RawImage IconImage;
    public Image MeltBar;

    
    private float meltDuration;

    private float currentMeltTime;
    private bool isMelting = false;
   
    public event Action<Slot> OnMelted;


    private void OnEnable(){
        ResetSlot();
    }

    public void Update(){

        if (!isMelting || meltDuration <= 0f) return;

        currentMeltTime += Time.deltaTime;
        float fillAmount = Mathf.Clamp01(1 - (currentMeltTime / meltDuration));
        MeltBar.fillAmount = fillAmount;

        if (fillAmount <= 0){
            isMelting = false;
            OnMelted?.Invoke(this);
        }
        
    }

    // -------- VISUAL -------- //

    public void SetIceCreamVisual(Texture icon){
        IconImage.texture = icon;
        IconImage.enabled = true;
    }

    // ---------------- CONTROL ---------------- //

    public void StartMelting(float duration){
        meltDuration = duration;
        currentMeltTime = 0f;
        MeltBar.fillAmount = 1f;
        isMelting = true;
    }

    public void StopMelting(){
        isMelting = false;
    }

    public void ResetSlot(){

        if (IconImage == null){
            Debug.LogError($"Slot {name} has no IconImage assigned");
            return;
        }

        currentMeltTime = 0f;
        meltDuration = 0f;
        MeltBar.fillAmount = 1f;
        isMelting = false;

        IconImage.texture = null;
        IconImage.enabled = false;
    }

    public float GetMeltPercentage(){
        if (meltDuration <= 0f) return 1.0f;
        return Mathf.Clamp01(currentMeltTime / meltDuration);
    }

}   
