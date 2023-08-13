using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightedFX : Selectable
{
    // Update is called once per frame
    void Update()
    {
        if (IsHighlighted() == true)
        {
            //Output that the GameObject was highlighted, or do something else
            Debug.Log("Selectable is Highlighted");
        }
    }
}
