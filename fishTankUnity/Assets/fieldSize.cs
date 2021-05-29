using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fieldSize : MonoBehaviour
{

    public float gameH;
    public float gameW;

    // Start is called before the first frame update
    void Start()
    {

       float fov = Camera.main.fieldOfView;
       gameH = (float)(2.0 * Mathf.Tan((float)(fov * Mathf.Deg2Rad * 0.5)) * transform.position.y);
        gameW = gameH / Camera.main.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
