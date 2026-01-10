using UnityEngine;

public class IceCreamShop : InteractableObject{

    public GameObject waypoint;

    public override void Interact(){
        base.Interact();

        if (currentInteractor == null) return;

        PlayerIceCream player = currentInteractor.GetComponent<PlayerIceCream>();
        if (player == null) return;

        player.GiveIceCream();
    }

    private void Start(){
        if (DropOffManager.instance == null){
            Debug.LogWarning("DropOffManager not ready yet.");
            return;
        }
        DropOffManager.instance.ShopRegister(this);
    }

    private void OnDestroy(){
        DropOffManager.instance.ShopUnregister(this);
    }
}
