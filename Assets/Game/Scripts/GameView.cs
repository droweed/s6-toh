using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace gotoandplay
{
    public class GameView : MonoBehaviour
    {
        public Slider diskCountSlider;
        public TextMeshProUGUI diskCountSliderLabel;

        public GameObject uiGameSetup;
        public GameObject uiGameHud;
        public GameObject uiLevelComplete;

        public TextMeshProUGUI levelCompleteDurationLabel;
        public TextMeshProUGUI levelCompleteMoveCountLabel;

        private void Start()
        {
            SubscribeEvents();
            Init();
        }

        private void OnDestroy()
        {
            UnSubscribeEvents();
        }

        private void Init()
        {
            OnDiskCountValueChanged(diskCountSlider.value);
        }

        public void OnDiskCountValueChanged(float value)
        {
            diskCountSliderLabel.text = value + "";
        }

        public void SetLevelCompleteSummaryValues(int moveCount, float gameDuration)
        {
            levelCompleteDurationLabel.text = gameDuration.ToString("0.00");
            levelCompleteMoveCountLabel.text = moveCount + "";
        }

        #region - event register/unregister methods
        void SubscribeEvents()
        {
            if (GameController.Instance)
                GameController.Instance.evLevelComplete.AddListener(LevelCompleteHandler);
        }

        void UnSubscribeEvents()
        {
            if (GameController.Instance)
                GameController.Instance.evLevelComplete.RemoveListener(LevelCompleteHandler);
        }
        #endregion

        private void LevelCompleteHandler()
        {
            uiGameSetup.SetActive(false);
            uiGameHud.SetActive(false);
            uiLevelComplete.SetActive(true);
        }

        #region - button actions
        public void OnPlayPressed()
        {
            //if(!string.IsNullOrEmpty(diskCountInput.text))
            if(diskCountSlider.value > 0)
            {
                int diskCount = (int) diskCountSlider.value;
                GameController.Instance.StartGame(diskCount);

                //uiGameSetup.SetActive(false);
                uiGameSetup.GetComponent<Animator>().Play("Anim Out");
                uiGameHud.SetActive(true);
            }
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        #endregion
    }
}