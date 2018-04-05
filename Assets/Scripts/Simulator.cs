using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour
{
	[SerializeField] Collider.eType m_type = Collider.eType.POINT;
	[SerializeField] [Range(0.1f,   5.0f)] float m_size = 1.0f;
	[SerializeField] [Range(0.0f, 100.0f)] float m_mass = 1.0f;

	List<PhysicsObject> m_physicsObjects = null;
    Creator m_creator = null;

    void Awake()
    {
        m_creator = new CreatorInputPoint();
        m_physicsObjects = new List<PhysicsObject>();
    }

    void Update()
    {
        float dt = Time.deltaTime;

		// set creator values
		m_creator.type = m_type;
		m_creator.size = m_size;
		m_creator.mass = m_mass;

		// create physics objects
        PhysicsObject newPhysicsObject = m_creator.Update(dt);
        if (newPhysicsObject != null)
        {
            m_physicsObjects.Add(newPhysicsObject);
        }

		// reset physics object collision state
        foreach (PhysicsObject physicsObject in m_physicsObjects)
        {
			physicsObject.m_collided = false;
        }

		// check collision detection
		Intersection.Result result = new Intersection.Result();
		for (int i = 0; i < m_physicsObjects.Count; i++)
		{
			for (int j = i + 1; j < m_physicsObjects.Count; j++)
			{
				bool intersects = m_physicsObjects[i].Intersects(m_physicsObjects[j], ref result);
				if (intersects)
				{
					m_physicsObjects[i].m_collided = true;
					m_physicsObjects[j].m_collided = true;
				}
			}
		}

		// draw physics objects
        foreach (PhysicsObject physicsObject in m_physicsObjects)
        {
			Color color = (physicsObject.m_collided) ? Color.red : Color.white;
			physicsObject.Draw(color);
		}
    }
}
