using UnityEngine;

public class CoinSpawn : MonoBehaviour
{
    public static int _collectedCoins;

    [SerializeField] private GameObject _goldCoin;
    [SerializeField] private GameObject _objectForWinText;
    [SerializeField] private Transform _planeTransform;
    [SerializeField] private int _numberOfCoinsStart;

    private void Start()
    {
        Spawner.Instance.Spawn(_goldCoin, _numberOfCoinsStart);
    }

    private void Update()
    {
        if (_collectedCoins == _numberOfCoinsStart)
        {
            _objectForWinText.SetActive(true);
        }
    }
}
