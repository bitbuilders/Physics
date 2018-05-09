using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorInputRandom : Creator
{
    float speed = 15.0f;
    float creationRate = 0.05f;
    float creationTime = 0.0f;

	public override PhysicsObject Update(float dt)
    {
        PhysicsObject physicsObject = null;

        creationTime += dt;
        if (Input.GetMouseButton(0) && creationTime > creationRate)
        {
            creationTime = 0.0f;
			physicsObject = new PhysicsObject();
			Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider collider = Collider.Create(type, size);
            Vector2 velocity = Random.insideUnitCircle * speed;
			physicsObject.Initialize(collider, position, velocity, mass, damping);
		}

        return physicsObject;
    }
}
