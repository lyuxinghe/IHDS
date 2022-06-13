using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityVolumeRendering;

public class TFInterface : MonoBehaviour
{
    /* constants/ macros defined here */
    private const float OVA_MAX = 1.0f;
    private const float OVA_MIN = 0.0f;
    private const float MIN_GAP = 0.01f;

    private const float MIN_SCALE = 4f;
    private const float MAX_SCALE = 4f;

    
    /* Private variables defined here */
    private static float curMin;
    private static float curMax;

    /* Default color value assigned to TF function (see Transfer Function Database file) */ 
    private Color c1 = new Color(0.11f, 0.14f, 0.13f, 1.0f);
    private Color c2 = new Color(0.469f, 0.354f, 0.223f, 1.0f);
    private Color c3 = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 curVisibilityWindow;
        VolumeRenderedObject VRO = FindObjectOfType<VolumeRenderedObject>();
        curVisibilityWindow = VRO.GetVisibilityWindow();
        curMin = curVisibilityWindow.x;
        curMax = curVisibilityWindow.y;
    }

    private Color mapColor(int index, float dif, Color oldRGBColor)
    {
        Color newRGBColor;
        switch (index)
        {
        case 2:
            newRGBColor = Color.Lerp(c3, Color.blue, 1.0f - dif);
            break;
        case 1:
            newRGBColor = Color.Lerp(c2, Color.blue, 1.0f - dif);
            break;
        case 0:
            newRGBColor = Color.Lerp(c1, Color.blue, 1.0f - dif);
            break;
        default:
            newRGBColor = oldRGBColor;
            break;
        }
        return newRGBColor;
    }

    private void TFColorChange(float newMin, float newMax)
    {
        VolumeRenderedObject[] volobjs = GameObject.FindObjectsOfType<VolumeRenderedObject>();
        Color oldRGBColor, newRGBColor;
        
        for(int i = 0; i < volobjs.Length; i++)
        {
            for(int j = 0; j < volobjs[i].transferFunction.colourControlPoints.Count; j++)
            {
                oldRGBColor = volobjs[i].transferFunction.colourControlPoints[j].getColor();
                newRGBColor = mapColor(j, newMax-newMin, oldRGBColor);
                volobjs[i].transferFunction.colourControlPoints[j].setColor(newRGBColor);
            }
            volobjs[i].transferFunction.GenerateTexture();
        }
    } 

    public void setIsovalue(float deltaMin, float deltaMax)
    {
        VolumeRenderedObject VRO = FindObjectOfType<VolumeRenderedObject>();
        float newMin = curMin + MIN_SCALE * deltaMin;
        float newMax = curMax + MIN_SCALE * deltaMax;

        if(newMin < OVA_MIN)
            newMin = OVA_MIN;
        if(newMax > OVA_MAX)
            newMax = OVA_MAX;
        if(newMin + MIN_GAP > newMax)
        {
            newMax = curMax;
            newMin = curMin;
        }
        VRO.SetVisibilityWindow(newMin, newMax);
        TFColorChange(newMin, newMax);
    }
}
