using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace gotoandplay
{
    public class MoveCountView : MonoBehaviour
    {
        public TextMeshProUGUI label;

        private int moveCount = 0;
        public int MoveCount
        {
            get
            {
                return moveCount;
            }
        }

        private void OnEnable()
        {
            Init();
        }

        private void Init()
        {
            UpdateLabel();
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnSubscribeEvents();
        }

        #region - event register/unregister methods
        private void SubscribeEvents()
        {
            if (GameController.Instance)
                GameController.Instance.evMoveAction.AddListener(EvMoveActionHandler);
        }

        private void UnSubscribeEvents()
        {
            if (GameController.Instance)
                GameController.Instance.evMoveAction.RemoveListener(EvMoveActionHandler);
        }
        #endregion

        private void EvMoveActionHandler()
        {
            moveCount++;
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            label.text = moveCount + "";
        }
    }
}