using UnityEngine;

public class ExplodeOnDeath : MonoBehaviour
{
    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
        if (_health != null)
        {
            _health.OnDeath += OnDeathHandler;
        }
    }

    private void OnDeathHandler()
    {
        Debug.Log($"{gameObject.name} triggered OnDeath event!");
        
        //play breaking animation
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (_health != null)
        {
            _health.OnDeath -= OnDeathHandler;
        }
    }
}