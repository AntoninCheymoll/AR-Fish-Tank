using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManaging : MonoBehaviour
{
    GameObject fishLeft;
    GameObject fishRight;
    GameObject fishMouvement;

    float gameX = -1;
    float gameY = -1;

    public float newDirX;
    public float newDirY;

    float fishLength = 2.3f;
    float fishWidth = 0.8f;

    public float directionX = 1;
    public float directionY = 0;

    public float speed = 0.3f;
    public float rotationSpeed = 0.1f;

    public float borderOffset = 5f;

    // Start is called before the first frame update
    void Start()
    {
        newDirX = directionX;
        newDirY = directionY;

        fishLeft = GameObject.Find("fishLeft");
        fishRight = GameObject.Find("fishRight");
        fishMouvement = GameObject.Find("Fish");

        //fishLeft.transform.localScale = new Vector3(0, 0, 0);
        //fishRight.transform.localScale = new Vector3(0, 0, 0);

        //updateRotations(new Vector3(0, getOrientation(), 0));

        Animation anim = fishMouvement.GetComponent<Animation>();

        anim["mouvement"].enabled = false;
        anim["mouvement"].enabled = true;
     


        Debug.Log(anim["mouvement"] + " " + anim["left"] + " " + anim["right"]);
        //anim["left"].enabled = true;


        //Debug.Log("num" + fishLeft.GetComponent<Animation>().GetClipCount());
        /*
        AnimationState s;
        foreach (AnimationState clip in anim)
        {
            s = clip;
            anim.clip = clip.clip;
            Debug.Log(clip.name);
        }*/

        //anim["legacy"] = "ok";

    }

    // Update is called once per frame
    void Update()
{
    
        
        /*
    if (gameX == -1)
    {
        gameX = GameObject.Find("Ground").GetComponent<GroundBehavior>().gameX;
        gameY = GameObject.Find("Ground").GetComponent<GroundBehavior>().gameY;
    }


        //float rad = getRadian();
        updatePositions(new Vector3(directionX, 0, directionY) * speed);

        if (newDirX == directionX && newDirY == directionY)
        {

            if (getPosition().x > gameX * 4)
            {
                updateDir(Mathf.PI);

            }
            else if (getPosition().x < -(gameX * 4))
            {
                updateDir(0);
            }


            if (getPosition().z > gameY * 4)
            {
                updateDir(-Mathf.PI / 2);

            }
            else if (getPosition().z < -(gameY * 4))
            {
                updateDir(Mathf.PI / 2);
            }





        }else
        {
            directionX = (directionX > newDirX) ? Mathf.Max(newDirX, directionX - rotationSpeed) : Mathf.Min(newDirX, directionX + rotationSpeed);
            directionY = (directionY > newDirY) ? Mathf.Max(newDirY, directionY - rotationSpeed) : Mathf.Min(newDirY, directionY + rotationSpeed);

            //if (newDirX == directionX && newDirY == directionY) mouvementVisible();
        }

        updateRotations(new Vector3(0, getOrientation(), 0));
        */
    }


    /*
    float getOrientation()
    {
        return Mathf.Atan2(directionX, directionY) / 2 / Mathf.PI * 360;
    }

    void updateDir(float angle)
    {
        //leftVisible();
        float randAngle = Random.Range(angle - Mathf.PI / 4, angle + Mathf.PI / 4);
        newDirX = Mathf.Cos(randAngle);
        newDirY = Mathf.Sin(randAngle);
    }

    void updatePositions(Vector3 pos)
    {
        fishLeft.transform.position += pos;
        fishMouvement.transform.position += pos;
        fishRight.transform.position += pos;
    }

    void updateRotations(Vector3 rot)
    {
        fishLeft.transform.eulerAngles = rot;
        fishMouvement.transform.eulerAngles = rot;
        fishRight.transform.eulerAngles = rot;
    }

    Vector3 getPosition()
    {
        return fishLeft.transform.position;
    }

    void leftVisible()
    {
        fishLeft.transform.localScale = new Vector3(1, 1, 1);
        fishRight.transform.localScale = new Vector3(0, 0, 0);
        fishMouvement.transform.localScale = new Vector3(0, 0, 0);
    }

    void rightVisible()
    {
        fishLeft.transform.localScale = new Vector3(0, 0, 0);
        fishRight.transform.localScale = new Vector3(1, 1, 1);
        fishMouvement.transform.localScale = new Vector3(0, 0, 0);
    }

    void mouvementVisible()
    {
        fishLeft.transform.localScale = new Vector3(0, 0, 0);
        fishRight.transform.localScale = new Vector3(0, 0, 0);
        fishMouvement.transform.localScale = new Vector3(1, 1, 1);
    }*/
}
