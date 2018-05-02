using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorInputLine : Creator
{
    Vector2 m_startPosition;
    Vector2 m_currentPosition;

    public override PhysicsObject Update(float dt)
    {
        PhysicsObject physicsObject = null;

        if (Input.GetMouseButtonDown(0))
        {
            m_startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            physicsObject = new PhysicsObject();
            physicsObject.restitutionCoef = restitutionCoef;
            Vector2 currentPos = m_currentPosition;
            Vector2 velocity = Vector2.zero;
            Collider collider = Collider.Create(type, m_startPosition, currentPos);
            Vector2 pos = ((ColliderLine)collider).center;
            physicsObject.Initialize(collider, pos, velocity, 0.0f, damping);
        }
        else if (Input.GetMouseButton(0))
        {
            m_currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.DrawLine(m_startPosition, m_currentPosition, Color.white);
        }

        return physicsObject;
    }
}
