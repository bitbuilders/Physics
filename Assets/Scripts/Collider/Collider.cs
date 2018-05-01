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
        LINE,
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

    public static Collider Create(eType type, Vector2 point1, Vector2 point2)
    {
        Collider collider = null;

        ColliderLine colliderLine = new ColliderLine();
        colliderLine.Initialize(point1, point2);
        collider = colliderLine;

        return collider;
    }
}