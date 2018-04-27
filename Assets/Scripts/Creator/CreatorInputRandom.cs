using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorInputRandom : Creator
{
	public PhysicsObject UpdateCreator(float dt)
    {
        PhysicsObject physicsObject = null;

        if (Input.GetMouseButton(0))
        {
			physicsObject = new PhysicsObject();
			Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider collider = Collider.Create(type, size);
            Vector2 velocity = Random.insideUnitCircle * 10.0f;
			physicsObject.Initialize(collider, position, velocity, mass, damping);
		}

        return physicsObject;
    }
}
