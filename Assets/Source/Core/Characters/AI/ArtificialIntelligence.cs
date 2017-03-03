using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;
using System.Linq;

using Core.Map;
using Utils;


namespace Core.Characters.AI
{
	[RequireComponent (typeof(MovableObject))]
	public abstract class ArtificialIntelligence : MonoBehaviour
	{
		#region Protected

		public EAIState BaseState;
		protected AIStateBase _currentState;
		protected Dictionary<EAIState, AIStateBase> _availiableStates = new Dictionary<EAIState, AIStateBase> ();
		protected MovableObject _movableObject;

		#endregion

		public MovableObject MovableObject
		{
			get
			{
				return _movableObject;
			}
		}

		public Text StatusText;

		#region Monobehaviour

		private void Awake ()
		{
			_movableObject = GetComponent <MovableObject> ();
			InitStates ();
		}

		private void OnDisable ()
		{
			_currentState.OnLeave ();
		
			_currentState = _availiableStates [_availiableStates.Keys.First ()];
			var effect = GameObject.FindObjectOfType<NoiseEffect> ();
			if (effect != null)
			{
				//effect.ChangeOpacity (0f);
			}
			_currentState.OnEnter ();
		}

		protected virtual void Start ()
		{
		}

		protected virtual void OnEnable ()
		{
			Debug.Assert (BaseState >= 0, this.name + " did not have a base state ID. CRASH LOUDLY");
           
			_currentState = _availiableStates.Count >= 1 ? _availiableStates [BaseState] : _availiableStates [_availiableStates.Keys.First ()];
			_currentState.OnEnter ();
		}

		protected virtual void Update ()
		{
			if (_currentState == null)
			{
				return;
			}

			if (_currentState.CurrentStateCondition == AIStateCondition.Done)
			{
				MoveToState (_currentState.PendingState);
			}
			else
			{
				_currentState.UpdateState ();
			}
		}

		#endregion

		protected abstract void InitStates ();

		public void MoveToState (EAIState pendingState)
		{
			if (_availiableStates [pendingState] != null)
			{
				_currentState.OnLeave ();
				_currentState = _availiableStates [pendingState];
				_currentState.OnEnter ();
			}
		}
	}
}

