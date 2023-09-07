//*************************************************
// Project: Build-A-Boss
// File: DebugMonoBehavior.cs
// Author/s: Emmy Berg
//
// Desc: Custom class to add the debugging system
//       to objects per script. Debug logging only
//       happens when it is enabled on the object.
//
// Last Edit: 12/18/22
//
//*************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;


public class DebugMonoBehavior : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    public bool DEBUGGING;

    Dictionary<string, object> lastVals = new Dictionary<string, object>();
    // *********************************************************************



    ////////////////////////////////////////////////////////////////////////
    // LOGGER METHODS ======================================================
    public void LogValue(string debugText)
    {
        if (DEBUGGING)
        {
            Debug.Log(debugText);
        }
    }


    public void LogValueWhenUpdated(string variable, object val)
    {
        if (DEBUGGING)
        {
            if (lastVals.TryGetValue(variable, out object oldVal))
            {
                if (val != null && val.Equals(oldVal) || val == null && oldVal == null)
                {
                    return;
                }

            }
            Debug.Log($"{variable}: {val}");
            lastVals[variable] = val;
        }
    }
    // *********************************************************************
}

