using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Utilities.UI
{
	[RequireComponent(typeof(Text))]
	public class FanfareMessage : MonoBehaviour
	{
		private Text _text;
		private Animator _animator;
		private static FanfareMessage _instance;
		// Use this for initialization
		void Start()
		{
			_text = GetComponent <Text>();
			_instance = this;
			_animator = GetComponent<Animator>();
			gameObject.SetActive(false);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		public static void ShowWithText(string text)
		{
			_instance.gameObject.SetActive(false);
			_instance.gameObject.SetActive(true);

			_instance._text.color = Color.white;
			_instance._text.text = text;
			_instance.gameObject.SetActive(true);
		}
	}
}

