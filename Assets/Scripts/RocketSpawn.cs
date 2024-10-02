using UnityEngine;

public class RocketSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _rocketprefab;
    [SerializeField] private int _rocketAmount;

    private void Start()
    {
        Spawner.Instance.Spawn(_rocketprefab, _rocketAmount);
    }
}
