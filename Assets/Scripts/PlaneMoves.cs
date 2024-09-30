using UnityEngine;

public class PlaneMoves : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _supportForce;
    [SerializeField] private float _speedRotate;
    [SerializeField] private GameObject _textDead;
    [SerializeField] private int _maxHealth;

    private Rigidbody _rigidbody;
    private float _horizon;
    private float _vertical;
    private int _health;

    public float Speed => _speed;

    public float Health { get; set; }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _health = _maxHealth;
    }

    private void Update()
    {
        _horizon = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        if (_health == 0)
        {
            _textDead.SetActive(true);
            Destroy(gameObject);
        }
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
