using UnityEngine;

public class WayPointer : MonoBehaviour
{
    [SerializeField] private WaypointController _controller;
    [SerializeField] private float angleAdjustment;
    [Tooltip("IDK Atleast 500")]
    [SerializeField] private float speed = 500.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _dir = _controller.nearestPoint.transform.position - transform.position;
        float singleStep = speed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.up, _dir, singleStep, 0.0f);

        transform.rotation =  Quaternion.LookRotation(newDirection);

    }
}