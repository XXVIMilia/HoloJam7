using System;
using UnityEngine;

[CreateAssetMenu(fileName ="New Scriptable", menuName = "SFX Dictionary")]
public class SFXScriptableScript : ScriptableObject
{
    [SerializeField]
    public SFXDict sfxDict;

    public AudioClip GetClipFromTag(String tag)
    {
        if (sfxDict != null && sfxDict.TryGetValue(tag, out var clip))
        {
            return clip;
        }

        Debug.LogWarning($"SFXDictionary: no clip found for tag '{tag}'.");
        return null;
    }
}
