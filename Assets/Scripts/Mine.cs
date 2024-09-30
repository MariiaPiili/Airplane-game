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
