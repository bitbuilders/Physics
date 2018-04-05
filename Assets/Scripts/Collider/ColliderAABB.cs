using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderAABB : Collider
{
	public float size { get; set; }

	public Vector2 center { get { return physicsObject.position; } }
	public Vector2 min { get { return new Vector2(left, top); } }
	public Vector2 max { get { return new Vector2(right, bottom); } }

	public float left { get { return physicsObject.position.x - size; } }
	public float right { get { return physicsObject.position.x + size; } }
	public float top { get { return physicsObject.position.y + size; } }
	public float bottom { get { return physicsObject.position.y - size; } }

	public void Initialize(float size)
	{
		this.size = size;
	}

    public override void Draw(Vector2 position, Color color, float duration = 0.0f)
    {
        // draw 4 lines from each corner to the next corner to form the aabb
        // hint: to create a corner Vector2 for the draw line function, you can use new Vector2(left, top)...

        Vector2 leftLineBottom = new Vector2(left, bottom);
        Vector2 leftLineTop = new Vector2(left, top);

        Vector2 topLineLeft = new Vector2(left, top);
        Vector2 topLineRight = new Vector2(right, top);

        Vector2 rightLineBottom = new Vector2(right, bottom);
        Vector2 rightLineTop = new Vector2(right, top);

        Vector2 bottomLineLeft = new Vector2(left, bottom);
        Vector2 bottomLineRight = new Vector2(right, bottom);

        Debug.DrawLine(leftLineBottom, leftLineTop, color, duration);
        Debug.DrawLine(topLineLeft, topLineRight, color, duration);
        Debug.DrawLine(rightLineBottom, rightLineTop, color, duration);
        Debug.DrawLine(bottomLineLeft, bottomLineRight, color, duration);
    }

	public override bool Intersects(Collider other, ref Intersection.Result result)
	{
		bool intersects = false;
		if (other.GetType() == typeof(ColliderAABB))
		{
			intersects = Intersection.Intersects(this, (ColliderAABB)other, ref result);
		}
		else if (other.GetType() == typeof(ColliderSphere))
		{
			intersects = Intersection.Intersects(this, (ColliderSphere)other, ref result);
		}
		else if (other.GetType() == typeof(ColliderPoint))
		{
			intersects = Intersection.Intersects(this, (ColliderPoint)other, ref result);
		}

		return intersects;
	}
}
