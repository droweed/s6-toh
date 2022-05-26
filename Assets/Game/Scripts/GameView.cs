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