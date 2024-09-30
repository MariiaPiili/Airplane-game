using UnityEngine;

public class MineSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _mine;
    [SerializeField] private float _numberOfMineStart;
    [Header("Spawn Position Range")]
    [SerializeField] private float _minPositionX;
    [SerializeField] private float _minPositionY;
    [SerializeField] private float _maxPositionX;
    [SerializeField] private float _maxPositionY;
    [SerializeField] private float _positionZ;

    private float _count;

    private void Start()
    {
        while (_numberOfMineStart > _count)
        {
            Vector3 randonSpawnPosition = new Vector3(Random.Range(_minPositionX, _maxPositionX), Random.Range(_minPositionY, _maxPositionY), _positionZ);
            GameObject mine = Instantiate(_mine, randonSpawnPosition, Quaternion.identity);
            _count++;
        }
    }
}
