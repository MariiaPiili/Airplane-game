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
