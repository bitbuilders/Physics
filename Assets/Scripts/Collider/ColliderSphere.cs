using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSphere : Collider
{
    const int RENDER_STEPS = 20;

	public float radius { get; set; }
	public Vector2 center { get { return physicsObject.position; } }

	public void Initialize(float radius)
	{
		this.radius = radius;
	}

    public override void Draw(Vector2 position, Color color, float duration = 0.0f)
    {
		// get the angle size step to draw a circle
		float angle = (Mathf.PI * 2.0f) / RENDER_STEPS;
		for (int i = 0; i < RENDER_STEPS; i++)
		{
			// draw  a line from the current angle to the next angle
			// 
			Vector2 offset1 = Vector2.zero;
            // use the cos() of i * angle to get the offset1.x value
            // use the sin() of i * angle to get the offset1.y value
            offset1.x = Mathf.Cos(i * angle);
            offset1.y = Mathf.Sin(i * angle);

            Vector2 offset2 = Vector2.zero;
            // this is simliar to offset1 but you'll use (i + 1) * angle to get the next angle in our circle
            offset2.x = Mathf.Cos((i + 1) * angle);
            offset2.y = Mathf.Sin((i + 1) * angle);

            // hint: you'll need to multiply the offset by the radius to get the correct size circle
            // draw a line from position + offset1 to position + offset2
            Debug.DrawLine(position + offset1 * radius, position + offset2 * radius, color, duration);
        }
    }

	public override bool Intersects(Collider other, ref Intersection.Result result)
	{
		bool intersects = false; 
		if (other.GetType() == typeof(ColliderSphere))
		{
			intersects = Intersection.Intersects(this, (ColliderSphere)other, ref result);
		}
		else if (other.GetType() == typeof(ColliderAABB))
		{
			intersects = Intersection.Intersects((ColliderAABB)other, this, ref result);
		}
		else if (other.GetType() == typeof(ColliderPoint))
		{
			intersects = Intersection.Intersects(this, (ColliderPoint)other, ref result);
		}
		
		return intersects;
	}
}
