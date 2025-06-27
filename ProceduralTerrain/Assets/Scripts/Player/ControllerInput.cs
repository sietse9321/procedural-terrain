using UnityEngine;

public class ControllerInput : MonoBehaviour, IPlayerInput
{
    public Vector2 GetMovementInput()
    {
        float horizontal = Input.GetAxisRaw("HorizontalG");
        float vertical = Input.GetAxisRaw("VerticalG");
        
        return new Vector2(horizontal, vertical);
    }

    public bool GetAttackInput()
    {
        return Input.GetKeyDown(KeyCode.JoystickButton0);
    }

    public bool GetTargerLockInput()
    {
        return Input.GetKeyDown(KeyCode.JoystickButton9);
    }

    public bool GetDashInput()
    {
        return Input.GetKeyDown(KeyCode.JoystickButton5);
    }

    public int GetTargetSwitchInput()
    {
        float dpadHorizontal = Input.GetAxisRaw("Axis6");
        float rightStickHorizontal = Input.GetAxisRaw("Axis4");
        
        if (dpadHorizontal < -0.5f)
            return -1;
        if (dpadHorizontal > 0.5f)
            return 1;
        
        if (rightStickHorizontal < -0.5f)
            return -1;
        if (rightStickHorizontal > 0.5f)
            return 1;
        
        return 0;
    }
}
