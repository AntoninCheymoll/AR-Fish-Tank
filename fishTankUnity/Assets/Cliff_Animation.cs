using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class Cliff_Animation : MonoBehaviour
{
    Vector3 initialPos; 
    bool start_moving = false;

    float speed = 0.003f;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown("c"))
        {
            start_moving = true;
        }

        if (Input.GetKeyDown("v"))
        {
            transform.position = initialPos;
        }

        if (start_moving)
        {
            
            Vector2 distance = (getTargetPos() - new Vector2(transform.position.x, transform.position.z)).normalized;

            this.transform.position += new Vector3(distance.x, 0, distance.y) * speed;

            if ((getTargetPos() - new Vector2(transform.position.x, transform.position.z)).magnitude < speed)
            {
                start_moving = false;
            }
        }
    }

    Vector2 getTargetPos()
    {
        Vector2 pos = new Vector2(GameObject.Find("Scenery").transform.position.x, GameObject.Find("Scenery").transform.position.z);
        pos.x += 0.3f;
        return pos;
    }

    
}
