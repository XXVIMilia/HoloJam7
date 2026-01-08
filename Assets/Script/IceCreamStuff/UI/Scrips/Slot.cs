using UnityEngine;
using UnityEngine.UI;
using System;

public class Slot : MonoBehaviour{
    
    [Header("UI References")]
    public RawImage IconImage;
    public Image MeltBar;

    [Header("Melt Settings")]
    public float meltDuration = 10f;

    private float currentMeltTime;
    private bool isMelting = false;

    public event Action<Slot> OnMelted;

    private void OnEnable(){
        ResetSlot();
}

    public void Update(){
        if (isMelting){
            currentMeltTime += Time.deltaTime;
            float fillAmount = Mathf.Clamp01(1 - (currentMeltTime / meltDuration));
            MeltBar.fillAmount = fillAmount;

            if (fillAmount <= 0){
                isMelting = false;
                OnMelted?.Invoke(this);
            }
        }
    }

    // -------- VISUAL -------- //

    public void SetIceCreamVisual(Texture icon){
        IconImage.texture = icon;
        IconImage.enabled = true;
    }

    // ---------------- CONTROL ---------------- //

    public void StartMelting(){
        currentMeltTime = 0f;
        MeltBar.fillAmount = 1f;
        isMelting = true;
    }

    public void StopMelting(){
        isMelting = false;
    }

    public void ResetSlot(){
        currentMeltTime = 0f;
        MeltBar.fillAmount = 1f;
        isMelting = false;

        IconImage.texture = null;
        IconImage.enabled = false;
    }

}   
