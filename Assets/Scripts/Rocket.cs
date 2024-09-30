using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Transform _planeTransform;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _planeTransform = FindObjectOfType<PlaneMoves>().transform;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_planeTransform)
        {
            Vector3 direction = _planeTransform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction, transform.right);
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.AddRelativeForce(Vector3.forward * _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlaneMoves planeMoves;
        if (other.gameObject.TryGetComponent(out planeMoves))
        {
            planeMoves.Health = 0;
            Destroy(gameObject);
        }
    }
}
