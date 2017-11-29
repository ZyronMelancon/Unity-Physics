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

        public Particle(Vector3 pos)
        {
            position = pos;
            velocity = Vector3.zero;
            acceleration = Vector3.zero;
            mass = 1;
            force = Vector3.zero;
        }

        public void AddForce(Vector3 f)
        {
            force += f;
        }

        // Update is called once per frame
        public void Update()
        {
            acceleration = force / mass * Time.deltaTime;
            velocity += acceleration * Time.deltaTime;
            position += velocity * Time.deltaTime;
        }
    }

    public class SpringDamper
    {
        Particle p1, p2;
        float Ks;
        float Lo;

        public SpringDamper(Particle pone, Particle ptwo, float springConstant, float restLength)
        {
            p1 = pone;
            p2 = ptwo;
            Ks = springConstant;
            Lo = restLength;
        }

        public void Zoz()
        {
            float distance = Vector3.Magnitude(p1.position - p2.position);
            Vector3 normaldir = Vector3.Normalize(p1.position - p2.position);

            Vector3 correction = normaldir * (Lo - distance) * Ks;

            p1.AddForce(correction);
            p2.AddForce(-correction);
        }
    }
}