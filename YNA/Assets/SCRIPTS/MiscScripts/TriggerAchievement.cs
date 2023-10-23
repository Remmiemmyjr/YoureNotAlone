using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerAchievement : MonoBehaviour
{
    bool triggered = false; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && triggered == false)
        {
            GetComponent<SteamForceAwardAchievement>().AwardAchievement();
            triggered = true;
        }
    }
}
