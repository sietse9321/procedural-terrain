using UnityEngine;

public class KeyboardInput : MonoBehaviour, IPlayerInput
{
    public Vector2 GetMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        return new Vector2(horizontal, vertical);
    }

    public bool GetTargerLockInput()
    {
        return Input.GetMouseButtonDown(2);
    }

    public int GetTargetSwitchInput()
    {
        if (Input.mouseScrollDelta.y != 0f)
        {
            return (int)Mathf.Sign(Input.mouseScrollDelta.y);
        }

        return 0;
    }
}