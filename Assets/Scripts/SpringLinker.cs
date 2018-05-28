using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringLinker : MonoBehaviour
{
    Simulator m_simulator;
    PhysicsObjectSpring m_physicsObjectFirst = null;
    PhysicsObjectSpring m_physicsObjectSecond = null;

    private void Awake()
    {
        m_simulator = GetComponent<Simulator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            SelectPhysicsObject(ref m_physicsObjectFirst);
        }
        else if (Input.GetMouseButtonUp(2))
        {
            SelectPhysicsObject(ref m_physicsObjectSecond);
            MakeLink();
        }
    }

    void SelectPhysicsObject(ref PhysicsObjectSpring output)
    {
        Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider collider = Collider.Create(Collider.eType.POINT, 0.2f);

        PhysicsObject pointPhysicsObject = new PhysicsObject();
        pointPhysicsObject.Initialize(collider, position, Vector2.zero, 0.0f, 0.0f);

        foreach (PhysicsObject physicsObject in m_simulator.m_physicsObjects)
        {
            if (physicsObject.GetType() == typeof(PhysicsObjectSpring) || physicsObject.GetType().IsSubclassOf(typeof(PhysicsObjectSpring)))
            {
                Intersection.Result result = new Intersection.Result();
                bool intersects = pointPhysicsObject.Intersects(physicsObject, ref result);
                if (intersects)
                {
                    output = (PhysicsObjectSpring)physicsObject;
                    break;
                }
            }
        }
    }

    void MakeLink()
    {
        if (m_physicsObjectFirst != null && m_physicsObjectSecond != null)
        {
            m_physicsObjectFirst.AddLink(m_physicsObjectSecond);
            m_physicsObjectSecond.AddLink(m_physicsObjectFirst);
            if (m_physicsObjectFirst.GetType() == typeof(PhysicsObjectSpringVerlet))
            {
                m_physicsObjectFirst.restLength = (m_physicsObjectSecond.position - m_physicsObjectFirst.position).magnitude;

            }
            if (m_physicsObjectSecond.GetType() == typeof(PhysicsObjectSpringVerlet))
            {
                m_physicsObjectSecond.restLength = (m_physicsObjectSecond.position - m_physicsObjectFirst.position).magnitude;
            }
            m_physicsObjectFirst = null;
            m_physicsObjectSecond = null;
        }
    }
}
