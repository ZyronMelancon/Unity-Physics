using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothBehavior : MonoBehaviour {

    public GameObject particleObject;
    public int rows;
    public int columns;
    public int restPosition;
    public int constant;
    public int damping;

    List<HookesLaw.Particle> particles;
    List<HookesLaw.SpringDamper> dampers;
    List<GameObject> particleObjects;

    // Use this for initialization
    void Start()
    {
        particles = new List<HookesLaw.Particle>();
        particleObjects = new List<GameObject>();
        dampers = new List<HookesLaw.SpringDamper>();

        //Create all particles and gameobjects
        for (int i = 0; i < columns; i++)
            for (int o = 0; o < rows; o++)
            {
                GameObject par = Instantiate(particleObject);
                par.transform.position = new Vector3(0, -i * rows * restPosition, -o * columns * restPosition);
                particleObjects.Add(par);
                HookesLaw.Particle par2 = new HookesLaw.Particle(par.transform.position);
                par2.useGravity = true;
                particles.Add(par2);
            }

        particles[0].useGravity = false;
        particles[columns-1].useGravity = false;

        //Create all dampers
        for (int i = 0; i < columns; i++)
            for (int o = 0; o < rows; o++)
            {
                dampers.Add(new HookesLaw.SpringDamper(particles[o + i], particles[o+1 + i], 5, restPosition, 10));
            }
    }
	
	// Update is called once per frame
	void Update ()
    {
        foreach (var i in dampers)
        {
            Debug.DrawLine(i.p1pos(), i.p2pos(), Color.white);
            i.CalculateForce();
        }

        foreach (var i in particles)
            if(i.useGravity)
                i.Update();

        for (int i = 0; i < particles.Count; i++)
            particleObjects[i].transform.position = particles[i].position;
	}
}
