using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Intersection
{
	public struct Result
	{
		public float distance;
	}

	static float Distance(Vector2 p1, Vector2 p2)
	{
        // use the pythagorean theorem to get the distance
        // https://www.varsitytutors.com/hotmath/hotmath_help/topics/distance-formula.html

        Vector2 sub = p2 - p1;
        float distance = 0.0f;

        float horiz = sub.x * sub.x;
        float vert = sub.y * sub.y;

        distance = Mathf.Sqrt(horiz + vert);

		return distance;
	}

	static float DistanceSqr(Vector2 p1, Vector2 p2)
	{
        // use the pythagorean theorem to get the distance
        // https://www.varsitytutors.com/hotmath/hotmath_help/topics/distance-formula.html
        // for this distance function we don't use the square root as an optimization

        Vector2 sub = p2 - p1;
        float distance = 0.0f;

        float horiz = sub.x * sub.x;
        float vert = sub.y * sub.y;

        distance = horiz + vert;

        return distance;
	}
	
	static public bool Intersects(ColliderSphere collider1, ColliderSphere collider2, ref Result result)
	{
		// get the distance between the center of the two sphere
		// subtract the distance for the sum of the radius of both spheres
		// there is an intersection if the distance <= 0.0
		bool intersects = false;

        float distance = DistanceSqr(collider1.center, collider2.center);
        float sumRadius = collider1.radius + collider2.radius;

        sumRadius *= sumRadius;
        intersects = (distance - sumRadius) <= 0.0f;
		
		return (intersects);
	}

	static public bool Intersects(ColliderPoint collider1, ColliderPoint collider2, ref Result result)
	{
		// get the distance between the two points
		// check if the distance is 0
		bool intersects = false;

        intersects = collider1.point == collider2.point;

		return (intersects);
	}

	static public bool Intersects(ColliderSphere collider1, ColliderPoint collider2, ref Result result)
	{
		// this is very similiar to the sphere to sphere collision but with only one radius
		bool intersects = false;

        float distance = DistanceSqr(collider1.center, collider2.point);
        float sumRadius = collider1.radius;

        sumRadius *= sumRadius;
        intersects = (distance - sumRadius) <= 0.0f;

        return (intersects);
	}

	static public bool Intersects(ColliderAABB collider1, ColliderAABB collider2, ref Result result)
	{
		// check if the edges of one of the AABB are within the edges of the other
		// collider1.left >= collider2.left && collider1.left <= collider2.right...
		// https://hopefultoad.blogspot.com/2017/09/2d-aabb-collision-detection-and-response.html
		bool intersects = false;

        bool leftOver = collider1.left <= collider2.right && collider1.left >= collider2.left;
        bool topOver = collider1.top >= collider2.bottom && collider1.top <= collider2.top;
        bool rightOver = collider1.right >= collider2.left && collider1.right <= collider2.right;
        bool bottomOver = collider1.bottom <= collider2.top && collider1.bottom >= collider2.bottom;

        intersects = (leftOver || rightOver) && (topOver || bottomOver);

        return (intersects);
	}

	static public bool Intersects(ColliderAABB collider1, ColliderPoint collider2, ref Result result)
	{
		// check if the point collider is within the edges of the AABB collider
		bool intersects = false;



		return (intersects);
	}

	static public bool Intersects(ColliderAABB collider1, ColliderSphere collider2, ref Result result)
	{
		// https://gamedev.stackexchange.com/questions/96337/collision-between-aabb-and-circle
		// I used the technique at the bottom of this page

		bool intersects = false;

		return (intersects);
	}
}
