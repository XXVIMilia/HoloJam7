using UnityEngine;


public class DropOffLocation : InteractableObject{
    public GameObject waypoint;
    public override void Interact(){
        base.Interact();

        if (currentInteractor == null) return;

        PlayerIceCream playerIceCream = currentInteractor.GetComponent<PlayerIceCream>();

        if (playerIceCream == null) return;

        playerIceCream.DeliverIceCream(this);
    }
    
    private void Start(){
        if (DropOffManager.instance == null){
            Debug.LogWarning("DropOffManager not ready yet.");
            return;
        }
        DropOffManager.instance.Register(this);
    }

    private void OnDestroy(){
        DropOffManager.instance.Unregister(this);
    }

}