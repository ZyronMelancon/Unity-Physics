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
    public bool lockLeft;
    public bool lockRight;
    public bool lockBottom;
    public bool lockTop;
    public bool genX;
    public bool genInforce;
    public float windDirX = 0;
    public float windDirY = 0;
    public float windDirZ = 0;

    List<HookesLaw.SpringDamper> dampers;
    public List<ParticleBehavior> particleObjects;

    // Use this for initialization
    void Start()
    {
        particleObjects = new List<ParticleBehavior>();
        dampers = new List<HookesLaw.SpringDamper>();

        //GenerateParticles();
        //GenerateDampers();
        //Relock();
    }

    public void GenerateParticles()
    {
        //If there is anything already in place, remove all to reset
        if (particleObjects.Count > 0)
        {
            for (int i = dampers.Count - 1; i >= 0; i--)
                dampers.Remove(dampers[i]);
            for (int i = particleObjects.Count - 1; i >= 0; i--)
            {
                Destroy(particleObjects[i].gameObject);
                particleObjects.Remove(particleObjects[i]);
            }
        }

        //Create all particles and gameobjects
        for (int o = 0; o < rows; o++)
            for (int i = 0; i < columns; i++)
            {
                GameObject par = Instantiate(particleObject);
                par.transform.position = new Vector3(0, -o * restPosition, -i * restPosition);
                par.GetComponent<ParticleBehavior>().particle = new HookesLaw.Particle(par.transform.position);
                par.GetComponent<ParticleBehavior>().particle.position = par.transform.position;
                par.transform.parent = transform;
                particleObjects.Add(par.GetComponent<ParticleBehavior>());
            }
    }

    public void Relock()
    {
        foreach (var i in particleObjects)
            i.particle.useGravity = true;

        if (lockLeft)
            for (int i = 0; i < rows; i++)
                particleObjects[i * columns].particle.useGravity = false;
        if (lockRight)
            for (int i = 0; i < rows; i++)
                particleObjects[i * columns + columns - 1].particle.useGravity = false;
        if (lockTop)
            for (int i = 0; i < columns; i++)
                particleObjects[i].particle.useGravity = false;
        if (lockBottom)
            for (int i = 0; i < columns; i++)
                particleObjects[columns * (rows - 1) + i].particle.useGravity = false;
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
                {
                    dampers.Add(new HookesLaw.SpringDamper(particleObjects[r + c * columns].particle,
                        particleObjects[r + 1 + (c * columns)].particle, constant, restPosition, damping));
                }
                //Col
                if (c < rows-1 )
                    dampers.Add(new HookesLaw.SpringDamper(particleObjects[r + c * columns].particle,
                        particleObjects[r + c * columns + columns].particle, constant, restPosition, damping));

                if (genInforce)
                {
                    //Row Reinforcement
                    if (r < columns - 2)
                        dampers.Add(new HookesLaw.SpringDamper(particleObjects[r + c * columns].particle,
                            particleObjects[r + 2 + (c * columns)].particle, constant, restPosition * 2, damping));
                    //Col Reinforcement
                    if (c < rows - 2)
                        dampers.Add(new HookesLaw.SpringDamper(particleObjects[r + c * columns].particle,
                            particleObjects[r + c * columns + columns * 2].particle, constant, restPosition * 2, damping));
                }
                
                if (genX)
                {
                    //Diagonal TL-BR
                    if (c < rows - 1 && r < columns - 1)
                        dampers.Add(new HookesLaw.SpringDamper(particleObjects[r + c * columns].particle,
                            particleObjects[r + c * columns + columns + 1].particle, constant, restPosition * 1.41f, damping));
                    //Diagonal TR-BL
                    if (c < rows - 1 && r < columns && r > 0)
                        dampers.Add(new HookesLaw.SpringDamper(particleObjects[r + c * columns].particle,
                            particleObjects[r + c * columns + columns - 1].particle, constant, restPosition * 1.41f, damping));
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
        for (int i = 0; i < particleObjects.Count - columns - 1; i++)
            if (i / columns != 1)
            {
                //TL, TR, BL
                CalculateWind(particleObjects[i].particle, particleObjects[i + 1].particle, particleObjects[i + columns].particle, new Vector3(windDirX, windDirY, windDirZ), 1, 1);
                //TR, BL, BR
                CalculateWind(particleObjects[i + 1].particle, particleObjects[i + columns].particle, particleObjects[i + columns + 1].particle, new Vector3(windDirX, windDirY, windDirZ), 1, 1);
            }

        foreach (var i in dampers)
        {
            Debug.DrawLine(i.p1pos(), i.p2pos(), 
                Color.HSVToRGB(1-Mathf.Min(i.p1.velocity.magnitude/10, 1),0.75f,1));
            i.CalculateForce();
        }

        //foreach (var i in particles)
        //    if (i.useGravity)
        //    {
        //        i.Update();
        //    }
    }
}
