using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
public class AutoInteract : MonoBehaviour
{
    [Header("Auto Interaction Settings")]
    public float interactionDelay = 0.5f;   
    public float cooldownTime = 1.0f;       

    private InteractableObject interactable;
    private Transform player;

    private float interactionTimer;
    private float cooldownTimer;

    private void Awake(){
        interactable = GetComponent<InteractableObject>();
        interactionTimer = interactionDelay;
    }

    private void Update(){
        if (player == null){
            return;
        }    

        if (cooldownTimer > 0f){
            cooldownTimer -= Time.deltaTime;
            return;
        }

        if (!interactable.CanInteract(player)){
            ResetInteraction();
            return;
        }

        interactionTimer -= Time.deltaTime;

        if (interactionTimer <= 0f){
            TriggerInteraction();
        }
    }

    private void TriggerInteraction(){
        interactable.Interact();

        cooldownTimer = cooldownTime;
        ResetInteraction();
    }

    private void ResetInteraction(){
        interactionTimer = interactionDelay;
    }

    // ------------- Player detection -----------

    private void OnTriggerEnter(Collider other){
        if (!other.CompareTag("Player"))
            return;

        player = other.transform;
      
        interactionTimer = interactionDelay;
    }

    private void OnTriggerExit(Collider other){
        if (!other.CompareTag("Player"))
            return;

        player = null;
        ResetInteraction();
    }
    
}
