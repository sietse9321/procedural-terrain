using UnityEngine;

public class Movement : MonoBehaviour, IMovement
{
    public float MoveSpeed { get; set; }

    public void Move(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0)
        {
            Vector3 movement = direction.normalized * MoveSpeed * Time.deltaTime;
            transform.Translate(movement, Space.World);
        }
    }
}