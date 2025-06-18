using UnityEngine;

public class ControllerInput : MonoBehaviour, IPlayerInput
{
    public Vector2 GetMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        return new Vector2(horizontal, vertical);
    }

    public bool GetAttackInput()
    {
        throw new System.NotImplementedException();
    }

    public bool GetTargerLockInput()
    {
        throw new System.NotImplementedException();
    }

    public bool GetDashInput()
    {
        throw new System.NotImplementedException();
    }

    public int GetTargetSwitchInput()
    {
        throw new System.NotImplementedException();
    }
}
