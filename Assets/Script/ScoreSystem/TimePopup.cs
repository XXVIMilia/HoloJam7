using System.Collections;
using TMPro;
using UnityEngine;

public class TimePopup : MonoBehaviour
{
    public TextMeshProUGUI text;

    [Header("Animation")]
    public float lifetime = 0.6f;
    public float floatDistance = 40f;

    // pop parameters no longer used; kept for compatibility if needed
    //public float popScale = 1.3f;
    //public float popDuration = 0.1f;

    private Vector3 startPos;
    private Vector3 originalScale;

    private void Awake(){
        startPos = transform.localPosition;
        originalScale = transform.localScale;        
    }

    public void Play(float seconds){
        // make sure the popup is visible only when we start animating
        gameObject.SetActive(true);
        text.text = $"+{seconds:0.#}s";
        StartCoroutine(Animate());  
    }

    private IEnumerator Animate(){
        // ensure starting scale and alpha
        transform.localScale = originalScale;
        text.alpha = 1f;

        float time = 0f;

        // SLIDE UP + FADE OUT over lifetime
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
