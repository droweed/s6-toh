using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace gotoandplay
{
    public class GameView : MonoBehaviour
    {
        public TMP_InputField pegCountInput;

        private void Start()
        {
            
        }

        #region - button actions
        public void OnPlayPressed()
        {
            if(!string.IsNullOrEmpty(pegCountInput.text))
            {
                int pegCount = int.Parse(pegCountInput.text);
                pegCount = Mathf.Clamp(pegCount, 2, pegCount);
                pegCountInput.text = pegCount + ""; // update in case the value is not in clamped range

                GameController.Instance.StartGame(pegCount);
            }
        }
        #endregion
    }
}