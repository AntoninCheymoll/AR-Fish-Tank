using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class used to resize dinamically the water plan at the start of the game in function of the ground dimensions
public class WaterSizeManaging : MonoBehaviour
{
    float gameW = -1;
    float gameH = -1;
    // Start is called before the first frame update
    void Start()
    {
        //get the ground dimension from its script
        gameW = GameObject.Find("Ground").GetComponent<GroundManager>().gameW;
        gameH = GameObject.Find("Ground").GetComponent<GroundManager>().gameH;

        //resize
        transform.localScale = new Vector3(gameW, 1, gameH);
    }

}
