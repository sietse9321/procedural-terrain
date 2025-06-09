using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{ 
    private void OnTriggerEnter(Collider other)
    {
        // Try to get a component that implements IHealth
        IHealth targetHealth = other.GetComponent<IHealth>();
        
        targetHealth?.TakeDamage(10);
    }

}