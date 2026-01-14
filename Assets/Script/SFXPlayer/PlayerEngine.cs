using UnityEngine;
using DG.Tweening;

public class PlayerEngine : MonoBehaviour
{
    [Header("Configurations")]
    public AnimationCurve engineShift;
    public float maxPlayerSpeed;

    [Header("References")]
    public Car _car;
    public Rigidbody CarRB;
    public AudioClip A_Track_Normal;
    public AudioClip B_Track_Normal;
    public AudioClip C_Track_Drift;
    // public AudioClip A_Track_Silly;
    // public AudioClip B_Track_Silly;
    public AudioSource A_Track;
    public AudioSource B_Track;
    public AudioSource C_Track;


    //Interior Variables
    [SerializeField]
    // private bool swapLock;
    private float currentSpeedRatio;
    // private float diceRoll;
    private InputSystem_Actions controller;
    private float throttle;
    private bool drifting;
    private float brake;



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
        controller.Car.Brake.performed += brakeCTX => UpdateBrakeInput(brakeCTX.ReadValue<float>());
        controller.Car.Brake.canceled += _ => UpdateBrakeInput(0f);

    }


    public void UpdateThrottle(float inputThrottle)
    {
        throttle = inputThrottle * 0.5f;
    }

    public void UpdateDriftInput(bool isDrifting)
    {
        drifting = isDrifting;
    }

    public void UpdateBrakeInput(float brakeInput)
    {
        brake = brakeInput;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        A_Track.loop = false;
        B_Track.loop = true;
        C_Track.loop = true;
        A_Track.volume = 1f;
        B_Track.volume = 0f;
        C_Track.volume = 1f;
        A_Track.clip = A_Track_Normal;
        B_Track.clip = B_Track_Normal;
        C_Track.clip = C_Track_Drift;
        CarRB = GetComponentInParent<Rigidbody>();
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        currentSpeedRatio = Vector3.Dot(transform.right, CarRB.linearVelocity) / maxPlayerSpeed;
        if (Mathf.Abs(currentSpeedRatio) < 0.1f)
        {
            if (B_Track.isPlaying)
            {
                A_Track.Stop();
                B_Track.Stop();
                C_Track.Stop();

            }

            // A_Track.volume = 1f;
            // if(currentSpeedRatio < -0.1f && brake > 0.25f)
            // {
            //     if (!A_Track.isPlaying)
            //     {
            //         A_Track.Play();
            //     }
            //     // B_Track.volume = engineShift.Evaluate(1 - Mathf.Abs(currentSpeedRatio)) *0.75f;
            // }
            // else
            // {
            //     if (A_Track.isPlaying)
            //     {
            //         A_Track.Stop();
            //     }
            // }
            

            

        }
        else
        {
            if (_car.CheckAirborne())
            {
                if (!drifting)
                {
                    if(C_Track.isPlaying)
                        C_Track.DOFade(0f,0.25f).OnComplete(() => C_Track.Stop());
                }
                else
                {
                    if (!C_Track.isPlaying)
                    {
                        C_Track.volume = 1f;
                        C_Track.Play();
                    }
                        
                        
                }
            }
            else
            {
                if(C_Track.isPlaying)
                        C_Track.DOFade(0f,0.25f).OnComplete(() => C_Track.Stop());
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

            if(currentSpeedRatio < -0.1f && brake > 0.25f)
            {
                A_Track.volume = 0.75f;
            }
            else
            {
                A_Track.volume = engineShift.Evaluate(currentSpeedRatio) * (throttle + 0.5f);
            }
            B_Track.volume = engineShift.Evaluate(1 - currentSpeedRatio) * (throttle + 0.5f);
            B_Track.pitch = 1f + currentSpeedRatio / 5f;
            C_Track.pitch = 1f + currentSpeedRatio / 8f;

        }
    }
}
