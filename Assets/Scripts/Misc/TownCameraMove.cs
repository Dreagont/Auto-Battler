using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownCameraMove : MonoBehaviour
{
    public Camera Camera;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void MoveCamera(int Index)
    {
        Camera.transform.localPosition = Vector3.zero;
        switch (Index)
        {
            case 0:
                Camera.transform.localPosition = new Vector3(-50, -27.5f, 0);
                break;
            case 1:
                Camera.transform.localPosition = new Vector3(-61, -27.5f, 0);
                break;
            case 2:
                Camera.transform.localPosition = new Vector3(-73, -27.5f, 0);
                break;
            case 3:
                Camera.transform.localPosition = new Vector3(-73, -15, 0);
                break;
            case 4:
                Camera.transform.localPosition = new Vector3(-61, -15, 0);
                break;
            case 5:
                Camera.transform.localPosition = new Vector3(-50, -15, 0);
                break;
            default:
                Debug.LogWarning("Invalid camera index.");
                break;
        }
    }

}
