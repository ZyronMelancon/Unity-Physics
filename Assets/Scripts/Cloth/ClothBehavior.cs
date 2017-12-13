﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothBehavior : MonoBehaviour {

    public GameObject particleObject;
    public int rows;
    public int columns;
    public int restPosition;
    public int constant;
    public int damping;
    public bool lockLeft;
    public bool lockRight;
    public bool lockBottom;
    public bool lockTop;
    public bool genX;
    public bool genInforce;
    public float windDirX = 0;
    public float windDirY = 0;
    public float windDirZ = 0;


    List<HookesLaw.Particle> particles;
    List<HookesLaw.SpringDamper> dampers;
    public List<GameObject> particleObjects;

    // Use this for initialization
    void Start()
    {
        particles = new List<HookesLaw.Particle>();
        particleObjects = new List<GameObject>();
        dampers = new List<HookesLaw.SpringDamper>();

        //GenerateParticles();
        //GenerateDampers();
        //Relock();
    }

    public void GenerateParticles()
    {
        //If there is anything already in place, remove all to reset
        if (particles.Count > 0)
        {
            for (int i = dampers.Count - 1; i >= 0; i--)
                dampers.Remove(dampers[i]);
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                Destroy(particleObjects[i]);
                particleObjects.Remove(particleObjects[i]);
                particles.Remove(particles[i]);
            }
        }

        //Create all particles and gameobjects
        for (int o = 0; o < rows; o++)
            for (int i = 0; i < columns; i++)
            {
                GameObject par = Instantiate(particleObject);
                par.transform.position = new Vector3(0, -o * restPosition, -i * restPosition);
                par.transform.parent = transform;
                particleObjects.Add(par);
                HookesLaw.Particle par2 = new HookesLaw.Particle(par.transform.position);
                par2.useGravity = true;

                particles.Add(par2);
            }
    }

    public void Relock()
    {
        foreach (var i in particles)
            i.useGravity = true;

        if (lockLeft)
            for (int i = 0; i < rows; i++)
                particles[i * columns].useGravity = false;
        if (lockRight)
            for (int i = 0; i < rows; i++)
                particles[i * columns + columns - 1].useGravity = false;
        if (lockTop)
            for (int i = 0; i < columns; i++)
                particles[i].useGravity = false;
        if (lockBottom)
            for (int i = 0; i < columns; i++)
                particles[columns * (rows - 1) + i].useGravity = false;
    }

    public void GenerateDampers()
    {
        //Remove all old dampers
        for (int i = dampers.Count - 1; i >= 0; i--)
            dampers.Remove(dampers[i]);

        //Create all dampers
        for (int c = 0; c < rows; c++)
            for (int r = 0; r < columns; r++)
            {
                //Row
                if (r < columns - 1)
                    dampers.Add(new HookesLaw.SpringDamper(particles[r + c * columns],
                        particles[r + 1 + (c * columns)], constant, restPosition, damping));
                //Col
                if (c < rows-1 )
                    dampers.Add(new HookesLaw.SpringDamper(particles[r + c * columns],
                        particles[r + c * columns + columns], constant, restPosition, damping));

                if (genInforce)
                {
                    //Row Reinforcement
                    if (r < columns - 2)
                        dampers.Add(new HookesLaw.SpringDamper(particles[r + c * columns],
                            particles[r + 2 + (c * columns)], constant, restPosition * 2, damping));
                    //Col Reinforcement
                    if (c < rows - 2)
                        dampers.Add(new HookesLaw.SpringDamper(particles[r + c * columns],
                            particles[r + c * columns + columns * 2], constant, restPosition * 2, damping));
                }
                
                if (genX)
                {
                    //Diagonal TL-BR
                    if (c < rows - 1 && r < columns - 1)
                        dampers.Add(new HookesLaw.SpringDamper(particles[r + c * columns],
                            particles[r + c * columns + columns + 1], constant, restPosition * 1.41f, damping));
                    //Diagonal TR-BL
                    if (c < rows - 1 && r < columns && r > 0)
                        dampers.Add(new HookesLaw.SpringDamper(particles[r + c * columns],
                            particles[r + c * columns + columns - 1], constant, restPosition * 1.41f, damping));
                }
            }
    }

    public void CalculateWind(HookesLaw.Particle p1, HookesLaw.Particle p2, HookesLaw.Particle p3, Vector3 windDir, float p, float cd)
    {
        Vector3 vsurface = p1.velocity + p2.velocity + p3.velocity / 3;
        Vector3 v = vsurface - windDir;
        Vector3 n = Vector3.Cross((p2.position - p1.position), (p3.position - p1.position)).normalized;
        float a = Vector3.Dot(v, n) / v.magnitude;

        Vector3 f = -.5f * p * (v.magnitude * v.magnitude) * cd * a * n;

        p1.AddForce(f / 3);
        p2.AddForce(f / 3);
        p3.AddForce(f / 3);
    }

    // Update is called once per frame
    void Update ()
    {
        for (int i = 0; i < particles.Count - columns -1; i++)
            if (i / columns != 1)
            {
                CalculateWind(particles[i], particles[i + 1], particles[i + columns], new Vector3(windDirX, windDirY, windDirZ), 1, 1);
                CalculateWind(particles[i + 1], particles[i + columns], particles[i + columns + 1], new Vector3(windDirX, windDirY, windDirZ), 1, 1);
            }

        foreach (var i in dampers)
        {
            Debug.DrawLine(i.p1pos(), i.p2pos(), 
                Color.HSVToRGB(1-Mathf.Min(i.p1.velocity.magnitude/10, 1),0.75f,1));
            i.CalculateForce();
        }

        foreach (var i in particles)
            if (i.useGravity)
            {
                i.Update();
            }

        for (int i = 0; i < particles.Count; i++)
        {            
            particleObjects[i].transform.position = particles[i].position;
        }
    }
}
