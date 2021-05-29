using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this script is managing all the fish mouvements and behaviors
public class FishMovement : MonoBehaviour
{

    GameObject wallLeft;
    GameObject wallRight;
    GameObject wallUp;
    GameObject wallDown;

    //float fishLength = 12;
    float fishLength = 0;

    //direction of the fish
    float directionX;
    float directionZ;

    //new direction that the fish is turning to
    float newDirX;
    float newDirZ;

    float initialSpeed= 0.0035f;
    float speed;
    float rushSpeed = 0.007f;
    float initialRotationSpeed = 0.05f;
    float rotationSpeed;

    string along = ""; //along wich border the fish is, "no" if the fish is along none

    bool isFaster = false;

    bool goTowardFood = false;
    bool hasEaten = false;
    bool isEating = false;

    float detectedFoodDist = 0.4f;
    float speedDecreaseNearFood = 0.1f;
    float canEatDist = 0.05f;

    bool isFlying = true;
    bool justLanded = false;

    bool cliffCollisionDetected = false;
    float exitDirX;
    float exitDirZ;
    bool isInsideCliff = false;

    Animation anim;
    GameObject cliff;
    List<GameObject> foodsList = new List<GameObject>();

    public AudioClip rushSound;
    AudioSource audio; 

    // Start is called before the first frame update
    void Start(){

        audio = GetComponent<AudioSource>();

        this.wallDown = GameObject.Find("WallDown");
        this.wallUp = GameObject.Find("WallUp");
        this.wallRight = GameObject.Find("WallRight");
        this.wallLeft = GameObject.Find("WallLeft");

        //give a random initial direction to the fish
        float randDir = Random.Range(0, Mathf.PI * 2);
        this.directionX = Mathf.Cos(randDir);
        this.directionZ = Mathf.Sin(randDir);

        this.newDirZ = directionZ;
        this.newDirX = directionX;

        this.anim = GetComponent<Animation>();
        this.speed = initialSpeed;
        this.rotationSpeed = initialRotationSpeed;

        this.cliff = GameObject.Find("Cliff");

        transform.eulerAngles = new Vector3(0, getFishOrientationDegree(), 0);
    }


    private void Update()
    {
        if (!this.isFlying){
            normalUpdate();

        }
        else if (this.justLanded){
            transform.position += new Vector3(directionX, 0, directionZ) * 0.002f;
        }
    }

