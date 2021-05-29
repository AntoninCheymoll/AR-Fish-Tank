using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableDisplays : MonoBehaviour
{
    void Start()
    {
        
        for (int i = 1; i < Display.displays.Length; i++)
            {
                Display.displays[i].Activate(1024, 768, 60);
            }
    }
}
