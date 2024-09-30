using UnityEngine;

public class CoinSpawn : MonoBehaviour
{
    public static int _collectedCoins;

    [SerializeField] private GameObject _goldCoin;
    [SerializeField] private GameObject _objectForWinText;
    [SerializeField] private Transform _planeTransform;
    [SerializeField] private float _numberOfCoinsStart;
    [Header("Spawn Position Range")]
    [SerializeField] private float _maxPositionX;
    [SerializeField] private float _minPositionX;
    [SerializeField] private float _maxPositionY;
    [SerializeField] private float _minPositionY;
    [SerializeField] private float _positionZ;

    private float _count;

    private void Start()
    {        
        while (_numberOfCoinsStart > _count)
        {
            Vector3 randonSpawnPosition = new Vector3(Random.Range(_minPositionX, _maxPositionX), Random.Range(_minPositionY, _maxPositionY), _positionZ);
            GameObject coin = Instantiate(_goldCoin, randonSpawnPosition, Quaternion.identity);            
            _count++;
        }
    }

    private void Update()
    {
        if (_collectedCoins == _count)
        {
            _objectForWinText.SetActive(true);
        }
    }
}
