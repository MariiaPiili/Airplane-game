using UnityEngine;

public class PlaneMoves : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _supportForce;
    [SerializeField] private float _speedRotate;      

    private Rigidbody _rigidbody;
    private float _horizon;
    private float _vertical;
    
    public float Speed => _speed;    

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();        
    }

    private void Update()
    {
        _horizon = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");        
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(Vector3.up * _supportForce);
        _rigidbody.AddRelativeForce(Vector3.forward * _speed);

        if (_horizon > 0)
        {
            _rigidbody.AddRelativeForce(Vector3.forward * _horizon * _speed);
        }
        _rigidbody.AddRelativeTorque(Vector3.left * _vertical * _speedRotate);
    }
}
