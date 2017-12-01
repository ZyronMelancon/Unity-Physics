using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLookAt : MonoBehaviour {

    public GameObject objectToLookAt;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(objectToLookAt.transform);
	}
}
