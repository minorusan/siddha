using UnityEngine;
using System.Collections;
using System.Linq;

using Utils;


namespace Core.Map
{
	[RequireComponent (typeof(BoxCollider2D))]
	public class RoomExit : MapDependentObject
	{
		#region Private

		private Bounds _bounds;
		private bool _initialised;

		#endregion

		public Bounds Bounds
		{
			get
			{
				return _bounds;
			}
		}

		#if UNITY_EDITOR
		[ReadOnly]
		#endif
		public EExitSide ExitSide;

		#if UNITY_EDITOR
		[ReadOnly]
		#endif
		public EExitSide LinksWithSide;
		#if UNITY_EDITOR
		[ReadOnly]
		#endif
		public RoomExit LinkedWith;

		#region Monobehaviour

		public void Init ()
		{
			ExitSide = GetExitSide ();
			LinksWithSide = GetLinkingSide ();
		}

		private void OnEnable ()
		{
			Init ();
		}

		private void OnDrawGizmos ()
		{
			if (!_initialised)
			{
				_initialised = true;
				Init ();
			}

			Gizmos.color = ExitSide == EExitSide.None || LinksWithSide == EExitSide.None ? Color.red : Color.cyan;
			Gizmos.DrawCube (_bounds.center, _bounds.size);
		}

		private EExitSide GetLinkingSide ()
		{
			switch (ExitSide)
			{
			case EExitSide.Down:
				{
					return  EExitSide.Top;
				}
			case EExitSide.Top:
				{
					return  EExitSide.Down;
				}
			case EExitSide.Left:
				{
					return  EExitSide.Right;
				}
			case EExitSide.Right:
				{
					return  EExitSide.Left;
				}
			default:
				{
					return  EExitSide.None;
				}
			}
		}

		private EExitSide GetExitSide ()
		{
			_bounds = GetComponent <BoxCollider2D> ().bounds;
			if (MapController.BottomEdgeNodesOfMap (Map).Any (n => _bounds.Contains (n.Position)))
			{
				return EExitSide.Down; 
			}

			if (MapController.TopEdgeNodesOfMap (Map).Any (n => _bounds.Contains (n.Position)))
			{
				return EExitSide.Top; 
			}

			if (MapController.LeftEdgeNodesOfMap (Map).Any (n => _bounds.Contains (n.Position)))
			{
				return EExitSide.Left; 
			}

			if (MapController.RightEdgeNodesOfMap (Map).Any (n => _bounds.Contains (n.Position)))
			{
				return EExitSide.Right; 
			}

			return EExitSide.None; 
		}

		#endregion
	}

	public enum EExitSide
	{
		None,
		Right,
		Left,
		Top,
		Down
	}
}
