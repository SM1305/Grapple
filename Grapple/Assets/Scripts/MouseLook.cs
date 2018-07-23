using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public static float sensitivityX = 5F;
	public static float sensitivityY = 5F;

	public float maximumX = 180F;
    public float minimumX = -180F;

    public float maximumY = 60;
    public float minimumY = -60F;

    float rotationY = 0F;
    float rotationX = 0F;

    public bool hideCursor;



    void Start()
    {
        if (hideCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    void Update ()
	{

        if (axes == RotationAxes.MouseXAndY)
        {
				float rotationX = transform.localEulerAngles.y + Input.GetAxisRaw ("Mouse X") * sensitivityX;

				rotationY += Input.GetAxisRaw ("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
		
				transform.localEulerAngles = new Vector3 (-rotationY, rotationX, 0);
		}
        else if (axes == RotationAxes.MouseX)
        {
                rotationX += Input.GetAxisRaw("Mouse X") * sensitivityX;

                transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else
        {
				rotationY += Input.GetAxisRaw ("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				transform.localEulerAngles = new Vector3 (-rotationY, -rotationX, 0);
		}
		
	}
}








