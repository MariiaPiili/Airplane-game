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
