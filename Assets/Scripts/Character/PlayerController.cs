using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class PlayerController : MonoBehaviour
{
	private Character m_Character;

	// Start is called before the first frame update
	void Start()
	{
		m_Character = GetComponent<Character>();
	}

	// Update is called once per frame
	void Update()
	{
		Vector2 movDir = new Vector2 (
			Input.GetAxisRaw("Horizontal"),
			Input.GetAxisRaw("Vertical")
		);
		Vector2 lookDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

		bool sprint = Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift);

		m_Character.Move(movDir);
		m_Character.SetSprint(sprint);
		m_Character.SetLookDirection(lookDir);

		if(Input.GetMouseButtonDown(0))
			m_Character.Attack();
	}
}
