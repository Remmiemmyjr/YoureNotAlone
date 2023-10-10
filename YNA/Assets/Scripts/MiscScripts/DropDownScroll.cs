using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropDownScroll : MonoBehaviour, ISelectHandler
{
    private ScrollRect scrollRect;
    private float scrollPos = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        scrollRect = GetComponentInParent<ScrollRect>(true);

        int childCount = scrollRect.content.transform.childCount - 1;
        int childIndex = transform.GetSiblingIndex();

        childIndex = childIndex < ((float)childCount / 2.0f) ? childIndex - 1 : childIndex;

        scrollPos = 1 - ((float)childIndex / childCount);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (scrollRect)
            scrollRect.verticalScrollbar.value = scrollPos;
    }
}
