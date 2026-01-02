using UnityEngine;
using Unity.Cinemachine;



public class LookAtTargetAdjustor : MonoBehaviour
{
    public CinemachineThirdPersonFollow cam;
    public InputSystem_Actions controller;

    float steeringAngle;

    void OnEnable()
    {
        controller.Enable();
    }

    void OnDisable()
    {
        controller.Disable();
    }

    void Awake()
    {
        controller = new InputSystem_Actions();
        controller.Car.Steering.performed += steerCTX => UpdateInputs(steerCTX.ReadValue<float>());
        controller.Car.Steering.canceled += _ => UpdateInputs(0);

    }


    void UpdateInputs(float steering)
    {
        steeringAngle = steering;
    }

    void UpdateCameraValues()
    {
        cam.CameraSide = (-steeringAngle + 1f)/2f;
        //transform.rotation = Quaternion.AngleAxis(16,-transform.forward) * Quaternion.AngleAxis(90 + 10*steeringAngle,transform.up);

    }

    void FixedUpdate()
    {
        UpdateCameraValues();
    }

}