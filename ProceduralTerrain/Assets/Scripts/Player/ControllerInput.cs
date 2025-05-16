using UnityEngine;

public class ControllerInput : MonoBehaviour, IPlayerInput
{
    public Vector2 GetMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        return new Vector2(horizontal, vertical);
    }

    public bool GetJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }
}
