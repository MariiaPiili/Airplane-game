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
