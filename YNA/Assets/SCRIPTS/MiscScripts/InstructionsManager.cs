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
using UnityEngine.XR;

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
        InputSystem.onDeviceChange += InputConnectionChangeCallback;

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
    private void InputConnectionChangeCallback(object obj, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                // New Device.
                keyboardMouseUsed = false;
                break;
            case InputDeviceChange.Disconnected:
                // Device got unplugged.
                keyboardMouseUsed = true;
                break;
            case InputDeviceChange.Reconnected:
                // Device Reconnected
                keyboardMouseUsed = false;
                break;
            case InputDeviceChange.Removed:
                // Remove from Input System entirely; by default, Devices stay in the system once discovered.
                keyboardMouseUsed = true;
                break;

            default:
                // See InputDeviceChange reference for other event types.
                break;
        }

        // Ensure they exist
        if (!keyboardText || !controllerText)
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
