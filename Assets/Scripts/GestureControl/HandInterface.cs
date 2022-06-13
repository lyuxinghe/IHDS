using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;

public class HandInterface : MonoBehaviour
{

    /* constants/ macros defined here */
    public const float ZOOM_SCALE = 0.5f;
    public const float ZOOM_CAP = 0.0005f;

    public const float PITCH_SCALE = 15.0f;
    public const float PITCH_CAP = 0.008f;
    public const float PITCH_OFFSET = 0.006f;

    public const float YAW_CW = -3f;
    public const float YAW_CCW = 3f;

    public const float SNAP_ON_CAP = 0.012f;
    public const float SNAP_OFF_CAP = 0.06f;

    /* Object pointers for hand models */
    public HandModelBase leftHandModel = null;
    public HandModelBase rightHandModel = null;
    private Hand leftHand, rightHand = null;

    /* private variables used accross the class */
    private float prevPalmDist;
    private float prevWristTilt;
    private float prevLHX;
    private float prevRHX;
    private static bool setIsoActive = false;
    private static bool yawLeft = false;
    private static bool yawRight = false;
    private static bool isSnapping = false;
/*
    private Vector origLPalmPos;
    private Vector origRPalmPos;
    private Vector origLWristPos;
    private Vector origRWristPos;
    private Vector zeroVector = Vector::zero();
*/
    void Start()
    {
        prevPalmDist = 0;
        prevWristTilt = 0;
    }

    void Update(){
        if (leftHandModel != null && leftHandModel.IsTracked)
        {
            leftHand = leftHandModel.GetLeapHand();
        }
        else
        {
            leftHand = null;
        }

        if (rightHandModel != null && rightHandModel.IsTracked)
        {
            rightHand = rightHandModel.GetLeapHand();
        }
        else
        {
            rightHand = null;
        }

        if(setIsoActive)
        {
            setTFIsovalue();
        }
        else{
            // deactivate volume manipulation if snaping in process
            if(!isSnapping)
            {
                zooming();
                pitching();
                yawing();  
            }
            snapCheck();   
        }
    }


/* Private helper functions that updates in real-time */
    private void zooming()
    {
        float palmDist = 0;
        float zoomDelta = 0;
        VolumeInterface VI = FindObjectOfType<VolumeInterface>();

        if (leftHand != null && rightHand != null)
        {
            palmDist = leftHand.PalmPosition.DistanceTo(rightHand.PalmPosition);

            if(prevPalmDist == 0)
            {
                prevPalmDist = palmDist;
            }
            else if(prevPalmDist != palmDist)
            {
                zoomDelta = palmDist - prevPalmDist;
                prevPalmDist = palmDist;

                if(Mathf.Abs(zoomDelta) > ZOOM_CAP)
                {
                    VI.setZoom(zoomDelta * ZOOM_SCALE);
                }
            }
        }
        else
        {
            prevPalmDist = 0;
        }
    }

    private void pitching()
    {
        Vector LPalmV, LWristV, RPalmV, RWristV;
        float wristTilt = 0;
        float pitchDelta = 0;
        VolumeInterface VI = FindObjectOfType<VolumeInterface>();

        if (leftHand != null && rightHand != null)
        {
            LPalmV = leftHand.PalmPosition;
            LWristV = leftHand.WristPosition;
            RPalmV = rightHand.PalmPosition;
            RWristV = rightHand.WristPosition;
            wristTilt = (LPalmV.Magnitude - LWristV.Magnitude) + (RPalmV.Magnitude - RWristV.Magnitude) - PITCH_OFFSET;
            
            if(prevWristTilt == 0)
            {
                prevWristTilt = wristTilt;
            }
            else if(prevWristTilt != wristTilt)
            {
                pitchDelta = wristTilt - prevWristTilt;
                prevWristTilt = wristTilt;
                
                if(Mathf.Abs(pitchDelta) > PITCH_CAP)
                {
                    VI.setPitch(pitchDelta * PITCH_SCALE);
                }
            }
        }
        else
        {
            prevWristTilt = 0;
        }
    }

    private void yawing()
    {
        VolumeInterface VI = FindObjectOfType<VolumeInterface>();
        if(yawLeft)
            VI.setYaw(Time.deltaTime * YAW_CW);
        else if(yawRight)
            VI.setYaw(Time.deltaTime * YAW_CCW);
        else
            VI.setYaw(0);
    }

    private void setTFIsovalue()
    {
        float LHX, RHX;
        float deltaMin, deltaMax;
        TFInterface FT = FindObjectOfType<TFInterface>();

        if (leftHand != null && rightHand != null)
        {
            LHX = leftHand.PalmPosition.x;
            RHX = rightHand.PalmPosition.x;
            if(prevLHX == 0 && prevRHX == 0)
            {
                prevLHX = LHX;
                prevRHX = RHX;
                prevPalmDist = leftHand.PalmPosition.DistanceTo(rightHand.PalmPosition);
            }
            else if(prevLHX != LHX || prevRHX != RHX)
            {
                deltaMin = LHX - prevLHX;
                deltaMax = RHX - prevRHX;
                prevLHX = LHX;
                prevRHX = RHX;
                prevPalmDist = leftHand.PalmPosition.DistanceTo(rightHand.PalmPosition);

                FT.setIsovalue(deltaMin, deltaMax);
            }
        }
        else
        {
            prevLHX = 0;
            prevRHX = 0;
        }
    }


    private void snapCheck()
    {
        if (rightHand != null && rightHand.Fingers != null)
        {
            VolumeInterface VI = FindObjectOfType<VolumeInterface>();
            Finger thumb = rightHand.Fingers[0];
            Finger index = rightHand.Fingers[1];
            Finger middle = rightHand.Fingers[2];
            Vector thumbTip, indexTip, middleTip;
            float TMdistance;
            float TIdistance;

            thumbTip = thumb.TipPosition;
            indexTip = index.TipPosition;
            middleTip = middle.TipPosition;
            TMdistance = thumbTip.DistanceTo(middleTip);
            TIdistance = thumbTip.DistanceTo(indexTip);

            //print("TMdistance: " + TMdistance);
            //print("TIdistance: " + TIdistance);

            if(TMdistance < SNAP_ON_CAP)
            {
                isSnapping = true;
            }

            if(isSnapping)
            {
                if(TIdistance < SNAP_ON_CAP)
                {
                    VI.reset();
                    prevPalmDist = 0;
                    prevWristTilt = 0;
                    prevLHX = 0;
                    prevRHX = 0;
                    print("snap!");
                }
                
                if(TIdistance > SNAP_OFF_CAP)
                {
                    isSnapping = false;
                }
            }
        }
    }


/* Public functions that perform changes only if invoked */

    public void turnClockwise(){
        //VolumeInterface.yawIncrement = 0.5f;
        yawLeft = false;
        yawRight = true;
        print ("Right-hand Activated");
    }

    public void turnCounterclockwise(){
        //VolumeInterface.yawIncrement = -0.5f;
        yawLeft = true;
        yawRight = false;
        print ("Left-hand Activated");
    }

    public void setDeactivate(){
        //VolumeInterface.yawIncrement = 0;
        yawLeft = false;
        yawRight = false;
        print ("Deactivated");
    }

    public void setTFIsovalueActive(){
        setIsoActive = true;
        print ("set FT isovalue active!");
    }

    public void setTFIsovalueDeactive(){
        setIsoActive = false;
        print ("set FT isovalue inactive!");
    }
}
