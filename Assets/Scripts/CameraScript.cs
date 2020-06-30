using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public int cameraWidth;
    public int cameraHeight;
    public int screenWidth;
    public int screenHeight;

    private void Awake()
    {
        Screen.SetResolution(screenWidth, screenHeight, true);
        SetResolution(cameraWidth, cameraHeight);
    }

    private void SetResolution(float w, float h)
    {
        Camera.main.orthographicSize = h;
        Camera.main.aspect = w / h;
    }
}
