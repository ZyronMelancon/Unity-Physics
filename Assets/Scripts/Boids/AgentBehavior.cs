using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBehavior : MonoBehaviour {

    Agent agent;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}

public class BoidBehavior : AgentBehavior
{
    IMoveable moveable;

    public void SetMoveable(IMoveable mover)
    {
        moveable = mover;
    }

    public void LateUpdate()
    {
        transform.position = moveable.Update_Agent(Time.deltaTime);
    }
}