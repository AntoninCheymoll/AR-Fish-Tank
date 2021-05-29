using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingScript : MonoBehaviour
{
    public bool isFlying;
    public ParticleSystem watterRipple;
    GameObject generatedParticle = null;
    GameObject ground;
    Animation anim;
    public bool isFish;
    float targetedZ;

    public AudioClip splashSound;
    AudioSource audio;
    bool splashPlayed = false;

    // Start is called before the first frame update
    public void Start()
    {
        if (this.isFlying) {
            
            audio = GetComponent<AudioSource>();


            this.ground = GameObject.Find("Ground");
            this.targetedZ = ground.transform.position.y + 0.055f;

            if (this.isFish){
                anim = GetComponent<Animation>();
                anim.Stop();
            }

            //if (this.isFish) this.transform.position += new Vector3(0, 150, 0);


            if (this.watterRipple) {
                Vector3 position = new Vector3(transform.position.x, ground.transform.position.y + 0.13f, transform.position.z);
                this.generatedParticle = Instantiate(this.watterRipple.gameObject, position, Quaternion.identity);
                StartCoroutine(runWaterRipple());
            }

        }
        else
        {
            if (this.isFish)
            {
                FishMovement script = this.GetComponent(typeof(FishMovement)) as FishMovement;
                script.isFlying = false;
            }
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (this.isFlying)
        {
            if (this.transform.position.y > 10)
            {
                this.transform.position -= new Vector3(0, 5, 0);
            }
            else
            {
                float ratio = (this.isFish) ? 5f : 10;
                float modif = (this.transform.position.y - this.targetedZ) / ratio;
                this.transform.position -= new Vector3(0, modif, 0);

                

                if (this.isFish && !splashPlayed && this.transform.position.y < ground.transform.position.y + 0.13f)
                {
                    Debug.Log("in");
                    audio.volume = 0.5f;
                    audio.PlayOneShot(this.splashSound);
                    splashPlayed = true;
                }

                if (this.transform.position.y < targetedZ + 0.01)
                {
                    this.isFlying = false;
                    this.transform.position = new Vector3(this.transform.position.x, this.targetedZ , this.transform.position.z);
                    
                    if (this.isFish)
                    {
                        FishMovement fishScript = this.GetComponent(typeof(FishMovement)) as FishMovement;
                        StartCoroutine(fishScript.startMoving());
                    }

                    if (!this.isFish)
                    {
                        foreach (Transform pill in this.transform) {
                            PillBehavior pillScript = pill.GetComponent<PillBehavior>(); 
                            if (pillScript != null) pillScript.isFlying = false;
                        }
                    }
                }
            }
        }
    }
 

    IEnumerator runWaterRipple()
    {
        yield return new WaitForSeconds(0.2f);
        if(!this.isFish) yield return new WaitForSeconds(0.3f);
        this.generatedParticle.GetComponent<ParticleSystem>().Play();


    }
}
