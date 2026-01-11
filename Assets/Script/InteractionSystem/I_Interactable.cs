using UnityEngine;

public interface I_Interactable{
    
    bool CanInteract(Transform interactor);
    string GetInteractionMessage();
    void Interact();
    bool ShowInteractionMessage();
    bool AllowButtonInteraction();

}
