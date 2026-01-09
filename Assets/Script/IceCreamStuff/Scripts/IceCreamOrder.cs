using UnityEngine;

public class IceCreamOrder {
    public DropOffLocation Target {get ; private set;} 
    public Slot Slot {get ; private set;}

    private readonly PlayerIceCream owner;

    private readonly Vector3 pickupPosition;

    public IceCreamOrder(DropOffLocation target, Slot slot, PlayerIceCream owner, Vector3 pickupPosition){
        this.Target = target;
        this.Slot = slot;
        this.owner = owner;
        this.pickupPosition = pickupPosition;

        Slot.OnMelted += OnMelted;
        Slot.StartMelting();

    }

    public void OnMelted(Slot slot){
        Slot.OnMelted -= OnMelted;
        owner.LoseIceCream(this);
        Debug.Log("Ice cream melted!");
    }

    public void Complete(){
        float meltPercent = Slot.GetMeltPercentage();
        if(ScoreManager.Instance != null){
            ScoreManager.Instance.addDeliveryScore(pickupPosition, Target.transform.position, meltPercent);
        }


        CleanUp();
        Debug.Log("Ice cream order completed!");
    }

    public void fail(){
        CleanUp();
        Debug.Log("Ice cream Melted and lost!");
    }

    public void CleanUp(){
        Slot.OnMelted -= OnMelted;
        owner.slotsContainer.ReleaseSlot(Slot);
    }


}
