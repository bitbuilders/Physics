using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorInputSpring : Creator
{
	public override PhysicsObject Update(float dt)
    {
        PhysicsObjectSpring physicsObject = null;

        if (Input.GetMouseButtonDown(0))
        {
			physicsObject = new PhysicsObjectSpring();
			Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider collider = Collider.Create(type, size);
            physicsObject.springConstant = springConstant;
            physicsObject.restLength = restLength;
            if (physicsObjectLink != null)
            {
                physicsObject.AddLink(physicsObjectLink);
            }
			physicsObject.Initialize(collider, position, Vector2.zero, mass, damping);
		}

        return physicsObject;
    }
}
