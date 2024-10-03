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
