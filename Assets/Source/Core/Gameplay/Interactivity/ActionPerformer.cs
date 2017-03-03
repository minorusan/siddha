using UnityEngine;
using UnityEngine.UI;


namespace Core.Gameplay.Interactivity
{
	[RequireComponent (typeof(Button))]
	public class ActionPerformer : MonoBehaviour
	{
		private static ActionPerformer _instance;
		private ActionBase _currentAction;
		private GameObject _currentActionSetter;
		private Button _button;

		public static ActionPerformer Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<ActionPerformer> ();
				}
				return _instance;
			}
		}

		private void OnDestroy ()
		{
			_instance = null;
		}

		private void Start ()
		{
			_button = GetComponent <Button> ();
			_button.enabled = false;
			_button.image.sprite = null;
		}

		public void SetAction (ActionBase action, GameObject setter = null, Sprite image = null)
		{
			if (action != null)
			{
				_currentAction = action;
				_button.enabled = true;
				_button.image.sprite = image == null? action.ActionImage : image;
				_button.image.color = Color.white;
				_currentActionSetter = setter;
				_button.interactable = action.IsRequirementSatisfied (setter);
			}
			else
			{
				_button.enabled = false;
				_button.image.color = new Color (0f, 0f, 0f, 0f);
				_currentAction = null;
				_currentActionSetter = null;
				_button.image.sprite = null;
			}
		}

		public void PerformAction ()
		{
			_currentAction.PerformAction (_currentActionSetter);
		}
	}
}

