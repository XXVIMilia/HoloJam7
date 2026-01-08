using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(HorizontalLayoutGroup))]
public class Slots_Container : MonoBehaviour
{
    [Header("Slots Settings")]
    public Slot slotPrefab;
    public int maxIceCreams = 3;

    [Header("IceCream Visuals")]
    [SerializeField] private List<Texture> iceCreamIcons; 

    [Header("Layout Settings")]
    public float slotSpacing = 100f;
    public float slotWidth = 300f;

    private RectTransform rectTransform;
    private HorizontalLayoutGroup layoutGroup;

    private readonly List<Slot> slots = new();

    // ---------------- UNITY ---------------- //

    private void Awake(){

        rectTransform = GetComponent<RectTransform>();
        layoutGroup = GetComponent<HorizontalLayoutGroup>();
    
        layoutGroup.spacing = slotSpacing;

        CreateSlots();
        ResizeContainer();
      
    }

    // ---------------- SETUP ---------------- //
    
    private void ResizeContainer(){
        float totalWidth = (slotWidth * maxIceCreams) + (slotSpacing * (maxIceCreams - 1));
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalWidth);
    }

    
    private void CreateSlots(){
        for (int i = 0; i < maxIceCreams; i++){
            Slot slot = Instantiate(slotPrefab, transform);
            slot.gameObject.SetActive(false);

            slots.Add(slot);
        }
    }
    
    // ---------------- SLOT CONTROL ---------------- //

   public int GetMaxSlots(){
        return maxIceCreams;
    }

    public Slot AddIceCream(){
        Slot slot = GetNextFreeSlot();
        if (slot == null){
            Debug.LogWarning("No free slots available.");
            return null;
        }

        Texture randomIcon = GetRandomIceCreamIcon();
        slot.SetIceCreamVisual(randomIcon);
        slot.StartMelting();
        return slot;
    }
    
    private Slot GetNextFreeSlot(){
        foreach (Slot slot in slots){
            if (!slot.gameObject.activeSelf){
                slot.gameObject.SetActive(true);
                slot.ResetSlot();
                return slot;
            }
        }
        return null;
    }

    private Texture GetRandomIceCreamIcon(){
        if (iceCreamIcons == null || iceCreamIcons.Count == 0){
            Debug.LogWarning("No ice cream icons assigned!");
            return null;
        }

        return iceCreamIcons[UnityEngine.Random.Range(0, iceCreamIcons.Count)];
    }

    public void ReleaseSlot(Slot slot){
        if (slot == null) return;

        slot.StopMelting();
        slot.ResetSlot();
        slot.gameObject.SetActive(false);
    }
  

    public int GetActiveSlotsCount(){
        int count = 0;
        foreach (Slot slot in slots){
            if (slot.gameObject.activeSelf){
                count++;
            }
        }
        return count;
    }

    
    

    

}
