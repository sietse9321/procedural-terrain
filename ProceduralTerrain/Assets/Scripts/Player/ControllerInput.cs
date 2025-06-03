using UnityEngine;

public class ControllerInput : MonoBehaviour, IPlayerInput
{
    public Vector2 GetMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        return new Vector2(horizontal, vertical);
    }

    public bool GetTargerLockInput()
    {
        throw new System.NotImplementedException();
    }

    public int GetTargetSwitchInput()
    {
        throw new System.NotImplementedException();
    }
}
