using UnityEngine;

public interface IPlayerInput
{
    Vector2 GetMovementInput();
    bool GetJumpInput();
}