    // Update is called once per frame
    void normalUpdate(){
        //update the fish object position and orientation in the game
        transform.position += new Vector3(directionX, 0, directionZ) * this.speed;
        transform.eulerAngles = new Vector3(0, getFishOrientationDegree(), 0);
        
        if (this.foodsList.Count > 0 && !this.hasEaten && !this.isEating && !this.isInsideCliff)
        {
            float minDist = int.MaxValue;
            GameObject nearerFoods = null;

            foreach(GameObject food in this.foodsList)
            {
                if (food == null) break;
                float distToFoods = Vector3.Distance(food.transform.position, transform.position);
                if(distToFoods < minDist){
                    minDist = distToFoods;
                    nearerFoods = food;
                }
            }
            
            if (minDist < detectedFoodDist )
            {
                if (minDist < this.canEatDist)
                {
                    isEating = true;
                    StartCoroutine(hasEatenRoutine(nearerFoods));
                }

                else
                {
                    if (!goTowardFood)
                    {
                        Vector3 foodDir = nearerFoods.transform.position - this.transform.position;
                        float angleToFood = Mathf.Atan2(foodDir.z, foodDir.x);
                        updateDir(angleToFood);
                        this.goTowardFood = true;
                        goFaster();
                    }

                    if (minDist < speedDecreaseNearFood)
                    {

                        this.speed = this.rushSpeed * (minDist -this.canEatDist + 0.005f) / speedDecreaseNearFood;

                    }
                }     
            }
        }

        

        if (this.cliffCollisionDetected)
        {
            Vector3 dir = this.cliff.transform.position - transform.position;
            float angleFishCliff = Mathf.Atan2(dir.x, dir.z);
            angleFishCliff = (angleFishCliff < 0) ? (angleFishCliff + Mathf.PI * 2) : (angleFishCliff%(Mathf.PI * 2));

            float fishOrientation = (getFishOrientationRadian() < 0) ? (getFishOrientationRadian() + Mathf.PI * 2) : (getFishOrientationRadian() % (Mathf.PI * 2));

            float angleDiff = angleFishCliff - fishOrientation;

            if (angleDiff < 0 && angleDiff > -1f)
            {

                this.newDirZ = Mathf.Cos(angleFishCliff + 1f);
                this.newDirX = Mathf.Sin(angleFishCliff + 1f);

                this.exitDirX = Mathf.Sin(angleFishCliff - 1f);
                this.exitDirZ = Mathf.Cos(angleFishCliff - 1f);

            }

            if (angleDiff > 0 && angleDiff < 1f)
            {
                this.newDirZ = Mathf.Cos(angleFishCliff - 1f);
                this.newDirX = Mathf.Sin(angleFishCliff - 1f);

                this.exitDirX = Mathf.Sin(angleFishCliff + 1f);
                this.exitDirZ = Mathf.Cos(angleFishCliff + 1f);
            }

            this.cliffCollisionDetected = false;
        }

        //the fish can start rushing randomly (only if it met canRush criterias)
        if (Random.Range(0, 400) == 1 && canRush() && !this.goTowardFood )
        {
            goFaster();
        }

        //if the fish is no longer rushing but still faster than the initial, decrease slightly the speed
        if (!isFaster && speed > initialSpeed && !this.goTowardFood)
        {
            this.speed = Mathf.Max(this.speed - 0.0001f, this.initialSpeed);
        }
        //test is the fish is swimming toward a wall and if it's not already turning, if yes, we will change it direction by updating newDir
        if (newDirX == directionX && newDirZ == directionZ && runToTheWall())
            {

                //verify that the fish is only near one wall
                if (isNearCorner() == "NO"){
                    //if the fish go toward the right wall
                    if (turnDetection("right"))
                    {
                        //if the fish is going toward the wall not to perpendicullary, it will just start to swim along the wall
                        if (directionX < 0.75)
                        {
                            newDirX = 0;
                            newDirZ = (directionZ < 0) ? -1 : 1;
                            along = "right";

                        }
                        else //else just give a new rndom direction to the fish
                        {
                            updateRandDirDoNotFullyTurn(getOrientationRadian(-directionX, directionZ), 0.8f * Mathf.PI / 2);
                        }

                    }
                    else if (turnDetection("left"))
                    {


                        if (directionX > -0.75)
                        {
                        newDirX = 0;
                            newDirZ = (directionZ < 0) ? -1 : 1;
                            along = "left";
                        }
                        else
                        {
                            updateRandDirDoNotFullyTurn(getOrientationRadian(-directionX, directionZ), 0.8f * Mathf.PI / 2);

                        }
                    }
                    else if (turnDetection("up"))
                    {
                        if (directionZ < 0.75)
                        {
                            newDirX = (directionX < 0) ? -1 : 1;
                            newDirZ = 0;
                            along = "up";

                        }
                        else
                        {
                            updateRandDirDoNotFullyTurn(getOrientationRadian(directionX, -directionZ), 0.8f * Mathf.PI / 2);
                        }


                    }
                    else if (turnDetection("down"))
                    {
                        if (directionZ > -0.75)
                        {
                            newDirX = (directionX < 0) ? -1 : 1;
                            newDirZ = 0;
                            along = "down";

                        }
                        else
                        {
                            updateRandDirDoNotFullyTurn(getOrientationRadian(directionX, -directionZ), 0.8f * Mathf.PI / 2);
                        }
                    }

                    // if the fish is near a corner, give it a new random direction opposite to the corner
                }
                else
                {

                    if (isNearCorner() == "LD") updateRandDir(Mathf.PI / 4, 0.8f * Mathf.PI / 2);
                    if (isNearCorner() == "LU") updateRandDir(-Mathf.PI / 4, 0.8f * Mathf.PI / 2);
                    if (isNearCorner() == "RD") updateRandDir(-5 * Mathf.PI / 4, 0.8f * Mathf.PI / 2);
                    if (isNearCorner() == "RU") updateRandDir(5 * Mathf.PI / 4, 0.8f * Mathf.PI / 2);
                }

                //if the fish should not start turning
            }
            else
            {

                //if the fish is not turning
                if (directionX == newDirX && directionZ == newDirZ)
                {

                    //give back the initial animation in cas it has been modified
                    anim.Play("mouvement");
                    //if it met certain condition (canTurnEverywhere), the fish can randomly start to turn
                    if (canTurnEverywhere() && (Random.Range(0, 100) == 1) && speed == initialSpeed && !this.goTowardFood && !this.isInsideCliff)
                    {
                        updateRandDir(getFishOrientationRadian(), Mathf.PI);
                        //the fish has 1 chance on 2 to start rushing when starting to turn
                        if (Random.Range(0, 1) == 0) goFaster();
                    }

                    //when swim along a wall, can randomly detach the wall
                    if (Random.Range(0, 100) == 1 && ((directionX == 0 && Mathf.Abs(directionZ) == 1) || (directionZ == 0 && Mathf.Abs(directionX) == 1)))
                    {

                        float angle = 0;
                        if (along == "right") angle = (directionZ == 1) ? angle = -Mathf.PI * 3 / 4 : Mathf.PI * 3 / 4;
                        if (along == "left") angle = (directionZ == 1) ? angle = -Mathf.PI / 4 : Mathf.PI / 4;
                        if (along == "up") angle = (directionX == 1) ? angle = -Mathf.PI / 4 : -Mathf.PI * 3 / 4;
                        if (along == "down") angle = (directionX == 1) ? angle = Mathf.PI / 4 : Mathf.PI * 3 / 4;
                        updateRandDir(angle, 0.8f * Mathf.PI / 2);
                    }
                }
                else
                {
                    //update slightly the diection of the fish, to draw near the new direction the fish will have after the turn
                    directionX = (directionX > newDirX) ? Mathf.Max(newDirX, directionX - rotationSpeed) : Mathf.Min(newDirX, directionX + rotationSpeed);
                    directionZ = (directionZ > newDirZ) ? Mathf.Max(newDirZ, directionZ - rotationSpeed) : Mathf.Min(newDirZ, directionZ + rotationSpeed);
                }
            }
    }

