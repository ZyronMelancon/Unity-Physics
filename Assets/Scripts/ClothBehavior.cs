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
                par.transform.position = new Vector3(0, -i * restPosition, -o * restPosition);
                particleObjects.Add(par);
                HookesLaw.Particle par2 = new HookesLaw.Particle(par.transform.position);
                par2.useGravity = true;

                particles.Add(par2);
            }

        for (int i = 0;i < columns; i++)
            particles[i*columns].useGravity = false;

        //Create all dampers
        for (int i = 0; i < columns; i++)
            for (int o = 0; o < rows; o++)
            {
                //Row
                if (o < rows - 1) 
                    dampers.Add(new HookesLaw.SpringDamper(particles[o + (i * columns)],
                        particles[o + 1 + (i * columns)], constant, restPosition, damping));
                //Double space row
                if (o < rows - 2) 
                    dampers.Add(new HookesLaw.SpringDamper(particles[o + (i * columns)],
                        particles[o + 2 + (i * columns)], constant, restPosition * 2, damping));
                //Col
                if (i < columns - 1) 
                    dampers.Add(new HookesLaw.SpringDamper(particles[i * rows + o], 
                        particles[i * rows + rows + o], constant, restPosition, damping));
                //Double space col
                if (i < columns - 2) 
                    dampers.Add(new HookesLaw.SpringDamper(particles[i * rows + o],
                        particles[i * rows + (rows * 2) + o], constant, restPosition * 2, damping));
                //Diagonal TL-BR
                if (i < columns-1 && o < rows-1) 
                    dampers.Add(new HookesLaw.SpringDamper(particles[i * rows + o],
                        particles[i * rows + rows + o + 1], constant, restPosition * 1.41f, damping));
                //Diagonal TR-BL
                if (i < columns - 1 && o < rows && o > 0) 
                    dampers.Add(new HookesLaw.SpringDamper(particles[i * rows + o],
                        particles[i * rows + rows + o - 1], constant, restPosition * 1.41f, damping));
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
            if (i.useGravity)
            {
                i.AddForce(new Vector3(-40, 0, 0));
                i.Update();
            }

        for (int i = 0; i < particles.Count; i++)
        {            
            particleObjects[i].transform.position = particles[i].position;
        }
    }
}
