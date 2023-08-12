using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoorProfile : MonoBehaviour
{
    public GameObject gem;
    public GameObject gemLight;


    public void DisableGem(Color c)
    {
        gemLight.SetActive(false);
        gem.GetComponent<SpriteRenderer>().color = c;
    }    
    

    public void EnableGem(Color c)
    {
        gemLight.SetActive(true);
        gem.GetComponent<SpriteRenderer>().color = c;
        gemLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().color = c;
    }
}
