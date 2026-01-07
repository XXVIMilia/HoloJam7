using Unity.Cinemachine;
using UnityEngine;


public class NPCBaseClass : Knockable
{

    private GameObject _player;

    

    public enum NPCAction
    {
        DRIVING,
        BRAKE,
        SMACKED
    }

    [Header("Driving Status")]
    public Waypoint target;
    public float distToTarget;
    public NPCAction currentAction;
    public float carSpeed;
    public float turnSpeed;

    [Header("Driving Perameters")]
    public float laneChangeProbability;




    
    
    [Header("References")]
    [SerializeField] public Transform distanceToWaypoint;





 
    void TargetUpdate()
    {
        Vector3 distanceFromTarget = distanceToWaypoint.position - target.GetExactPosition();
        distToTarget = distanceFromTarget.magnitude;
        if(distanceFromTarget.magnitude < 1f)
        {
            if(target.NextWaypointA == null)//Any deadend
                Destroy(gameObject);


            if(target.waypointType == Waypoint.WaypointType.PATHING || target.waypointType == Waypoint.WaypointType.START)//Target Reached
            {
                float diceRoll = Random.Range(0f,1f);
                if(diceRoll < laneChangeProbability)
                {
                    if(target.name == "WaypointA")
                    {
                        target = target.NextWaypointB;
                    }
                    else
                    {
                        target = target.NextWaypointA;
                    }
                    
                }
                else
                {
                    if(target.name == "WaypointA")
                    {
                        target = target.NextWaypointA;
                    }
                    else
                    {
                        target = target.NextWaypointB;
                    }
                }
            }
        }
    }

    public virtual void DriveNPC(){}//Virtual Function for different NPC to override



    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _player)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        TargetUpdate();
        DriveNPC();
        
    }

}
