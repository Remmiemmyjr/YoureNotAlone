//*************************************************
// Project: We're Tethered Together
// File: InstructionsManager.cs
// Author/s: Corbyn LaMar
//
// Desc: Change controls displayed on instructions
//       depending on player input
//
// Notes:
// -
//
// Last Edit: 8/03/2023
//
//*************************************************

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


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    private void Start()
    {
        // Add input callback
        InputSystem.onActionChange += InputActionChangeCallback;

        // Check if any gamepads are connected
        var controllers = Input.GetJoystickNames();
        if (controllers.Length != 0)
            keyboardMouseUsed = false;

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


    ////////////////////////////////////////////////////////////////////////
    // INPUT ACTION CHANGE =================================================
    // Detect if input has been changed from one type to another, then
    // update instruction images accordingly
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
            //Debug.Log("Instructions text have not been set");
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
