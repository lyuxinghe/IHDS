using UnityEngine;

namespace UnityVolumeRendering
{
    [System.Serializable]
    public class TFColourControlPoint
    {
        public float dataValue;
        public Color colourValue;

        public TFColourControlPoint(float dataValue, Color colourValue)
        {
            this.dataValue = dataValue;
            this.colourValue = colourValue;
        }

        public void setColor(Color colourValue)
        {
            this.colourValue = colourValue;
        }

        public Color getColor()
        {
            return this.colourValue;
        }
    }

    [System.Serializable]
    public class TFAlphaControlPoint
    {
        public float dataValue;
        public float alphaValue;

        public TFAlphaControlPoint(float dataValue, float alphaValue)
        {
            this.dataValue = dataValue;
            this.alphaValue = alphaValue;
        }
    }
}
