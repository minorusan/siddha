using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Core.Map;


namespace Core.Characters.AI
{
	public enum AIStateCondition
	{
		Done,
		Active
	}

	public abstract class AIStateBase
	{
		protected readonly ArtificialIntelligence _masterBrain;
		protected MapController _map;
		protected AIStateCondition _currentCondition;
		protected EAIState _pendingState = EAIState.Empty;

		public EAIState State {
			get;
			protected set;
		}

		public AIStateCondition CurrentStateCondition {
			get
			{
				return _currentCondition;
			}
		}

		public EAIState PendingState {
			get
			{
				return _pendingState;
			}
		}

		public AIStateBase (ArtificialIntelligence brains)
		{
			_masterBrain = brains;
			_map = _masterBrain.MovableObject.Map;
		}

		public virtual void OnEnter ()
		{
			_currentCondition = AIStateCondition.Active;
			_masterBrain.MovableObject.CurrentPath.Nodes.Clear ();
		}

		public abstract void OnLeave ();

		public virtual void UpdateState ()
		{
			if (_map == null)
			{
				_map = _masterBrain.MovableObject.Map;
			}
		}
	}
}

