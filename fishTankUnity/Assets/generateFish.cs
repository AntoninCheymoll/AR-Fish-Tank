using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateFish : MonoBehaviour
{
    public GameObject orangeFish;
    public GameObject redFish;
    public GameObject blueFish;
    public GameObject greenFish;

    string currentGeneratedFish = "random";
    GameObject ground;
    float initialY;

    void Start() {
        this.ground = GameObject.Find("Ground");
        this.initialY = ground.transform.position.y - 100f;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("b")) {
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
        float rand = Random.value * 3000;
        if (rand < 10) createFish();

    }

    void createFish() {

        GameObject toCreateFish = null;
        if (currentGeneratedFish == "blue") {
            toCreateFish = this.blueFish;
        }
        else if (currentGeneratedFish == "red")
        {
            toCreateFish = this.redFish;
        }
        else if (currentGeneratedFish == "orange") {
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

        if (GameObject.FindGameObjectsWithTag("fish").Length < 10) {

            GameObject createdFish = Instantiate(toCreateFish, transform.position, Quaternion.identity, GameObject.Find("Fishs").transform);
            createdFish.transform.eulerAngles = (Random.value<0.5)?new Vector3(0, 130, 0): new Vector3(0, 290, 0);
            createdFish.transform.position += new Vector3(0, -0.05f, 0.05f);
            createdFish.tag = "fish";
            createdFish.SetActive(true);

            FishMovement script = createdFish.GetComponent(typeof(FishMovement)) as FishMovement;
            script.appearFromHouse = true;
        }
    }

}
