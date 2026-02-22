using UnityEngine;

[System.Serializable]
public class NPCInfo
{
    public enum prefabType
    {
        LONG,
        SHORT,
        SPORTS,
        PICKUP,
        VAN,
        POLICE,
        Count
    }

    public GameObject prefab;

    public static prefabType RandomPrefab()
    {
        return (prefabType) Random.Range(0,(int)prefabType.Count);
    }

    public static prefabType fromString(string toMatch)
    {
        switch (toMatch)
        {
            case "LONG":
                return prefabType.LONG;
            case "SHORT":
            return prefabType.SHORT;
            case "SPORTS":
            return prefabType.SPORTS;
            case "PICKUP":
            return prefabType.PICKUP;
            case "VAN":
            return prefabType.VAN;
            case "POLICE":
            return prefabType.POLICE;
            default:
                return prefabType.SHORT;
        }
    }


}
