using UnityEngine;

public interface IPlayerInput
{
    Vector2 GetMovementInput();
    
    bool GetTargerLockInput();
    
    int GetTargetSwitchInput();
}
 