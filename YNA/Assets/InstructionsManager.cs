using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InstructionsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject keyboardText;
    [SerializeField]
    private GameObject controllerText;

    private bool keyboardMouseUsed = true;

    private void Start()
    {
        // Add input callback
        InputSystem.onActionChange += InputActionChangeCallback;

        // Check if any gamepads are connected
        var controllers = Input.GetJoystickNames();
        if (controllers.Length != 0)
            keyboardMouseUsed = false;

        // Ensure they exist
        if (!keyboardText || !controllerText)
        {
            Debug.Log("Instructions text have not been set");
            return;
        }

        // Update instructions accordingly
        if (keyboardMouseUsed)
        {
            keyboardText.SetActive(true);
            controllerText.SetActive(false);
        }
        else
        {
            keyboardText.SetActive(false);
            controllerText.SetActive(true);
        }
    }

    private void InputActionChangeCallback(object obj, InputActionChange change)
    {
        // Detect Changes
        if (change == InputActionChange.ActionPerformed)
        {
            InputAction receivedInputAction = (InputAction)obj;
            InputDevice lastDevice = receivedInputAction.activeControl.device;

            keyboardMouseUsed = lastDevice.name.Equals("Keyboard") || lastDevice.name.Equals("Mouse");
        }

        // Ensure they exist
        if(!keyboardText || !controllerText)
        {
            Debug.Log("Instructions text have not been set");
            return;
        }

        // Update Instructions
        if (keyboardMouseUsed)
        {
            keyboardText.SetActive(true);
            controllerText.SetActive(false);
        }
        else
        {
            keyboardText.SetActive(false);
            controllerText.SetActive(true);
        }
    }

}
