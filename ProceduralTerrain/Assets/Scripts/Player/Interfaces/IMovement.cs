using UnityEngine;

public interface IMovement
{
    float MoveSpeed { get; set; }

    void Move(Vector3 direction);
}