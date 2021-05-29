using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillBehavior : MonoBehaviour
{
    Vector3 floatingOrientation;
    
    float framNum = 0;
    int currentDir = 1;
    int frameNbBeforeChangingDir;
    
    GameObject food;
    
    public bool isFlying = false;

    // Start is called before the first frame update
    void Start()
    {
        this.frameNbBeforeChangingDir = Random.Range(40,100);
        float randomAngle = Random.Range(0, Mathf.PI*2);
        this.floatingOrientation = new Vector3(Mathf.Cos(randomAngle),0, Mathf.Sin(randomAngle));

        this.food = this.transform.parent.gameObject;
        float x = this.food.transform.position.x + Random.Range(-0.02f, 0.02f);
        float z = this.food.transform.position.z + Random.Range(-0.02f, 0.02f);

        this.transform.position = new Vector3(x, this.transform.position.y, z);

    }

    void Update(){
        if (!isFlying)
        {
            normalUpdate();
        }
    }

   


    // Update is called once per frame
    void normalUpdate()
    {
        this.framNum++;
        if(this.framNum == this.frameNbBeforeChangingDir)
        {
            this.framNum = 0;
            this.currentDir *= -1;

            if (this.currentDir == 1)
            {
                this.frameNbBeforeChangingDir = Random.Range(40, 100);
                float randomAngle = Random.Range(0, Mathf.PI * 2);
                this.floatingOrientation = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle));
            }
        }
        float speed = 0.0002f*(1-Mathf.Abs(this.framNum - this.frameNbBeforeChangingDir/2)/(this.frameNbBeforeChangingDir / 2) );
        this.transform.position += this.floatingOrientation * speed * this.currentDir;

    }

    

}
