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
        LayerMask layerToCheck = LayerMask.GetMask("Street");
        Collider[] newrbyStreet = Physics.OverlapSphere(transform.position,75f);
        Collider closest = null;
        float closestColDist = Mathf.Infinity;
        Vector3 directionToTarget;
        foreach (Collider col in newrbyStreet){
            if(!col.CompareTag("Street")) continue;

            if(closest == null)
            {
                closest = col;
                directionToTarget = col.transform.position - transform.position;
                closestColDist = directionToTarget.sqrMagnitude;
            }
            else
            {
                directionToTarget = col.transform.position - transform.position;
                float dist = directionToTarget.sqrMagnitude;
                if(dist < closestColDist)
                {
                    closestColDist = dist;
                    closest = col;
                }
            }
        }

        Quaternion direction;
        if(closest != null)
        {
            SmartStreet streetScript = closest.GetComponentInParent<SmartStreet>();
            Transform ustuckPosition = streetScript.GetWaypoint().transform;
            _rb.linearVelocity = Vector3.zero;
            transform.position = new Vector3(ustuckPosition.position.x, ustuckPosition.position.y + _heightChange, ustuckPosition.position.z);
            direction = Quaternion.Euler(0, -ustuckPosition.rotation.y, 0);
            transform.rotation = direction;
            _timer = _cooldown;
        }
        else
        {
            _timer = _cooldown;
            _rb.linearVelocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, transform.position.y + _heightChange, transform.position.z);
            direction = Quaternion.Euler(0, transform.rotation.y, 0);
            transform.rotation = direction;
        }

        
    }
}
