using UnityEngine;
    
public class InteractableObject : MonoBehaviour, I_Interactable{

    [Header("Interaction Settings")]
    public float interactionRange = 2f;

    [TextArea]
    public string interactionMessage = "Interact";

    [Header("Interaction Options")]
    public bool showInteractionMessage = true;
    public bool allowButtonInteraction = true;

    protected Transform currentInteractor;

    public virtual bool AllowButtonInteraction(){
        return true;
    }

    public virtual bool ShowInteractionMessage(){
        return true;
    }

    public bool CanInteract(Transform interactor){
        float distance = Vector3.Distance(transform.position, interactor.position);
        if (distance <= interactionRange){
            currentInteractor = interactor;
            return true;
        }
        currentInteractor = null;
        return false;
    }

    public string GetInteractionMessage(){
        return interactionMessage;
    }

    public virtual void Interact(){
        Debug.Log($"{name} interacted");
    }

   

    private void OnDrawGizmos(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
