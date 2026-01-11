using System.Globalization;
using UnityEngine;

public class IceCreamOrder {
    public DropOffLocation Target {get ; private set;} 
    public Slot Slot {get ; private set;}

    private readonly PlayerIceCream owner;
    private readonly Vector3 pickupPosition;

    
     
    public float MeltDuration { get; private set; }

    //🔧(Balancing)
    private const float secondsPerUnit = 0.6f;
    private const float minMeltTime = 5f;
    private const float maxMeltTime = 25f;



    public IceCreamOrder(DropOffLocation target, Slot slot, PlayerIceCream owner, Vector3 pickupPosition){
        this.Target = target;
        this.Slot = slot;
        this.owner = owner;
        this.pickupPosition = pickupPosition;

        CalculateMeltDuration();

        Slot.OnMelted += OnMelted;

        Slot.StartMelting(MeltDuration);

    }
    // ---------------- CALCULATION ---------------- //

    private void CalculateMeltDuration(){
        if (Target == null){
            MeltDuration = minMeltTime;
            return;
        }

        float distance = Vector3.Distance(pickupPosition, Target.transform.position);
        float calculatedTime = distance * secondsPerUnit;

        MeltDuration = Mathf.Clamp(calculatedTime, minMeltTime, maxMeltTime);
    }


    // ----------------- EVENTS ------------------ //

    public void OnMelted(Slot slot){
        Slot.OnMelted -= OnMelted;
        owner.LoseIceCream(this);
        // Debug.Log("Ice cream melted!");
    }

    public void Complete(){
        float meltPercent = Slot.GetMeltPercentage();

        if(ScoreManager.Instance != null){
            ScoreManager.Instance.addDeliveryScore(pickupPosition, Target.transform.position, meltPercent);
        }


        CleanUp();
        // Debug.Log("Ice cream order completed!");
    }

    public void Fail(){
        CleanUp();
        // Debug.Log("Ice cream Melted and lost!");
    }

    public void CleanUp(){
        Slot.OnMelted -= OnMelted;
        owner.slotsContainer.ReleaseSlot(Slot);
    }


}
