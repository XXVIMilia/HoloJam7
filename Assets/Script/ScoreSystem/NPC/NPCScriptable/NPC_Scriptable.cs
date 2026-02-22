using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="New Scriptable", menuName = "NPCDictionary")]
public class NPC_Scriptable : ScriptableObject
{
    [SerializeField]
    public NPC_Dictionary npcDict;

    public SerializableDictionary<int,GameObject> GetNPC(NPCInfo.prefabType type)
    {
        if (npcDict.ContainsKey(type))
        {
            return npcDict[type];
        }
        else
        {
            return null;
        }
    }
}
