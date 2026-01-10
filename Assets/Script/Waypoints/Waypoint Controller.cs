using UnityEngine;
using System.Collections.Generic;

public class WaypointController : MonoBehaviour
{
    [Tooltip("Manually add all potential waypoints to script")]
    [SerializeField] private GameObject[] _wayPointPosition;
    [SerializeField] private Transform _nearestPoint;
    [SerializeField] private GameObject car;
    public Transform nearestPoint => _nearestPoint;

    void Start()
    {
        car = transform.parent.gameObject;
        _wayPointPosition = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach (var item in _wayPointPosition)
        {
            if (item.transform.parent.GetComponent<DropOffLocation>() == null)
            {
                Debug.Log("skiped");
                continue;
            }
            else {
                Debug.Log("turning off");
                DropOffLocation drop = item.transform.parent.GetComponent<DropOffLocation>();
                drop.waypoint.SetActive(false);
            }
        }
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

        for (int i = 1; i < _wayPointPosition.Length; i++)
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
