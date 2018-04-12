using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Integrator
{
    public static void ExplicitEuler(float dt, PhysicsObject physicsObject)
    {
        physicsObject.position = physicsObject.position + physicsObject.velocity * dt;
        physicsObject.velocity = physicsObject.velocity + (physicsObject.inverseMass * physicsObject.force) * dt;
    }

    public static void SemiImplicitEuler(float dt, PhysicsObject physicsObject)
    {
        physicsObject.velocity = physicsObject.velocity + (physicsObject.inverseMass * physicsObject.force) * dt;
        physicsObject.position = physicsObject.position + physicsObject.velocity * dt;
    }


}
