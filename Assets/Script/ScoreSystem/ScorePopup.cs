using System.Collections;
using TMPro;
using UnityEngine;

public class ScorePopup : MonoBehaviour
{
    public TextMeshProUGUI text;

    [Header("Animation")]
    public float lifetime = 0.6f;
    public float floatDistance = 40f;
    public float popScale = 1.3f;
    public float popDuration = 0.1f;

    private Vector3 startPos;
    private Vector3 originalScale;

    private void Awake(){
        startPos = transform.localPosition;
        originalScale = transform.localScale;        
    }

    public void Play(int amount){
        text.text = $"+{amount}";
        StartCoroutine(Animate());  
    }

    private IEnumerator Animate(){

        // 🔥 POP IN
        transform.localScale = originalScale * popScale;
        yield return new WaitForSeconds(popDuration);
        transform.localScale = originalScale;


        float time = 0f;

        // 🔼 FLOAT + FADE
        while (time < lifetime){
            time += Time.deltaTime;
            float t = time / lifetime;

            transform.localPosition = startPos + Vector3.up * floatDistance * t;
            text.alpha = 1f - t;

            yield return null;
        }

        Destroy(gameObject);
    }












}
