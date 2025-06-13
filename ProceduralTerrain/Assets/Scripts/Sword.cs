using UnityEngine;

public class Sword : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IHittable[] hittables = other.GetComponents<IHittable>();
        foreach (var hittable in hittables)
        {
            hittable.OnHit();
        }
        
        if (other.TryGetComponent(out IHealth health))
        {
            health.TakeDamage(10);
        }
    }
}