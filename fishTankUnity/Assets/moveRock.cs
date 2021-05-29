using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class moveRock : MonoBehaviour
{
    public GameObject rock;
    float X_OFFSET = 0.08f;
    float Y_OFFSET = 0.22f;
    float Z_OFFSET = 0.08f;
    // Start is called before the first frame update
    void Start()
    {
    }
        

    // Update is called once per frame
    void Update()
    {
        Vuforia.TrackableBehaviour.Status status = gameObject.GetComponent<TrackableBehaviour>().CurrentStatus;

        if (status == Vuforia.TrackableBehaviour.Status.TRACKED)
        {
            rock.SetActive(true);
            Vector3 newRockPosition = rock.transform.position;
            Quaternion newRockRotate = rock.transform.rotation;

            newRockPosition.x = gameObject.transform.position.x - X_OFFSET;
            newRockPosition.y = gameObject.transform.position.y - Y_OFFSET;
            newRockPosition.z = gameObject.transform.position.z - Z_OFFSET;
            rock.transform.position = newRockPosition;

            newRockRotate.y = gameObject.transform.rotation.y;
            rock.transform.rotation = newRockRotate;
        } else
        {
            rock.SetActive(false);
        }
    }
}
