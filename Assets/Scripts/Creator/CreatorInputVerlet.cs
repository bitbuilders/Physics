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
            physicsObject.springConstant = 1.0f;
			physicsObject.Initialize(collider, position, Vector2.zero, mass, damping);
            if (physicsObjectLink != null && !Input.GetKey(KeyCode.LeftShift))
            {
                physicsObject.AddLink(physicsObjectLink);
                physicsObject.restLength = (physicsObjectLink.position - physicsObject.position).magnitude;
            }
		}

        return physicsObject;
    }
}
