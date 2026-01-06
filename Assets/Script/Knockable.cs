using UnityEngine;

public class Knockable : MonoBehaviour
{
    [Header("Parameters")]
    public float _radius = 5.0f;
    public float _power = 10.0f;
    public float _upwardsModifier = 3.0f;
    public ForceMode _forceMode = ForceMode.Force;
    [Tooltip ("How strong player speed increases imapact")]
    public float _baseSpeedPower = 0.7f;
    public bool _isKnocked = false;
    [SerializeField] private float _destroyTime = 10.0f;

    [Header("References")]
    [SerializeField] private Rigidbody _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_rb == null)
        {
            _rb = this.gameObject.GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.TryGetComponent<Car>(out Car car))
            {
                float percent = _baseSpeedPower + (car.carSpeed / car.topSpeed);
                _rb.AddExplosionForce(Mathf.Pow(_power, percent), transform.position, _radius, Mathf.Pow(_upwardsModifier, percent), _forceMode);
                _isKnocked = true;
                Destroy(this.gameObject, _destroyTime);
            }
        }
    }
}