    //give the current orientation of the fish in degree
    float getFishOrientationDegree(){
        return Mathf.Atan2(directionX, directionZ) / 2 / Mathf.PI * 360;
    }

    float getFishOrientationRadian()
    {
        return Mathf.Atan2(directionX, directionZ);
    }

    //give the orientation of the given parameters in radian
    float getOrientationRadian(float x, float y){
        return Mathf.Atan2(y, x);
    }

    float getOrientationDegree(float x, float y)
    {
        return Mathf.Atan2(y, x) / 2 / Mathf.PI * 360;
    }
    Vector3 getFishHeadPosition()
    {
        return this.transform.position + (new Vector3(directionX, 0, directionZ)) * 0;
    }

    public void addFood(GameObject food){
        Debug.Log("Food added");
        this.foodsList.Add(food);
    }
    public void removeFood(GameObject food)
    {
        this.foodsList.Remove(food);

        this.speed = initialSpeed;
        this.isEating = false;
        this.goTowardFood = false;

    }

    //give a new direction the fish should turning toward by changing newDir, the new direction is random, deriving from "dir" in a maximum angle given by "angle" (both in radian)
    void updateRandDir(float dir, float angle){

        bool flag = true;
        float randAngle = 0;


        randAngle = Random.Range(dir - angle / 2, dir + angle / 2);

        updateDir(randAngle);
    }

    void updateRandDirDoNotFullyTurn(float dir, float angle)
    {

        bool flag = true;
        float randAngle = 0;

        while (flag)
        {
            randAngle = Random.Range(dir - angle / 2, dir + angle / 2);

            float normRandAngle = (randAngle - Mathf.PI / 2 < 0) ? (randAngle - Mathf.PI / 2 + Mathf.PI * 2) : (randAngle - Mathf.PI / 2 % (Mathf.PI * 2));
            float normFishAngle = (getFishOrientationRadian() < 0) ? (getFishOrientationRadian() + Mathf.PI * 2) : ((getFishOrientationRadian()) % (Mathf.PI * 2));
            normFishAngle = Mathf.PI * 2 - normFishAngle;
            float diff = Mathf.Abs(normFishAngle - normRandAngle);

            if (diff < Mathf.PI - 0.5f || diff > Mathf.PI + 0.5f)
            {
                flag = false;
            }

        }

        updateDir(randAngle);
    }

    void updateDir(float angle) {
        float fishDir = getOrientationRadian(directionX, directionZ)%(Mathf.PI*2);
        if (fishDir < 0) fishDir += Mathf.PI * 2;

        angle = angle % (Mathf.PI * 2);
        if (angle < 0) angle += Mathf.PI * 2;

        if((angle < fishDir && angle > fishDir - Mathf.PI) || (angle -Mathf.PI*2 < fishDir && angle - Mathf.PI * 2 > fishDir - Mathf.PI))
        {
            anim.Play("right");
        }
        else
        {
            anim.Play("left");
        }

        //update the direction
        newDirX = Mathf.Cos(angle);
        newDirZ = Mathf.Sin(angle);
    }

    //return true if the fish is near a wall and swiming in its direction
    bool runToTheWall(){
        return (turnDetection("right") && newDirX > 0) || (turnDetection("left") && newDirX < 0) ||
                (turnDetection("up") && newDirZ > 0) || (turnDetection("down") && newDirZ < 0 );
    }


