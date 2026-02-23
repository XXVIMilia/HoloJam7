using UnityEngine;

public class DeliverSFX : MonoBehaviour
{
    public SFXScriptableScript sFXScriptableScript;
    public AudioSource audioSource;

    public void PlaySFXFromLookup(string lookup)
    {
        // Debug.Log(sFXScriptableScript.name);
        AudioClip clip = sFXScriptableScript.GetClipFromTag(lookup);
        if (clip == null)
            return; // nothing to play

        audioSource.PlayOneShot(clip);
    }

}
