using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class fixScenary : MonoBehaviour
{
    public GameObject scenary;
    bool isFixed;
    void Start()
    {
        isFixed = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vuforia.TrackableBehaviour.Status status = gameObject.GetComponent<TrackableBehaviour>().CurrentStatus;

        if (status != Vuforia.TrackableBehaviour.Status.NO_POSE && Input.GetKeyDown("f"))
        {
            scenary.transform.position = toPosition(gameObject.transform.position);
            //scenary.transform.rotation = gameObject.transform.rotation;
            isFixed = !isFixed;
        }
        if (status != Vuforia.TrackableBehaviour.Status.NO_POSE && !isFixed) {
            scenary.transform.position = toPosition(gameObject.transform.position);
            //scenary.transform.rotation = gameObject.transform.rotation;
        }
    }

    private Vector3 toPosition(Vector3 tagPosition) {
        return new Vector3(tagPosition.x - 0.198f, tagPosition.y + 0.086f, tagPosition.z - 0.089f);
    }
}
