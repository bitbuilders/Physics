﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Integrator
{
    public static void ExplicitEuler(float dt, PhysicsObject physicsObject)
    {
        physicsObject.acceleration = physicsObject.force * physicsObject.inverseMass;
        physicsObject.position = physicsObject.position + physicsObject.velocity * dt;
        physicsObject.velocity = physicsObject.velocity + (physicsObject.inverseMass * physicsObject.force) * dt;
        physicsObject.velocity = physicsObject.velocity * Mathf.Pow(physicsObject.damping, dt);
        physicsObject.velocity = physicsObject.velocity + physicsObject.acceleration * dt;
    }

    public static void SemiImplicitEuler(float dt, PhysicsObject physicsObject)
    {
        physicsObject.acceleration = physicsObject.force * physicsObject.inverseMass;
        physicsObject.velocity = physicsObject.velocity + (physicsObject.inverseMass * physicsObject.force) * dt;
        physicsObject.velocity = physicsObject.velocity * Mathf.Pow(physicsObject.damping, dt);
        physicsObject.position = physicsObject.position + physicsObject.velocity * dt;
        physicsObject.velocity = physicsObject.velocity + physicsObject.acceleration * dt;
    }
}
