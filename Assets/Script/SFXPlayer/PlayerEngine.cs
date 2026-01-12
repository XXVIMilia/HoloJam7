using UnityEngine;

public class PlayerEngine : MonoBehaviour
{
    [Header("Configurations")]
    public AnimationCurve engineShift;
    public float maxPlayerSpeed;

    [Header("References")]
    public Rigidbody CarRB;
    public AudioClip A_Track_Normal;
    public AudioClip B_Track_Normal;
    public AudioClip B_Track_Drift;
    // public AudioClip A_Track_Silly;
    // public AudioClip B_Track_Silly;
    public AudioSource A_Track;
    public AudioSource B_Track;
    

    //Interior Variables
    [SerializeField]
    // private bool swapLock;
    private float currentSpeedRatio;
    // private float diceRoll;
    private InputSystem_Actions controller;
    private float throttle;
    private bool drifting;
    private bool driftActive;



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
        controller.Car.Acceleration.performed += accelCTX => UpdateThrottle(accelCTX.ReadValue<float>());
        controller.Car.Acceleration.canceled += _ => UpdateThrottle(0f);
        controller.Car.Drift.started += _ => UpdateDriftInput(true);
        controller.Car.Drift.canceled += _ => UpdateDriftInput(false);

    }

    
    public void UpdateThrottle(float inputThrottle)
    {
        throttle = inputThrottle * 0.9f;
    }

    public void UpdateDriftInput(bool isDrifting)
    {
        drifting = isDrifting;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        A_Track.loop = false;
        B_Track.loop = true;
        A_Track.volume = 1f;
        B_Track.volume = 0f;
        A_Track.clip = A_Track_Normal;
        B_Track.clip = B_Track_Normal;
        CarRB = GetComponentInParent<Rigidbody>();
        driftActive = false;
        // swapLock = true;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        currentSpeedRatio = Vector3.Dot(transform.right, CarRB.linearVelocity) / maxPlayerSpeed;
        if( Mathf.Abs(currentSpeedRatio) < 0.1f)
        {
            if (B_Track.isPlaying)
            {
                A_Track.Stop();
                B_Track.Stop();
                
            }
            
            A_Track.volume = 1f;
            B_Track.volume = 0f;
            // if (!swapLock)//Silly Code
            // {
            //     diceRoll = Random.Range(0f,1f);
            //     if(diceRoll < 0.05)
            //     {
            //         print("Swapped to silly track");
            //         // A_Track.clip = A_Track_Silly;
            //         // B_Track.clip = B_Track_Silly;
            //     }
            //     else
            //     {
            //         print("Swapped to Normal track");
            //         // A_Track.clip = A_Track_Normal;
            //         // B_Track.clip = B_Track_Normal;
            //     }
            //     swapLock = true;
            // }
        }
        else
        {
            if (drifting)
            {
                if (!driftActive)
                {
                    B_Track.Stop();
                    B_Track.clip = B_Track_Drift;
                    B_Track.Play();
                    driftActive = true;
                }
            }
            else
            {
                if (driftActive)
                {
                    B_Track.Stop();
                    B_Track.clip = B_Track_Normal;
                    B_Track.Play();
                    driftActive = false;
                }
            }

            if (!B_Track.isPlaying)
            {
                A_Track.Play();
                B_Track.Play();
            }

            // if(currentSpeedRatio > 0.75f)
            // {
            //     swapLock = false;
            // }

            A_Track.volume = engineShift.Evaluate(currentSpeedRatio) * (throttle + 0.1f);
            B_Track.volume = engineShift.Evaluate(1-currentSpeedRatio) * (throttle + 0.1f);
            B_Track.pitch = 1f + currentSpeedRatio/5f;

        }
    }
}
