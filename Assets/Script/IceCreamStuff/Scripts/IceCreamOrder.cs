using UnityEngine;

public class IceCreamOrder {
    public DropOffLocation Target {get ; private set;} 
    public Slot Slot {get ; private set;}

    private readonly PlayerIceCream owner;

    public IceCreamOrder(DropOffLocation target, Slot slot, PlayerIceCream owner){
        this.Target = target;
        this.Slot = slot;
        this.owner = owner;

        Slot.OnMelted += OnMelted;
        Slot.StartMelting();

    }

    public void OnMelted(Slot slot){
        Slot.OnMelted -= OnMelted;
        owner.LoseIceCream(this);
        Debug.Log("Ice cream melted!");
    }

    public void Complete(){
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
