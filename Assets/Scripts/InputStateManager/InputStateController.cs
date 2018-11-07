using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputStateController : MonoBehaviour
{
	public IInputState m_state = null;
	private bool m_bRunbegin = false;
	private ActionControl _action;

	private void Awake()
	{
		_action = GetComponent<ActionControl>();
		SetState(new JoyStrickState(this, this.transform));
		_action.InputState = m_state;
	}

	private void Update()
	{
		StateUpdate();
		if (m_state != _action.InputState)
		{
			_action.InputState = m_state;
		}
	}

	public void SetState(IInputState state)
	{
		m_bRunbegin = false;
		
		if (m_state != null)
		{
			m_state.StateEnd();
		}

		m_state = state;
	}

	public void StateUpdate()
	{
		if (m_state != null && m_bRunbegin == false)
		{
			m_state.StateBegin();
			m_bRunbegin = true;
		}

		if (m_state != null)
		{
			m_state.StateUpdate();
		}
	}

	

}
