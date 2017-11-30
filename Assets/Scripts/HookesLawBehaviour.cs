using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookesLawBehaviour : MonoBehaviour {

    public GameObject part1gm;
    public GameObject part2gm;
    public bool p1static;
    public bool p1gravity;
    public bool p2static;
    public bool p2gravity;
    
    HookesLaw.SpringDamper damper;
    HookesLaw.Particle part1;
    HookesLaw.Particle part2;
    
	// Use this for initialization
	void Start ()
    {
        part1 = new HookesLaw.Particle(part1gm.transform.position);
        part2 = new HookesLaw.Particle(part2gm.transform.position);
        damper = new HookesLaw.SpringDamper(part1, part2, 1f, 5, 10);
        if (p1gravity)
            part1.UseGravity();
        if (p2gravity)
            part2.UseGravity();
    }
	
	// Update is called once per frame
	void Update ()
    {
        damper.CalculateForce();
        if (!p1static)
            part1.Update();
        if(!p2static)
            part2.Update();

        part1gm.transform.position = part1.position;
        part2gm.transform.position = part2.position;
    }
}
