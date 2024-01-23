using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;


public class dropFood : MonoBehaviour
{
    private bool tracking;
    private bool dropped;
    public bool dropFish = false;
    public GameObject food;

    public GameObject orangeFish;
    public GameObject redFish;
    public GameObject blueFish;
    public GameObject greenFish;

    string currentGeneratedFish = "random";

    public GameObject scene;
    // Start is called before the first frame update
    void Start()
    {
        tracking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("b"))
        {
            currentGeneratedFish = "blue";
        }
        if (Input.GetKeyDown("r"))
        {
            currentGeneratedFish = "red";
        }
        if (Input.GetKeyDown("o"))
        {
            currentGeneratedFish = "orange";
        }
        if (Input.GetKeyDown("g"))
        {
            currentGeneratedFish = "green";
        }
        if (Input.GetKeyDown("a"))
        {
            currentGeneratedFish = "random";
        }
        if (Input.GetKeyDown("s"))
        {
            this.dropFish = !this.dropFish;
        }

        Vuforia.TrackableBehaviour.Status status = gameObject.GetComponent<TrackableBehaviour>().CurrentStatus;
        if (status != Vuforia.TrackableBehaviour.Status.NO_POSE && !tracking) {
            tracking = true;
            dropped = false;
            if (dropFish)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
            } else
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
            }
        } else if (status == Vuforia.TrackableBehaviour.Status.NO_POSE)
        {
            tracking = false;
        }
        if (tracking && !dropped) {
            Vector3 rotation = gameObject.transform.rotation.eulerAngles;
            float rx = rotation.x;
            float rz = rotation.z;
            if(rx > 180) {
                rx -= 360;
            }
            if(rz > 180) {
                rz -= 360;
            }
            if(Mathf.Abs(rx) > 50 || Mathf.Abs(rz) > 50) {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(false);
                dropped = true;
                if (this.dropFish)
                {
                    generateFish(gameObject.transform.position);
                }
                else
                {
                     
                    GameObject clone = Instantiate(food, gameObject.transform.position, food.transform.rotation, scene.transform);
                    clone.SetActive(true);
                }
            }
        }
        if (Input.GetKeyDown("d"))
        {
            Vector3 pos = GameObject.Find("Scenery").transform.position;
            pos.y += 50; pos.x += 0.3f; pos.z += 0.05f;

            if (dropFish)
            {
                generateFish(pos);
            }
            else
            {
                GameObject clone = Instantiate(food, pos, food.transform.rotation, scene.transform);
                FlyingScript script = clone.GetComponent(typeof(FlyingScript)) as FlyingScript;
                script.isFlying = true;
                clone.SetActive(true);
            }
            
        }
    }

    void generateFish(Vector3 position)
    {
        GameObject toCreateFish = null;

        if (currentGeneratedFish == "blue")
        {
            toCreateFish = this.blueFish;
        }
        else if (currentGeneratedFish == "red")
        {
            toCreateFish = this.redFish;
        }
        else if (currentGeneratedFish == "orange")
        {
            toCreateFish = this.orangeFish;
        }
        else if (currentGeneratedFish == "random")
        {
            int num = Random.Range(0, 4);
            if (num == 0)
            {
                toCreateFish = this.redFish;
            }
            else if (num == 1)
            {
                toCreateFish = this.greenFish;
            }
            else if (num == 2)
            {
                toCreateFish = this.blueFish;
            }
            else if (num == 3)
            {
                toCreateFish = this.orangeFish;
            }

        }

        GameObject clone = Instantiate(toCreateFish, position, toCreateFish.transform.rotation, scene.transform);
        clone.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        FlyingScript script = clone.GetComponent(typeof(FlyingScript)) as FlyingScript;
        script.isFlying = true;
        script.isFish = true;
        clone.SetActive(true);
    }
}


/*

public class dropFood : MonoBehaviour
{
    private bool tracking;
    private bool dropped;
    public GameObject food;
    public GameObject foodParent;
    // Start is called before the first frame update
    void Start()
    {
        tracking = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vuforia.TrackableBehaviour.Status status = gameObject.GetComponent<TrackableBehaviour>().CurrentStatus;
        if (status != Vuforia.TrackableBehaviour.Status.NO_POSE && !tracking)
        {
            tracking = true;
            dropped = false;
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (status == Vuforia.TrackableBehaviour.Status.NO_POSE)
        {
            tracking = false;
        }
        if (tracking && !dropped)
        {
            Vector3 rotation = gameObject.transform.rotation.eulerAngles;
            float rx = rotation.x;
            float rz = rotation.z;
            if (rx > 180)
            {
                rx -= 360;
            }
            if (rz > 180)
            {
                rz -= 360;
            }
            if (Mathf.Abs(rx) > 50 || Mathf.Abs(rz) > 50)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                dropped = true;
                GameObject foodClone = Instantiate(food, gameObject.transform.position, food.transform.rotation, foodParent.transform);
                foodClone.SetActive(true);
            }
        }
        if (Input.GetKeyDown("d"))
        {
            
            Vector3 pos = GameObject.Find("Scenery").transform.position;
            pos.y += 50;
            GameObject foodClone = Instantiate(food, pos, food.transform.rotation, foodParent.transform);
            FlyingScript script = foodClone.GetComponent(typeof(FlyingScript)) as FlyingScript;
            script.isFlying = true;
            foodClone.SetActive(true);
        }
    }
}
*/