using UnityEngine;

public class DeliverSFX : MonoBehaviour
{
    public SFXScriptableScript sFXScriptableScript;
    public AudioSource audioSource;

    public void PlaySFXFromLookup(string lookup)
    {
        Debug.Log(sFXScriptableScript.name);

        audioSource.PlayOneShot(sFXScriptableScript.GetClipFromTag(lookup));
    }

}
