using UnityEngine;

public class DropItem : MonoBehaviour, IHittable
{
    public void OnHit()
    {
        if (TryGetComponent(out IHealth health) && !health.IsAlive())
        {
            Debug.Log("drop item");
        }
        else
        {
            Debug.Log("drop item");
        }
    }
}