using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspector : MonoBehaviour
{
	[SerializeField] public float m_size = 0.0f;
	[SerializeField] public float m_mass = 0.0f;
	[SerializeField] public float m_damping = 0.0f;
	[SerializeField] public float m_springConstant = 0.0f;
	[SerializeField] public float m_restLength = 0.0f;

	PhysicsObject m_physicsObject { get; set; }

	Simulator m_simulator = null;

	private void Awake()
	{
		m_simulator = GetComponent<Simulator>();
	}

	private void Update()
	{
		// check for physics object selection
		if (Input.GetMouseButtonDown(1))
		{
			Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider collider = Collider.Create(Collider.eType.POINT, 0.2f);

			PhysicsObject pointPhysicsObject = new PhysicsObject();
			pointPhysicsObject.Initialize(collider, position, Vector2.zero, 0.0f, 0.0f);
			foreach (PhysicsObject physicsObject in m_simulator.m_physicsObjects)
			{
				Intersection.Result result = new Intersection.Result();
				bool intersects = pointPhysicsObject.Intersects(physicsObject, ref result);
				if (intersects)
				{
					Inspector inspector = GetComponent<Inspector>();
					inspector.Initialize(physicsObject);
					break;
				}
			}
		}

		// update physics object parameters
		if (m_physicsObject != null)
		{
			if (typeof(PhysicsObject).IsAssignableFrom(m_physicsObject.GetType()))
			{
				m_physicsObject.mass = m_mass;
				m_physicsObject.damping = m_damping;
			}
			if (m_physicsObject.GetType() == typeof(PhysicsObjectSpring))
			{
				PhysicsObjectSpring physicsObjectSpring = (PhysicsObjectSpring)m_physicsObject;
				physicsObjectSpring.springConstant = m_springConstant;
				physicsObjectSpring.restLength = m_restLength;
			}
		}
	}

	private void LateUpdate()
	{
		if (m_physicsObject != null)
		{
			m_physicsObject.Draw(Color.red);
		}
	}

	private void Initialize(PhysicsObject physicsObject)
	{
		this.m_physicsObject = physicsObject;
		if (physicsObject != null)
		{
			if (typeof(PhysicsObject).IsAssignableFrom(physicsObject.GetType()))
			{
				m_mass = physicsObject.mass;
				m_damping = physicsObject.damping;
			}
			if (physicsObject.GetType() == typeof(PhysicsObjectSpring))
			{
				PhysicsObjectSpring physicsObjectSpring = (PhysicsObjectSpring)physicsObject;
				m_springConstant = physicsObjectSpring.springConstant;
				m_restLength = physicsObjectSpring.restLength;
			}
		}
	}
}
