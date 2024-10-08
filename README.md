# Airplane game

 A simple game created in Unity, where the player controls an airplane, collecting coins and avoiding collisions with mines and rockets.
 
![game](https://github.com/MariiaPiili/Airplane-game/blob/main/Airplane%20game.png)
## Plane's movement сontrol
The plane's movement is controlled by `PlaneMoves.cs`. It uses variables for speed, lift force, and rotation speed to provide forward movement and maneuverability.  
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

    private Rigidbody _rigidbody;
    private float _horizon;
    private float _vertical;
    
    public float Speed => _speed;    

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();        
    }

    private void Update()
    {
        _horizon = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");        
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

`PlaneHealth.cs` is responsible for managing the airplane's health in the game. It has fields for maximum health (_maxHealth), a display object for when the airplane is destroyed (_textDead), and the current health value (_health). Upon starting, the airplane's health is set to the maximum value. In the `Update` method, the class checks the health; if it reaches zero, the airplane is destroyed. 

```csharp
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
```

## Creating collision objects

`ElementForSpawn.cs` is designed to store information about objects that will be spawned. It contains public field for the object that will be spawned (GameObject) and public field for the number of instances of this object to spawn (AmountFoSpawn). The class is Serializable, so it is easy to configure in Unity's Inspector. 

```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ElementForSpawn 
{
    public GameObject GameObject;
    public int AmountFoSpawn;
}
```

`Spawner.cs` handles the spawning of objects in the game. It contains a serialized list, _elementForSpawn, which stores the different ElementForSpawn objects to be spawned. Additionally, it defines spawn position ranges with _minPositionX, _minPositionY, _maxPositionX, _maxPositionY, and _positionZ, specifying the coordinates within which the objects can appear.

In the `Awake` method `Spawner.cs` iterates through each element in _elementForSpawn and calls the `Spawn` method. The `Spawn` method instantiates the specified number of instances (AmountFoSpawn) of each GameObject at random positions within the defined range. 

```csharp
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<ElementForSpawn> _elementForSpawn;

    [Header("Spawn Position Range")]
    [SerializeField] private float _minPositionX;
    [SerializeField] private float _minPositionY;
    [SerializeField] private float _maxPositionX;
    [SerializeField] private float _maxPositionY;
    [SerializeField] private float _positionZ;

    private void Awake()
    {
        foreach (var element in _elementForSpawn)
        {
            Spawn(element.GameObject, element.AmountFoSpawn);
        }
    }

    private void Spawn(GameObject gameObject, int count)
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

`Coin.cs` handles the behavior of a coin. In the `Update` method, the coin rotates around its vertical axis at a specified speed, _speedRot. In the `OnTriggerEnter` method, it checks if the object colliding with the coin has the PlaneMoves component. If so, it calls the `CoinCollected` method from CoinManager to increment the collected coin count, and then destroys the coin.

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
            CoinManager.CoinCollected();            
            Destroy(gameObject);
        }
    }
}
```

`CoinManager.cs` tracks the number of collected coins and manages the display of a win message. In the `Start` method, it calculates the total number of coins in the scene at the beginning of the game. In the `Update` method, it checks if all coins have been collected; if they have, it activates the _objectForWinText to display a win message. The `CoinCollected` method increments the _collectedCoins counter each time a coin is collected by the player.

```csharp
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
```

### Mine

`Mine.cs` manages the behavior of a mine. When an object enters its trigger collider (OnTriggerEnter), it checks if the colliding object has a PlaneHealth component. If so, it decreases the airplane's health by one and then destroys itself.

```csharp
using UnityEngine;

public class Mine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlaneHealth planeHealth;
        if (other.gameObject.TryGetComponent(out planeHealth))
        {
            planeHealth.Health--;            
            Destroy(gameObject);
        }
    }
}
```

### Rocket

`Rocket.cs` controls a rocket that targets the player’s airplane. In the `Awake` method, it finds and stores the player's plane's transform. In the `Start` method, it retrieves the rocket's Rigidbody component. In `Update` it adjusts its rotation to face the plane. In `FixedUpdate` it applies a forward force to move the rocket towards the plane at a speed specified by _speed. When it collides with an object with a PlaneHealth component, it sets the airplane’s health to zero and destroys itself.

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
        PlaneHealth planeHealth;
        if (other.gameObject.TryGetComponent(out planeHealth))
        {
            planeHealth.Health = 0;
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
