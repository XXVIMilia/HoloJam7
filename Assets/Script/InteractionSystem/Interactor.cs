using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.InputSystem.Utilities;
using System;

public class Interactor : MonoBehaviour{
    [Header("Interaction Settings")]
    public Transform InteractorSource;
    public float interactRange = 3f;
    public bool canInteract;

    [Header("UI")]
    public TextMeshProUGUI interactText;
    public GameObject interactPromptUI;

    // Input System
    private InputSystem_Actions inputActions;
    private InputDevice lastDevice;
    private IDisposable anyButtonListener;
    private bool usingGamepad;

    private I_Interactable currentInteractable;

    private void Awake(){
        inputActions = new InputSystem_Actions();

        if (InteractorSource == null){
            InteractorSource = this.transform;
        }
    }

    private void OnEnable(){
        inputActions.Car.Interact.performed += OnInteract;
        inputActions.Enable();

        anyButtonListener = InputSystem.onAnyButtonPress.Call(OnAnyButtonPressed);
    }

    private void OnDisable(){
        inputActions.Car.Interact.performed -= OnInteract;
        inputActions.Disable();

        anyButtonListener?.Dispose();
    }

    private void Update(){

        if (InteractorSource == null || interactPromptUI == null){
            return;
        }

        currentInteractable = GetNearbyInteractable();
        canInteract = currentInteractable != null;

        interactPromptUI.SetActive(canInteract);

        if (canInteract && interactText != null){
            interactText.text =
                $"Press {GetInteractKey()} - {currentInteractable.GetInteractionMessage()}";
        }
    }

    private void OnInteract(InputAction.CallbackContext context){
        lastDevice = context.control.device;

        if (!canInteract || currentInteractable == null){
            return;
        }
        if (currentInteractable.CanInteract(InteractorSource)){
            currentInteractable.Interact();
        }
    }

    private I_Interactable GetNearbyInteractable(){
        Collider[] hits = Physics.OverlapSphere(
            InteractorSource.position,
            interactRange
        );

        foreach (var hit in hits){
            if (hit.TryGetComponent(out I_Interactable interactable)){
                if (interactable.CanInteract(InteractorSource))
                {
                    return interactable;
                }
            }
        }

        return null;
    }

    private string GetInteractKey(){
        var action = inputActions.Car.Interact;
        string targetGroup = usingGamepad ? "Gamepad" : "Keyboard&Mouse";

        for (int i = 0; i < action.bindings.Count; i++){
            var binding = action.bindings[i];

            if (binding.isComposite || binding.isPartOfComposite){
                continue;
            }
            if (binding.groups.Contains(targetGroup)){
                return action.GetBindingDisplayString(i);
            }
        }

        return action.GetBindingDisplayString();
    }

    private void OnAnyButtonPressed(InputControl control){
        usingGamepad = control.device is Gamepad;
    }

    private void OnDrawGizmos(){
        if (InteractorSource == null)
            return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(InteractorSource.position, interactRange);
    }
}
