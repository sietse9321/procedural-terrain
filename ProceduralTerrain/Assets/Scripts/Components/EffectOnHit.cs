using UnityEngine;

public class EffectOnHit : MonoBehaviour, IHittable
{
    private const string EffectPath = "Effects/HitEffect";

    public void OnHit()
    {
        ParticleSystem effectPrefab = Resources.Load<ParticleSystem>(EffectPath);
        if (effectPrefab == null)
        {
            Debug.LogWarning($"Particle system at Resources/{EffectPath}.prefab not found!");
            return;
        }

        ParticleSystem effectInstance = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        
        effectInstance.Play();

        Destroy(effectInstance.gameObject, effectInstance.main.duration + effectInstance.main.startLifetime.constantMax);
    }
}