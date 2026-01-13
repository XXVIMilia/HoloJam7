using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class Unstuck : MonoBehaviour
{
    [SerializeField] private float _heightChange = 10f;
    [SerializeField] private float _cooldown = 10f;
    [SerializeField] private float _timer = 0f;
    private InputSystem_Actions controller;
    private Rigidbody _rb;

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
        controller.Car.Unstuck.performed += Unstick;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
    }

    private void Unstick(InputAction.CallbackContext context)
    {
        if (_timer > 0)
        {
            return;
        }
        _timer = _cooldown;
        _rb.linearVelocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x, transform.position.y + _heightChange, transform.position.z);
        
        Quaternion direction = Quaternion.Euler(0, transform.rotation.y, 0);
        transform.rotation = direction;
    }
}
