using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Characters.Player;
using Core.Map;
#if UNITY_EDITOR
using UnityEditor;
#endif



namespace Utilities.UI
{
    public class NPC : MonoBehaviour
    {
        private string kSFXPAth = "Prefabs/NPC/BlurredSFX";
        private Renderer _renderer;
        private DayNight _dayNight;
        private DuplicateSprite _duplicate;
        private int layer;
        private int order;

        public DuplicateSprite DuplicateSprite
        {
            get { return _duplicate; }
        }

        // Use this for initialization
        private void Awake()
        {
            _duplicate = GetComponentInChildren<DuplicateSprite>();
            _renderer = GetComponentInChildren<Renderer>();
            _renderer.sortingLayerID = PlayerBehaviour.CurrentPlayer.Renderer.sortingLayerID;
            layer = gameObject.layer;
            order = _renderer.sortingOrder;

            _renderer.transform.position = new Vector3(_renderer.transform.position.x,
                   _renderer.transform.position.y,
                   -2f);
        }

        public void AddDuplicate()
        {
            //_renderer = GetComponentInChildren<SpriteRenderer>();
            //_renderer.sortingOrder = 2;
            //var sfx = Instantiate(Resources.Load<GameObject>(kSFXPAth), transform);
            //sfx.GetComponent<SpriteRenderer>().sprite = _renderer.sprite;
            //sfx.transform.localPosition = Vector2.zero;
            //sfx.transform.localScale = new Vector2(1.1f, 1.1f);
        }

        // Update is called once per frame
        void Update()
        {
            if (PlayerBehaviour.CurrentPlayer.isActiveAndEnabled && transform.position.y > PlayerBehaviour.CurrentPlayer.transform.position.y)
            {
                _renderer.sortingOrder = PlayerBehaviour.CurrentPlayer.Renderer.sortingOrder - 1;
               
            }
            else
            {
                _renderer.sortingOrder = PlayerBehaviour.CurrentPlayer.Renderer.sortingOrder + 1;
               
            }
        }

        private void OnDisable()
        {
            _renderer.transform.position = new Vector3(_renderer.transform.position.x,
                   _renderer.transform.position.y,
                   0f);
            _renderer.gameObject.layer = layer;
            _renderer.sortingOrder = order;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(NPC))]
    [CanEditMultipleObjects]
    class DecalMeshHelperEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("AddSFXDuplicate"))
                (target as NPC).AddDuplicate();
        }
    }
#endif
}
