using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public float panRate = 0.5f;
    public float rotateRate = 1f;
    public float zoomRate = 1;

	// Use this for initialization
	void Start ()
    {
		
	}

    Vector3 deltaMouse = new Vector3();
    Vector3 lastPos = new Vector3();
    Vector3 rotation = new Vector3();
    // Update is called once per frame
    void Update ()
    {
        Vector3 newPos = new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0);
        deltaMouse = lastPos - newPos;
        rotation += deltaMouse;
        if (Input.GetMouseButton(0))
        {
            transform.Rotate(rotation * rotateRate);
            float z = transform.eulerAngles.z;
            transform.Rotate(new Vector3(0, 0, -z));
        }
        if(Input.GetMouseButton(1))
        {
            transform.position += new Vector3(-rotation.y, rotation.x, 0);
        }

        lastPos = newPos;
	}
}