    //return true if the fish is near a wall defined by "dir"
    bool collideFurther(string dir, float offset)
    {
        Vector3 pos = transform.position;

        if (dir == "left") return ( pos.x < this.wallLeft.transform.position.x - offset*this.directionX);
        if (dir == "right") return  ( pos.x > this.wallRight.transform.position.x - offset * this.directionX);
        if (dir == "up") return  ( pos.z > this.wallUp.transform.position.z - offset * this.directionZ);
        if (dir == "down") return  ( pos.z < this.wallDown.transform.position.z - offset * this.directionZ);

        return false;
    }

    bool turnDetection(string dir)
    {
        return collideFurther(dir, 0.1f);
    }

    //return a string deffining the corner the fish is near of, return "NO" if it's near no corner (L = left, R = Right, D = down, U = Up)
    string isNearCorner(){

        Vector3 pos = transform.position;
        float cornerSize = 0.2f;

        bool isNearLeft = pos.x < this.wallLeft.transform.position.x + cornerSize;
        bool isNearRight = pos.x > this.wallRight.transform.position.x - cornerSize;
        bool isNearUp = pos.z > this.wallUp.transform.position.z - cornerSize;
        bool isNearDown = pos.z < this.wallDown.transform.position.z + cornerSize;

        if (isNearLeft && isNearDown) return "LD";
        if (isNearLeft && isNearUp) return "LU";
        if (isNearRight && isNearUp) return "RU";
        if (isNearRight && isNearDown) return "RD";

        return "NO";
    }

    //we consider that the fish can turn in any direction if it's not too close to any wall
    bool canTurnEverywhere(){

        Vector3 pos = transform.position;
        float cornerSize = 0.1f;

        bool isNearLeft = pos.x < this.wallLeft.transform.position.x + cornerSize;
        bool isNearRight = pos.x > this.wallRight.transform.position.x - cornerSize;
        bool isNearUp = pos.z > this.wallUp.transform.position.z - cornerSize;
        bool isNearDown = pos.z < this.wallDown.transform.position.z + cornerSize;

        return (!isNearLeft && !isNearRight && !isNearUp && !isNearDown);
    }

    //the fish can rush if it wan't face a wall too soon in it's curent direction
    bool canRush(){
        foreach (string dir in new string[] { "right", "left", "up", "down" }) {
            if (collideFurther(dir, 0.2f)) return false;
        }
        return true;
    }

    //the fish start to rush
    void goFaster(){

        if (isFaster) return;

        audio.volume = 0.08f;
        audio.PlayOneShot(this.rushSound);

        //acceleration the animation speed
        isFaster = true;
        anim["mouvement"].speed = 3;
        anim["right"].speed = 3;
        anim["left"].speed = 3;

        //accelerate the speed
        speed = this.rushSpeed;
        rotationSpeed = 0.1f;

        //start the timer that will give back its initial speed
        StartCoroutine(backToNormalSpeed());
    }



    //routine that will run the function stoping the fish run after 0.5 sec
    IEnumerator backToNormalSpeed(){

        yield return new WaitForSeconds(0.5f);

        isFaster = false;

        //reduce the animation speed
        anim["mouvement"].speed = 1;
        anim["right"].speed = 1;
        anim["left"].speed = 1;

        rotationSpeed = initialRotationSpeed;
    }

    public IEnumerator startMoving()
    {
        this.anim.Play("mouvement");
        this.anim["mouvement"].speed = 5;
        this.justLanded = true;

        yield return new WaitForSeconds(0.4f);
        this.anim["mouvement"].speed = 1;

        yield return new WaitForSeconds(0.2f);
        this.isFlying = false;
        this.justLanded = false;

        if (canRush()) goFaster();
        if (canTurnEverywhere()){
            updateRandDir(getFishOrientationRadian(), Mathf.PI);
        }

    }

    IEnumerator hasEatenRoutine(GameObject nearerFoods)
    {

        yield return new WaitForSeconds(0.5f);

        this.hasEaten = true;
        this.isEating = false;
        this.goTowardFood = false;
        this.speed = this.initialSpeed;
        if (canRush()) goFaster();
        if (canTurnEverywhere()) updateRandDir(getFishOrientationRadian(), Mathf.PI);


        if (nearerFoods != null){
            FoodBehavior script = nearerFoods.GetComponent(typeof(FoodBehavior)) as FoodBehavior;

            script.eaten();
            StartCoroutine(digestion());
        }

    }

    IEnumerator digestion()
    {
        yield return new WaitForSeconds(15);
        this.hasEaten = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.name == "Cliff")
        {
            this.isInsideCliff = false;
            this.newDirX = this.exitDirX;
            this.newDirZ = this.exitDirZ;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Cliff")
        {
            this.isInsideCliff = true;
            this.cliffCollisionDetected = true;
        }
    }


}
