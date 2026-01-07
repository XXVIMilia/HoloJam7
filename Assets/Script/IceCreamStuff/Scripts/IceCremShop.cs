using UnityEngine;

public class IceCreamShop : InteractableObject{

    
    public override void Interact(){
        base.Interact();

        if (currentInteractor == null) return;

        PlayerIceCream player = currentInteractor.GetComponent<PlayerIceCream>();
        if (player == null) return;

        player.GiveIceCream();
    }
}
