using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVHNode
{
    List<PhysicsObject> m_objects;
    AABB m_boundary;
    BVHNode m_left = null;
    BVHNode m_right = null;

    public BVHNode(List<PhysicsObject> objects)
    {
        m_objects = new List<PhysicsObject>(objects);
        ComputeBoundary();
        Split();
    }

    private void ComputeBoundary()
    {
        if (m_objects.Count > 0)
        {
            Vector2 min = m_objects[0].position;
            Vector2 max = min;

            for (int i = 1; i < m_objects.Count; ++i)
            {
                Vector2 pos = m_objects[i].position;
                if (pos.x < min.x) min.x = pos.x;
                if (pos.x > max.x) max.x = pos.x;
                if (pos.y < min.y) min.y = pos.y;
                if (pos.y > max.y) max.y = pos.y;
            }

            Vector2 center = (min + max) / 2.0f;
            Vector2 size = new Vector2(max.x - min.x + 1.0f, max.y - min.y + 1.0f);
            m_boundary = new AABB(center, size);
        }
    }

    private void Split()
    {
        if (m_objects.Count > 1)
        {
            List<PhysicsObject> sorted = new List<PhysicsObject>(m_objects);
            m_objects.Sort((a, b) => a.position.x.CompareTo(b.position.x));
            int half = sorted.Count / 2;
            List<PhysicsObject> left = m_objects.GetRange(0, half);
            List<PhysicsObject> right = m_objects.GetRange(half, sorted.Count - half);
            m_left = new BVHNode(left);
            m_right = new BVHNode(right);

            m_objects.Clear();
        }
    }

    public void Draw(Color color, float duration = 0)
    {
        m_boundary.Draw(color, duration);

        if (m_left == null)
            return;

        m_left.Draw(color, duration);
        m_right.Draw(color, duration);
    }

    public void Query(AABB range, ref List<PhysicsObject> physicsObjects)
    {
        if (!m_boundary.Intersects(range))
            return;

        if (m_objects.Count == 1)
        {
            physicsObjects.Add(m_objects[0]);
            return;
        }

        if (m_left == null)
            return;

        m_left.Query(range, ref physicsObjects);
        m_right.Query(range, ref physicsObjects);
    }
}
