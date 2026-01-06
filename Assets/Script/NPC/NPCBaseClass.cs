using UnityEngine;


public class NPCBaseClass : MonoBehaviour
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
    public NPCAction currentAction;

    [Header("Driving Perameters")]
    public float topSpeed;
    public AnimationCurve powerCurve;
    public AnimationCurve steeringCurve;
    public float maxSteeringAngle;
    public float ackermanConstant;
    public float tireMass;
    [SerializeField] public Transform FR_Tire;
    [SerializeField] public Transform FL_Tire;
    [SerializeField] public Transform BL_Tire;
    [SerializeField] public Transform BR_Tire;
    


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _player)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

}
