using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;

    [Header("Spawn Position Range")]
    [SerializeField] private float _minPositionX;
    [SerializeField] private float _minPositionY;
    [SerializeField] private float _maxPositionX;
    [SerializeField] private float _maxPositionY;
    [SerializeField] private float _positionZ;

    private void Awake()
    {
        if (Spawner.Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Spawn(GameObject gameObject, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randonSpawnPosition = new Vector3(Random.Range(_minPositionX, _maxPositionX), Random.Range(_minPositionY, _maxPositionY), _positionZ);
            GameObject newGameObject = Instantiate(gameObject, randonSpawnPosition, Quaternion.identity);            
        }
    }
}
