using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorInputVerlet : Creator
{
	public override PhysicsObject Update(float dt)
    {
        PhysicsObjectSpringVerlet physicsObject = null;

        if (Input.GetMouseButtonDown(0))
        {
			physicsObject = new PhysicsObjectSpringVerlet();
			Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider collider = Collider.Create(type, size);
            physicsObject.springConstant = 10.0f;
            physicsObject.restLength = restLength;
            physicsObject.AddLink(physicsObjectLink);
			physicsObject.Initialize(collider, position, Vector2.zero, mass, damping);
		}

        return physicsObject;
    }
}
