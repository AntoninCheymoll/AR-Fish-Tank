using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class used to resize dinamically the ground at the start of the game 
public class GroundManager : MonoBehaviour
{
    public float gameH = 12;
    public float gameW = 22f;

    // Start is called before the first frame update
    void Start()
    {
        //resize ground
        transform.localScale = new Vector3(gameW, 1, gameH);
    }

}
