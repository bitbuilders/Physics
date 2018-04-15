using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorInputVelocity : Creator
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
            // create physics object
            // position = start position
            // velocity = start position - position
            // collider
            // use the velocity in the physics object Initialize function
            //Debug.Log(m_startPosition);
            physicsObject = new PhysicsObject();
            Vector2 pos = m_startPosition;
            Vector2 currentPos = m_currentPosition;
            Vector2 velocity = (m_startPosition - currentPos) * 2.0f;
            Collider collider = Collider.Create(type, size);
            physicsObject.Initialize(collider, pos, velocity, mass, damping);
        }
        else if (Input.GetMouseButton(0))
        {
            m_currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.DrawLine(m_startPosition, m_currentPosition, Color.blue);
        }

        return physicsObject;
    }
}
