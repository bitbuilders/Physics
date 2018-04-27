using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorInputPoint : Creator
{
	public PhysicsObject UpdateCreator(float dt)
    {
        PhysicsObject physicsObject = null;

        if (Input.GetMouseButtonDown(0))
        {
			physicsObject = new PhysicsObject();
			Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider collider = Collider.Create(type, size);
			physicsObject.Initialize(collider, position, Vector2.zero, mass, damping);
		}

        return physicsObject;
    }
}
