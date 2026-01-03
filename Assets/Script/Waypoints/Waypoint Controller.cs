using UnityEngine;
using System.Collections.Generic;

public class WaypointController : MonoBehaviour
{
    [Tooltip("Manually add all potential waypoints to script")]
    [SerializeField] private List<Transform> _wayPointPosition = new List<Transform>();
    [SerializeField] private Transform _nearestPoint;
    [SerializeField] private GameObject car;
    public Transform nearestPoint => _nearestPoint;

    void Start()
    {
        car = transform.parent.gameObject;
    }


    void Update()
    {
        CheckNearestWaypoint();
        Debug.DrawLine(transform.position, _nearestPoint.position, Color.white, 0.0f);
    }

    void CheckNearestWaypoint()
    {
        int nearestIndex = 0;
        float minPointDistance = Vector3.Distance(car.transform.position, _wayPointPosition[0].transform.position);

        for (int i = 1; i < _wayPointPosition.Count; i++)
        {
            float currentPointDistance = Vector3.Distance(car.transform.position, _wayPointPosition[i].transform.position);
            if (currentPointDistance <= minPointDistance)
            {
                minPointDistance = currentPointDistance;
                nearestIndex = i;
            }
        }

        _nearestPoint = _wayPointPosition[nearestIndex].transform;
    }

}
