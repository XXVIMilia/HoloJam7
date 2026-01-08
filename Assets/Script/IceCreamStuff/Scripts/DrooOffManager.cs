using UnityEngine;
using System.Collections.Generic;

public class DropOffManager : MonoBehaviour
{
   
   public static DropOffManager instance{ get; private set; }

    private readonly List<DropOffLocation> activeLocations = new();

    private void Awake(){
        if (instance != null && instance != this){
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    // ---------------- REGISTRATION ---------------- //

    public void Register(DropOffLocation location){
        if (!activeLocations.Contains(location)){
            activeLocations.Add(location);
        }
    }

    public void Unregister(DropOffLocation location){
        if (activeLocations.Contains(location)){
            activeLocations.Remove(location);
        }
    }

    // ---------------- REGISTRATION ---------------- //


    public DropOffLocation GetRandomDropOffLocation(){
        if (activeLocations.Count == 0){
            Debug.LogWarning("No active drop-off locations available.");
            return null;
        } 

        int randomIndex = Random.Range(0, activeLocations.Count);
        return activeLocations[randomIndex];
    }




}
