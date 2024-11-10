using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerRumble : MonoBehaviour
{
    public static IEnumerator ControllerRumbleFX(float low, float high, float time)
    {
        if (!Info.isPaused)
        {
            Gamepad.current.SetMotorSpeeds(low, high);
            yield return new WaitForSeconds(time);

            InputSystem.ResetHaptics();
        }
        else
        {
            InputSystem.PauseHaptics();
        }
    }
    

    public static IEnumerator EyeWakingUpRumble(float low, float high, float time)
    {
        if (!Info.isPaused)
        {
            int rumbleCount = 4; // Number of times to play the rumble effect
            float pauseTime = 0.65f; // Half a second pause between rumbles

            for (int i = 0; i < rumbleCount; i++)
            {
                // Start the rumble
                Gamepad.current.SetMotorSpeeds(low, high);
                yield return new WaitForSeconds(time);

                // Stop the rumble
                InputSystem.ResetHaptics();

                // Wait for half a second before the next rumble, if not the last rumble
                if (i < rumbleCount)
                {
                    yield return new WaitForSeconds(pauseTime);
                }
            }
        }
        else
        {
            InputSystem.ResetHaptics();
            InputSystem.PauseHaptics();
        }
    }
}
