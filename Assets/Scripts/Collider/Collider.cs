using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collider
{
	public enum eType
	{
		POINT,
		SPHERE,
		AABB,
	}

	public PhysicsObject physicsObject { get; set; }

	abstract public void Draw(Vector2 position, Color color, float duration = 0.0f);
	abstract public bool Intersects(Collider other, ref Intersection.Result result);

	public static Collider Create(eType type, float size)
	{
		Collider collider = null;

		switch(type)
		{
			case eType.POINT:
				collider = new ColliderPoint();
				break;

			case eType.SPHERE:
				ColliderSphere colliderSphere = new ColliderSphere();
				colliderSphere.Initialize(size);
				collider = colliderSphere;
				break;

			case eType.AABB:
				ColliderAABB colliderAABB = new ColliderAABB();
				colliderAABB.Initialize(size);
				collider = colliderAABB;
				break;
		}

		return collider;
	}
}