using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectGrappleable : MonoBehaviour
{
    public GameObject player;
    Grapple _grapple;

	void Start ()
    {
        _grapple = player.GetComponent<Grapple>();
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Grappleable")
        {
            _grapple.isHooked = true;
            _grapple.hookedObject = other.gameObject;
        }

        if (other.tag == "Swingable")
        {
            _grapple.isSwinging = true;
            _grapple.swingObject = other.gameObject;
        }

        if (other.tag == "Collectable")
        {
            _grapple.isGrabbed = true;
            _grapple.grabbedObject = other.gameObject;
        }
    }
}
