using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeInterface : MonoBehaviour
{
    /* constants/ macros defined here */
    private const float TRANSFORM_SPEED = 10.0f;
    private const float ZOOM_MAX = 2f;
    private const float ZOOM_MIN = 0.7f;

    /* Private variables defined here */
    private static float yawIncrement;
    private Vector3 origScale;
    private Quaternion origRotation;

    // Start is called before the first frame update
    void Start()
    {
        origScale = transform.localScale;
        origRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(Vector3.forward * TRANSFORM_SPEED * yawIncrement);
    }


    public void setZoom(float delta)
    {
        Vector3 afterScaled =  transform.localScale * (1.0f + delta);
        if(afterScaled.x < ZOOM_MAX && afterScaled.x > ZOOM_MIN)
        {
            transform.localScale = afterScaled;
        }
    }

    public void setYaw(float delta)
    {
        transform.Rotate(Vector3.forward * TRANSFORM_SPEED * delta);
    }

    public void setPitch(float delta)
    {
        transform.Rotate(Vector3.right * TRANSFORM_SPEED * delta);
    }

    public void reset()
    {
        transform.localScale = origScale;
        transform.rotation = origRotation;
    }
}
