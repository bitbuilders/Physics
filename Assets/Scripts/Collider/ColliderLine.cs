using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderLine : Collider
{
	public float size { get; set; }

	public Vector2 center { get; private set; }
	public Vector2 point1 { get; private set; }
	public Vector2 point2 { get; private set; }
	public Vector2 normal { get; private set; }

    public void Initialize(Vector2 point1, Vector2 point2)
	{
        this.point1 = point1;
        this.point2 = point2;
        center = (point1 - point2) * 0.5f + point2;
        Vector2 line = (point1 - point2);
        normal = new Vector2(line.y, -line.x).normalized;
    }

    public override void Draw(Vector2 position, Color color, float duration = 0.0f)
    {
        Debug.DrawLine(center, center + normal, Color.yellow);
        Debug.DrawLine(point1, point2, color);
    }

	public override bool Intersects(Collider other, ref Intersection.Result result)
	{
		bool intersects = false;
		if (other.GetType() == typeof(ColliderLine))
		{
			intersects = Intersection.Intersects(this, (ColliderLine)other, ref result);
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
