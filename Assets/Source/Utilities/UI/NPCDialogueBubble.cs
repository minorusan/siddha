using Core.Characters.AI;
using Core.Characters.Player;
using Core.Map;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class NPCDialogueBubble : MonoBehaviour
    {
        private ArtificialIntelligence _owner;
        private Image _image;
        private Text _text;

        #region Monobehaviour

        private void Start()
        {
            _image = GetComponent<Image>();
            _text = GetComponentInChildren<Text>();
            _owner = transform.parent.GetComponentInParent<ArtificialIntelligence>();
        }
        // Update is called once per frame
        private void Update()
        {
            var active = MapController.GetNodeByPosition(PlayerBehaviour.CurrentPlayer.transform.position) !=
                         null;
            _image.enabled = active;
            _text.enabled = active;
        }

        #endregion
    }

}
