using UnityEngine;

public class KeySpin : MonoBehaviour
{
    [Header("Spin Speed and Limit")]
    [SerializeField] private AnimationCurve _setSpeed;
    [SerializeField] private float _maxSpeed;


    private Vector3 _rotation;
    [Header("References")]
    public Car car;
    [SerializeField] private  Animator _anim;
    private float _topSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rotation  = new Vector3(transform.rotation.x,transform.rotation.y, transform.rotation.z);
        _topSpeed = car.topSpeed;
        _anim = gameObject.GetComponent<Animator>();
        _anim.speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        float percent = car.carSpeed / _topSpeed;
        float speed = _maxSpeed * _setSpeed.Evaluate(percent);
        _anim.speed = speed;

    }
}
