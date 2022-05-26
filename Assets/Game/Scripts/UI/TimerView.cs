using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace gotoandplay
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField]
        private float timerUpdateFreq = 0.025f;

        public TextMeshProUGUI label;

        private float currentTimerValue = 0f;
        public float CurrentTimerValue
        {
            get
            {
                return currentTimerValue;
            }
        }
        private void OnEnable()
        {
            // pretty sure this gets enabled only when start is pressed. (for now)
            Init();
        }

        private void Init()
        {
            // only run if we have reference to the text component
            if(label)
            {
                Debug.Log("Game started!");
                StartCoroutine(CoroutineTimerStart());
            }
        }

        string[] timerArray;
        private IEnumerator CoroutineTimerStart()
        {
            // only run when game state is in_game
            while ((GameController.Instance != null && GameController.Instance.GetGameState() == GameState.IN_GAME))
            {
                yield return new WaitForSeconds(timerUpdateFreq);
                currentTimerValue += timerUpdateFreq;
                timerArray = currentTimerValue.ToString("0.00").Split(".");
                if (timerArray != null && timerArray.Length > 0)
                    label.text = string.Format("{0}<size=32>.{1}s</size>", timerArray[0], timerArray[1]);
                else
                {
                    // fallback
                    label.text = currentTimerValue + "";
                }
            }
        }
    }
}