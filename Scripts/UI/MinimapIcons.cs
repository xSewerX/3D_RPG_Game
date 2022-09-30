using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIcons : MonoBehaviour
{
    private GameObject cam;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void LateUpdate()
    {
        // transform.rotation = Quaternion.Euler(90f, cam.transform.eulerAngles.y, 0f);
        
        
    }
}
