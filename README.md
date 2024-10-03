# Airplane game

 A simple game created in Unity, where the player controls an airplane, collecting coins and avoiding collisions with mines and rockets.
 
![game](https://github.com/MariiaPiili/Airplane-game/blob/main/Airplane%20game.png)
## Plane's movement сontrol
The plane's movement is controlled by `PlaneMoves.cs`. It uses variables for speed, lift force, and rotation speed to provide forward movement and maneuverability. If the plane's health reaches zero, a death message appears on the screen and the plane is destroyed. 
- Move Upward: W 
- Move Downward: S
- Move Left: A
- Move Rigth: D
```csharp
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
```

## Creating collision objects

`Spawner.cs` is responsible for spawning objects in random positions within a specified range in the game. It allows the creation of multiple instances of a given object at random coordinates, with configurable boundaries for X and Y positions. It also implements the Singleton pattern, ensuring only one instance of this class exists during the game. 

`Spawn()` method takes two parameters: a GameObject to spawn and an integer count, which specifies how many instances of the object to create. Inside the method, a loop runs based on the count, and for each iteration, a random position is generated using `Random.Range()` for X and Y values. A new instance of the provided GameObject is then instantiated at that position with no rotation.
```csharp
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
```
The airplane in the game can collide with coins (to collect them), mines or rockets.

### Coins

`Coin.cs` handles the behavior of the coin collectible in the game. It rotates the coin continuously and detects collisions with the player’s plane. The coin rotates around the Y-axis. 

When the plane collides with the coin, the coin is "collected." The total count of collected coins is increased, and the coin is destroyed, simulating the collection.
```csharp
using UnityEngine;

public class Coin : MonoBehaviour
{   
    [SerializeField] private float _speedRot;
        
    private void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * _speedRot, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlaneMoves>() != null)
        {
            CoinSpawn._collectedCoins++;            
            Destroy(gameObject);
        }
    }
}
```
`CoinSpawn.cs` is responsible for spawning coins at the start of the game and checking if the player has collected all of them.
```csharp
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
```

### Mine

`MineSpawn.cs` is responsible for spawning a set number of mines at the start of the game. In the `Start()` method, it uses the `Spawner.cs` to create the specified number of mines.
```csharp
using UnityEngine;

public class MineSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _mine;
    [SerializeField] private int _numberOfMineStart;    

    private void Start()
    {
        Spawner.Instance.Spawn(_mine, _numberOfMineStart);
    }
}
```
`Mine.cs` handles what happens when a mine collides with the player’s airplane. In `OnTriggerEnter()` method it detects if the object is the player’s plane, decreases the plane’s health by 1, and then destroys the mine.
```csharp
using UnityEngine;

public class Mine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlaneMoves planeMoves;
        if (other.gameObject.TryGetComponent(out planeMoves))
        {
            planeMoves.Health--;
            Debug.Log(planeMoves.Health + " Health");
            Destroy(gameObject);
        }
    }
}
```

### Rocket

`RocketSpawn.cs` spawns a specified number of rockets at the start of the game. It uses `Spawner.cs` to generate the rockets.
```csharp
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
```
`Rocket.cs` controls the behavior of the rockets. It tracks the player's plane and rotates to face it, then moves toward the plane at a constant speed. If the rocket collides with the plane, the plane's health is set to zero, and the rocket is destroyed.
```csharp
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
```

## Camera 

`CameraFollow.cs` controls the movement of the camera, which constantly follows the airplane. Every frame the camera updates its position in space so that its X and Y coordinates match the plane’s coordinates, while the Z coordinate remains fixed, maintaining a constant distance between the camera and the airplane.
````csharp
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject _plane;

    private void Update()
    {
        if (_plane != null)
        {
            Vector3 position = new Vector3(_plane.transform.position.x, _plane.transform.position.y, transform.position.z);
            transform.position = position;
        }
    }
}
````

## Plane propeller

`Propeller.cs` is responsible for the rotation of the airplane’s propeller. It uses the plane’s movement speed, obtained from the `PlaneMoves.cs`, and multiplies it by the propeller's rotation speed to create a visual effect of spinning.
```csharp
using UnityEngine;

public class Propeller : MonoBehaviour
{
    [SerializeField] private float _speedRot;
    [SerializeField] private PlaneMoves _planeMoves;
    
    private void Update()
    {
        transform.Rotate(Vector3.up * _speedRot * _planeMoves.Speed * Time.deltaTime, Space.Self);
    }
}
```

## Requirements

Unity Game Engine
