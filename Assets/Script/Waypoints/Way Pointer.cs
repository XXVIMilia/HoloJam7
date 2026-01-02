using UnityEngine;

public class WayPointer : MonoBehaviour
{
    [SerializeField] private WaypointController _controller;
    [SerializeField] private RectTransform _pointer;
    [SerializeField] private float angleAdjustment;
    private Vector3 _dir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _dir = (_controller.nearestPoint - _controller.transform.position).normalized;
        _dir.y = 0;


        float angle = Mathf.Atan2(_dir.z, _dir.x) * Mathf.Rad2Deg;
        if (angle < 0)
        {
            angle += 360;
        }
        _pointer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + angleAdjustment));

    }
}