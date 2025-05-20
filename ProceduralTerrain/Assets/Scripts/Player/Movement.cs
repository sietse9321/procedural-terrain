using UnityEngine;

public class Movement : MonoBehaviour, IMovement
{
    [SerializeField] private float moveSpeed = 5f;

    public void Move(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0)
        {
            Vector3 movement = direction.normalized * moveSpeed * Time.deltaTime;
            transform.Translate(movement, Space.World);
        }
    }
}