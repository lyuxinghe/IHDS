using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is purely used for debug

public class PrintMessage : MonoBehaviour
{
    public void PrintActivateMessage(){
        print ("Activated");
    }

    public void PrintDeactivateMessage(){
        print ("Deactivated");
    }

    public void PrintInProximity(){
        print("In Proximity");
    }

    public void PrintOutProximity()
    {
        print("Out Proximity");
    }
}
