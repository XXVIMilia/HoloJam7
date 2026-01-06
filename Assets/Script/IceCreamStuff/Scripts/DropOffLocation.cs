using UnityEngine;


public class DropOffLocation : InteractableObject{

    public override void Interact(){
        base.Interact();

        if (currentInteractor == null) return;

        PlayerIceCream playerIceCream = currentInteractor.GetComponent<PlayerIceCream>();

        if (playerIceCream == null) return;

        playerIceCream.DeliverIceCream(this);
    }
    
    private void OnEnable(){

        DropOffManager.instance.Register(this);
    }

    private void OnDisable(){
        DropOffManager.instance.Unregister(this);
    }

}