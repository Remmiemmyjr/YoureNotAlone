using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;




public class CameraFX : MonoBehaviour
{


    public int testnubmb = 0;

    public enum FX
    {
        ChromaticAbberation,
        Vignette
    }

    [SerializeField]
    public List<FX> EffectsList = new List<FX>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void test()
    {

    }
}
