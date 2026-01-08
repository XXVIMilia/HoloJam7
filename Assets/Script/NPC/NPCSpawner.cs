using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Pool;

public class NPCSpawner : MonoBehaviour
{
    public ObjectPool<GameObject> _pool;
    public Transform PoolOrigin;
    public NPC_Scriptable nPC_Scriptable;

    
   

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Street"))
        {
            Waypoint waypointGameObject = other.transform.GetComponentInParent<SmartStreet>().GetWaypoint();
            SerializableDictionary<int,GameObject> assets = nPC_Scriptable.GetNPC(NPCInfo.RandomPrefab());
            int randomIndex = Random.Range(0,assets.Count);
            GameObject asset = assets[randomIndex];
            if(asset != null)
            {
                GameObject instance = Instantiate(asset);
                instance.transform.position = waypointGameObject.transform.position;
                instance.transform.rotation = waypointGameObject.transform.rotation * Quaternion.AngleAxis(180f,Vector3.up);
                instance.GetComponent<NPCBaseClass>().target = waypointGameObject.NextWaypointA;
                if(instance.GetComponent<NPCBaseClass>().target == null)
                {
                    Destroy(instance.gameObject);
                }
                instance.transform.SetParent(PoolOrigin);
            }
            
        }
    }
}
