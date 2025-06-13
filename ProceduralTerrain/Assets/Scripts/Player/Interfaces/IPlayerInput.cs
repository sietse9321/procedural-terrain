using UnityEngine;

public interface IPlayerInput
{
    Vector2 GetMovementInput();

    bool GetAttackInput();
    
    bool GetTargerLockInput();
    
    int GetTargetSwitchInput();
}