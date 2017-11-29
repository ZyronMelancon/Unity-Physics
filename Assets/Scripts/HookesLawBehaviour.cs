using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookesLawBehaviour : MonoBehaviour {

    public GameObject part1gm;
    public GameObject part2gm;
    
    HookesLaw.SpringDamper dampener;
    HookesLaw.Particle part1;
    HookesLaw.Particle part2;
    
	// Use this for initialization
	void Start ()
    {
        part1 = new HookesLaw.Particle(part1gm.transform.position);
        part2 = new HookesLaw.Particle(part2gm.transform.position);
        dampener = new HookesLaw.SpringDamper(part1, part2, 1, 0.1f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        dampener.Zoz();
        part1.Update();
        part2.Update();

        part1gm.transform.position = part1.position;
        part2gm.transform.position = part2.position;
    }
}
