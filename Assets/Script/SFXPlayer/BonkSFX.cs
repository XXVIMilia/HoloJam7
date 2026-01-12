using UnityEngine;

public class BonkSFX : MonoBehaviour
{
    public SFXScriptableScript sFXScriptableScript;
    public AudioSource audioSource;

    public void PlaySFXFromLookup(string lookup)
    {
        switch (lookup)
        {
            case "Biboo":
            case "Fauna":
            case "Fuwamoco":
            case "Kronii":
                audioSource.volume = 0.5f;
                break;
            default:
                audioSource.volume = 1f;
                break;
        }
        audioSource.PlayOneShot(sFXScriptableScript.GetClipFromTag(lookup));
    }

}
