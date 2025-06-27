using UnityEngine;

public class InputSwitcher : MonoBehaviour
{
    public KeyboardInput keyboardInput;
    public ControllerInput controllerInput;

    public IPlayerInput ActiveInput { get; private set; }

    private string lastUsedInput = "Keyboard";

    void Awake()
    {
        if (keyboardInput == null)
            keyboardInput = GetComponent<KeyboardInput>();
        if (controllerInput == null)
            controllerInput = GetComponent<ControllerInput>();

        SetActiveInput("Keyboard"); // Default
    }

    void Update()
    {
        // Detect gamepad movement input using HorizontalG/VerticalG
        if (Mathf.Abs(Input.GetAxisRaw("HorizontalG")) > 0.1f || Mathf.Abs(Input.GetAxisRaw("VerticalG")) > 0.1f)
        {
            if (lastUsedInput != "Controller")
                SetActiveInput("Controller");
        }
        // Detect keyboard movement input using Horizontal/Vertical OR mouse buttons/scroll
        else if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f
                || Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2) || Input.mouseScrollDelta.y != 0f)
        {
            if (lastUsedInput != "Keyboard")
                SetActiveInput("Keyboard");
        }
    }

    private void SetActiveInput(string inputType)
    {
        if (inputType == "Keyboard")
        {
            ActiveInput = keyboardInput;
            lastUsedInput = "Keyboard";
        }
        else if (inputType == "Controller")
        {
            ActiveInput = controllerInput;
            lastUsedInput = "Controller";
        }
    }
}