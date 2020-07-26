using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PUNBT
{
    public class PlayerUI : MonoBehaviour
    {
        #region Public Properties

        [Tooltip("UI Text to display Player's Name")]
        public Text PlayerNameText;

        [Tooltip("UI Slider to display Player's Health")]
        public Slider PlayerHealthSlider;

        [Tooltip("Pixel offset from the player target")]
        public Vector3 ScreenOffset = new Vector3(0f, 30f, 0f);
        #endregion


        #region Private Properties

        private PlayerManager _target;
        float _characterControllerHeight = 0f;
        Transform _targetTransform;
        Renderer _targetRenderer;
        CanvasGroup _canvasGroup;
        Vector3 _targetPosition;

        #endregion


        #region MonoBehaviour Messages

        private void Awake()
        {
            this.GetComponent<Transform>().SetParent(GameObject.Find("Canvas").GetComponent<Transform>());
            _canvasGroup = this.GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            if (_target == null)
            {
                Destroy(this.gameObject);
                return;
            }
            if (PlayerHealthSlider != null)
            {
                PlayerHealthSlider.value = _target.Health;
            }
        }

        #endregion


        #region Public Methods

        public void SetTarget(PlayerManager target)
        {
            if(target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }

            _target = target;
            if(PlayerNameText != null)
            {
                PlayerNameText.text = _target.photonView.owner.NickName;
            }

            _targetTransform = _target.transform;
            _targetRenderer = _target.GetComponent<Renderer>();
            CharacterController _characterController = _target.GetComponent<CharacterController>();

            if (_characterController != null)
            {
                _characterControllerHeight = _characterController.height;
            }
        }

        private void LateUpdate()
        {
            if(_targetRenderer != null)
            {
                this._canvasGroup.alpha = _targetRenderer.isVisible ? 1f : 0f;
            }

            if(_targetTransform != null)
            {
                _targetPosition = _targetTransform.position;
                _targetPosition.y += _characterControllerHeight;
                this.transform.position = Camera.main.WorldToScreenPoint(_targetPosition) + ScreenOffset;
            }
        }

        #endregion
    }
}

