using UnityEngine;
using System.Collections.Generic;

public class DropOffManager : MonoBehaviour
{

    public static DropOffManager instance { get; private set; }

    [SerializeField] private readonly List<DropOffLocation> activeLocations = new();
    [SerializeField] private List<IceCreamShop> shopLocations = new();
    [SerializeField] private bool showWaypoint = true;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    // ---------------- REGISTRATION ---------------- //

    public void Register(DropOffLocation location)
    {
        if (!activeLocations.Contains(location))
        {
            activeLocations.Add(location);
        }
    }

    public void Unregister(DropOffLocation location)
    {
        if (activeLocations.Contains(location))
        {
            activeLocations.Remove(location);
        }
    }

    public void ShopRegister(IceCreamShop location)
    {
        if (!shopLocations.Contains(location))
        {
            shopLocations.Add(location);
        }
    }

    public void ShopUnregister(IceCreamShop location)
    {
        if (shopLocations.Contains(location))
        {
            shopLocations.Remove(location);
        }
    }

    // ---------------- REGISTRATION ---------------- //


    public DropOffLocation GetRandomDropOffLocation()
    {
        if (activeLocations.Count == 0)
        {
            Debug.LogWarning("No active drop-off locations available.");
            return null;
        }

        int randomIndex = Random.Range(0, activeLocations.Count);
        return activeLocations[randomIndex];
    }

    public void ToggleShopWaypoint()
    {
        if (shopLocations.Count == 0)
        {
            Debug.LogWarning("No active drop-off locations available.");
            return;
        }

        for (int i = 0; i < shopLocations.Count; i++)
        {
            if (showWaypoint)
            {
                shopLocations[i].waypoint.SetActive(false);
                Debug.Log("turning waypoint off");
                continue;
            }
            else if (!showWaypoint)
            {
                shopLocations[i].waypoint.SetActive(true);
                Debug.Log("turning waypoint on");
                continue;
            }
        }

        if (showWaypoint)
        {
            showWaypoint = false;
        }
        else
        {
            showWaypoint = true;

        }
    }


}
