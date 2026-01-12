using System;
using UnityEngine;

[CreateAssetMenu(fileName ="New Scriptable", menuName = "SFX Dictionary")]
public class SFXScriptableScript : ScriptableObject
{
    [SerializeField]
    public SFXDict sfxDict;

    public AudioClip GetClipFromTag(String tag)
    {
        if (sfxDict.ContainsKey(tag))
        {
            return sfxDict[tag];
        }
        else
        {
            return sfxDict["CrashingPipe"];
        }
    }
}
