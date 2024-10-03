using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{   
    [SerializeField] private GameObject _objectForWinText;
    [SerializeField] private Transform _planeTransform;
    
    private int _numberOfCoinsStart;   
    private static int _collectedCoins = 0;

    private void Start()
    {
        _numberOfCoinsStart = FindObjectsOfType<Coin>().Length;
    }

    private void Update()
    {
        if (_collectedCoins == _numberOfCoinsStart)
        {
            _objectForWinText.SetActive(true);
        }
    }

    public static void CoinCollected()
    {
        _collectedCoins++;
    }
}
