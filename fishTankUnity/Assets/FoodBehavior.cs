using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBehavior : MonoBehaviour
{
    public bool isFlying = true;

    // Start is called before the first frame update
    void Start(){

        if (this.isFlying){
            StartCoroutine(dropFoodRoutine());
        }
        else{
            addInFishsFoodList();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void eaten(){
        foreach(Transform child in transform)
        {
            if(child.localScale != new Vector3(0, 0, 0))
            {
                child.localScale = new Vector3(0, 0, 0);
                return;
            }
        }
        Destroy(this.gameObject);
        removeFromFishsFoodList();
    }


    IEnumerator dropFoodRoutine()
    {
        FlyingScript script = this.GetComponent<FlyingScript>();
        script.isFlying = true;
        script.Start();
        yield return new WaitForSeconds(0f);
        addInFishsFoodList();
    }

    GameObject[] getAllFishs()
    {
        /*List<GameObject> res = new List<GameObject>();
        GameObject fishs = GameObject.Find("Fishs");
        foreach (Transform child in fishs.transform)
        {
            res.Add(child.gameObject);
        }*/
        return GameObject.FindGameObjectsWithTag("fish");
    }


    void addInFishsFoodList()
    {
        foreach (GameObject fish in getAllFishs())
        {
            FishMovement script = fish.GetComponent(typeof(FishMovement)) as FishMovement;
            script.addFood(this.gameObject);
        }
    }

    void removeFromFishsFoodList()
    {
        foreach (GameObject fish in getAllFishs())
        {
            FishMovement script = fish.GetComponent(typeof(FishMovement)) as FishMovement;
            script.removeFood(this.gameObject);
        }
    }

}
