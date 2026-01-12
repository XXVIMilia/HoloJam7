using UnityEngine;


public class DropOffLocation : InteractableObject{
    public GameObject waypoint;
           
    public override void Interact(){
        if (currentInteractor == null){
            Debug.LogWarning("DropOffLocation: No interactor found.");
            return;
        }

        PlayerIceCream playerIceCream = currentInteractor.GetComponent<PlayerIceCream>();

        if (playerIceCream == null){
            Debug.LogWarning("DropOffLocation: Interactor has no PlayerIceCream.");
            return;
        }

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


    public override bool ShowInteractionMessage(){
        return false;
    }

    public override bool AllowButtonInteraction(){
        return false;
    }

}