using UnityEngine;

public class PlaneHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private GameObject _textDead;

    private int _health;

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
        }
    }

    private void Start()
    {
        _health = _maxHealth;
    }

    private void Update()
    {
        if (_health == 0)
        {
            _textDead.SetActive(true);
            Destroy(gameObject);
        }
    }
}
