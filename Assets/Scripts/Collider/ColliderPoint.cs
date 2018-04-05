using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPoint : Collider
{
    const float RENDER_SIZE = 0.2f;
	public Vector2 point { get { return physicsObject.position; } }
	
    public override void Draw(Vector2 position, Color color, float duration = 0.0f)
    {
        // draw a cross ( + ) for the point collider, use the RENDER_SIZE as the size of the cross
        // (horizontal line) Debug.DrawLine(position + left vector * RENDERSIZE, position + right vector * RENDERSIZE, color, duration);
        // (vertical line) ...
        // hint: use the defined Vector2.left, Vector2.right, Vector2.up, Vector2.down 

        Debug.DrawLine(position + (Vector2.left * RENDER_SIZE), position + (Vector2.right * RENDER_SIZE), color, duration);

        Debug.DrawLine(position + (Vector2.up * RENDER_SIZE), position + (Vector2.down * RENDER_SIZE), color, duration);
    }

	public override bool Intersects(Collider other, ref Intersection.Result result)
	{
		bool intersects = false;
		if (other.GetType() == typeof(ColliderPoint))
		{
			intersects = Intersection.Intersects(this, (ColliderPoint)other, ref result);
		}
		else if (other.GetType() == typeof(ColliderSphere))
		{
			intersects = Intersection.Intersects((ColliderSphere)other, this, ref result);
		}
		else if (other.GetType() == typeof(ColliderAABB))
		{
			intersects = Intersection.Intersects((ColliderAABB)other, this, ref result);
		}

		return intersects;
	}
}
