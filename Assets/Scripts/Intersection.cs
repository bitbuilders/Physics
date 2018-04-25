using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Intersection
{
	public struct Result
	{
		public float distance;
        public Vector2 contactNormal;
        public Collider collided1;
        public Collider collided2;
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

        result.distance = distance - sumRadius;
        result.contactNormal = (collider1.center - collider2.center).normalized;
        result.collided1 = collider1;
        result.collided2 = collider2;

        return (intersects);
	}

	static public bool Intersects(ColliderPoint collider1, ColliderPoint collider2, ref Result result)
	{
		// get the distance between the two points
		// check if the distance is 0
		bool intersects = false;

        intersects = collider1.point == collider2.point;

        result.distance = Distance(collider1.point, collider2.point);
        result.contactNormal = (collider1.point - collider2.point).normalized;
        result.collided1 = collider1;
        result.collided2 = collider2;

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

        result.distance = distance - sumRadius;
        result.contactNormal = (collider1.center - collider2.point).normalized;
        result.collided1 = collider1;
        result.collided2 = collider2;

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

        bool insideHoriz = collider2.point.x >= collider1.left && collider2.point.x <= collider1.right;
        bool insideVert = collider2.point.y >= collider1.bottom && collider2.point.y <= collider1.top;

        intersects = (insideHoriz && insideVert);

        return (intersects);
	}

	static public bool Intersects(ColliderAABB collider1, ColliderSphere collider2, ref Result result)
	{
		// https://gamedev.stackexchange.com/questions/96337/collision-between-aabb-and-circle
		// I used the technique at the bottom of this page

		bool intersects = false;

        Vector2 direction = collider1.center - collider2.center;
        float horiz = direction.x * direction.x;
        float vert = direction.y * direction.y;
        Vector2 directionNormal = direction / Mathf.Sqrt(horiz + vert);
        Vector2 point = directionNormal * collider2.radius; // The point closest to the center of the square (on the lane connecting them)
        Vector2 closestPosition = collider2.center + point;

        //Debug.DrawLine(collider2.center, closestPosition, Color.yellow);
        bool insideHoriz = closestPosition.x >= collider1.left && closestPosition.x <= collider1.right;
        bool insideVert = closestPosition.y >= collider1.bottom && closestPosition.y <= collider1.top;

        intersects = (insideHoriz && insideVert);

        return (intersects);
	}
}
