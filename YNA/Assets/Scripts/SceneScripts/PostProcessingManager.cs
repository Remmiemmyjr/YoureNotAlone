using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class PostProcessingManager : MonoBehaviour
{
    [SerializeField]
    private GameObject GlobalVolume;

    public SerializableDictionaryBase<string, CameraFX> FX_List;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Runs the respective camera fx function
    public void InvokeCameraFX(string eEvent)
    {
        CameraFX fx;

        //if the parameter is equal to a key
        if (FX_List.TryGetValue(eEvent, out fx))
        {
            fx.Activate();
        }
        //print an error message
        else
        {
            Debug.LogError("POST PROCESSING MANAGER (" + gameObject.name + "): Invalid Event Parameter or Invalid Key in FX_List");
        }
        
    }

    public Volume GetGlobalVolume()
    {
        return GlobalVolume.GetComponent<Volume>();
    }

}
