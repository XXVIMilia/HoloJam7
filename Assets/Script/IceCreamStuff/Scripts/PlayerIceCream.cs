using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIceCream : MonoBehaviour{

    [Header("UI References")]
    public Slots_Container slotsContainer;
       
    private readonly List<IceCreamOrder> activeOrders = new();        

    ///---------------Getters----------------///
    
    public bool HasIceCream(){
         return activeOrders.Count > 0;

    }

    public bool IsFull(){
        return activeOrders.Count >= slotsContainer.GetMaxSlots();
    }

    ///---------------Logic----------------///
    

    public void GiveIceCream(){
        if (IsFull()){
            Debug.Log("IceCream slots full!");
            return;
        }


        DropOffLocation target = DropOffManager.instance.GetRandomDropOffLocation();

        if (target == null){
            Debug.Log("No available drop-off locations.");
            return;
        }

        Slot slot = slotsContainer.AddIceCream();

        if (slot == null){      
            Debug.Log("No free slot available.");
            return;
        }

        IceCreamOrder newOrder = new IceCreamOrder(target, slot, this, transform.position);
        activeOrders.Add(newOrder);

        Debug.Log("IceCream received!. Current IceCreams: " + activeOrders.Count);
    }


    public void DeliverIceCream(DropOffLocation location){
        IceCreamOrder order = activeOrders.Find(o => o.Target == location);

        if (order == null){
            Debug.Log("No ice cream order for this location.");
            return;
        }

        order.Complete();
        activeOrders.Remove(order);
    }

    public void LoseIceCream(IceCreamOrder order){
        if (!activeOrders.Contains(order)){
            Debug.Log("Order not found among active orders.");
            return;
        }

        order.fail();
        activeOrders.Remove(order);

    }
    
}
