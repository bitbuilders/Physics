using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AABB
{
	public Vector2 center;
	public Vector2 size;
	
	public float left { get { return center.x - (size.x * 0.5f); } }
	public float right { get { return center.x + (size.x * 0.5f); } }
	public float top { get { return center.y + (size.y * 0.5f); } }
	public float bottom { get { return center.y - (size.y * 0.5f); } }
	
	public AABB(Vector2 center, Vector2 size)
	{
		this.center = center;
		this.size = size;
	}

	public bool Intersects(AABB aabb)
	{
		return (left <= aabb.right && right >= aabb.left) && (bottom <= aabb.top && top >= aabb.bottom);
	}

	public bool Contains(Vector2 point)
	{
		return (left <= point.x && right >= point.x) && (bottom <= point.y && top >= point.y);
	}

	public void Draw(Color color, float duration = 0.0f)
	{
		Debug.DrawLine(new Vector2(left, top), new Vector2(right, top), color, duration);
		Debug.DrawLine(new Vector2(right, top), new Vector2(right, bottom), color, duration);
		Debug.DrawLine(new Vector2(right, bottom), new Vector2(left, bottom), color, duration);
		Debug.DrawLine(new Vector2(left, bottom), new Vector2(left, top), color, duration);
	}
}
