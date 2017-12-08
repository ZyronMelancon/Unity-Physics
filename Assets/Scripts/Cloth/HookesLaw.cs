using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HookesLaw
{
    public class Particle
    {
        public Vector3 position;
        public Vector3 velocity;
        Vector3 acceleration;
        public float mass;
        Vector3 force;
        public bool useGravity;

        public Particle(Vector3 pos)
        {
            position = pos;
            velocity = Vector3.zero;
            acceleration = Vector3.zero;
            mass = 1;
            force = Vector3.zero;
            useGravity = false;
        }

        public void AddForce(Vector3 f)
        {
            force += f;
        }

        public void UseGravity()
        {
            useGravity = true;
        }

        // Update is called once per frame
        public void Update()
        {
            acceleration = force / mass;
            velocity += acceleration * Time.fixedDeltaTime;
            if (useGravity == true)
                velocity += new Vector3(0, -9.81f, 0) * Time.fixedDeltaTime;
            position += velocity * Time.fixedDeltaTime;
            force = Vector3.zero;
        }
    }

    public class SpringDamper
    {
        public Particle p1, p2;
        float Ks;
        float Kd;
        float Lo;

        public SpringDamper(Particle pone, Particle ptwo, float springConstant, float restLength, float springDamping)
        {
            p1 = pone;
            p2 = ptwo;
            Ks = springConstant;
            Kd = springDamping;
            Lo = restLength;
        }

        public Vector3 p1pos()
        {
            return p1.position;
        }

        public Vector3 p2pos()
        {
            return p2.position;
        }

        public void CalculateForce()
        {
            Vector3 estar = p2.position - p1.position;
            float l = Vector3.Magnitude(estar);
            Vector3 e = Vector3.Normalize(estar);

            float v1 = Vector3.Dot(e, p1.velocity);
            float v2 = Vector3.Dot(e, p2.velocity);

            float Fsminusd = (-Ks * (Lo - l)) - (Kd * (v1-v2));

            Vector3 correction = Fsminusd * e;

            if(p1.useGravity)
                p1.AddForce(correction);
            if(p2.useGravity)
                p2.AddForce(-correction);
        }
    }
}