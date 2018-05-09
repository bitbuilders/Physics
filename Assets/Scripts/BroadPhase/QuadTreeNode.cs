using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTreeNode
{
    List<PhysicsObject> m_objects;
    AABB m_boundary;
    int m_capacity;
    QuadTreeNode m_northEast = null;
    QuadTreeNode m_northWest = null;
    QuadTreeNode m_southEast = null;
    QuadTreeNode m_southWest = null;

    public QuadTreeNode(AABB boundary, int capacity)
    {
        m_objects = new List<PhysicsObject>();
        m_boundary = boundary;
        m_capacity = capacity;
    }

    public bool Insert(PhysicsObject physicsObject)
    {
        if (!m_boundary.Contains(physicsObject.position))
            return false;

        if (m_objects.Count < m_capacity)
        {
            m_objects.Add(physicsObject);
            return true;
        }

        if (m_northWest == null)
            Subdivide();

        if (m_northWest.Insert(physicsObject)) return true;
        if (m_northEast.Insert(physicsObject)) return true;
        if (m_southWest.Insert(physicsObject)) return true;
        if (m_southEast.Insert(physicsObject)) return true;

        return false;
    }

    public void Draw(Color color, float duration = 0)
    {
        m_boundary.Draw(color, duration);
        if (m_northWest != null) m_northWest.Draw(color, duration);
        if (m_northEast != null) m_northEast.Draw(color, duration);
        if (m_southWest != null) m_southWest.Draw(color, duration);
        if (m_southEast != null) m_southEast.Draw(color, duration);
    }

    public void Query(AABB range, ref List<PhysicsObject> physicsObjects)
    {
        if (!m_boundary.Intersects(range))
            return;

        for (int i = 0; i < m_objects.Count; ++i)
        {
            if (range.Contains(m_objects[i].position))
                physicsObjects.Add(m_objects[i]);
        }

        if (m_northWest == null)
            return;

        m_northWest.Query(range, ref physicsObjects);
        m_northEast.Query(range, ref physicsObjects);
        m_southWest.Query(range, ref physicsObjects);
        m_southEast.Query(range, ref physicsObjects);
    }

    private void Subdivide()
    {
        Vector2 c = m_boundary.center;
        Vector2 s = m_boundary.size * 0.25f;
        m_northWest = new QuadTreeNode(new AABB(c + new Vector2(-s.x, s.y), m_boundary.size * 0.5f), m_capacity);
        m_northEast = new QuadTreeNode(new AABB(c + new Vector2(s.x, s.y), m_boundary.size * 0.5f), m_capacity);
        m_southWest = new QuadTreeNode(new AABB(c + new Vector2(-s.x, -s.y), m_boundary.size * 0.5f), m_capacity);
        m_southEast = new QuadTreeNode(new AABB(c + new Vector2(s.x, -s.y), m_boundary.size * 0.5f), m_capacity);
    }
}
