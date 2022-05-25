using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace gotoandplay
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;
        public UnityEvent<int> evStartGame;

        private GameView gameView;

        public TowerController focusedTower;
        public TowerController targetTower;

        private void Awake()
        {
            if(Instance == null) { Instance = this; }
        }

        private void Start()
        {
            Init();
            // Test tower of hanoi logic
            //TOH(3, 1, 2, 3);
        }

        void Init()
        {
            gameView = FindObjectOfType<GameView>();
        }

        public void StartGame(int pegCount)
        {
            evStartGame.Invoke(pegCount);
        }

        // simulate 3 peg toh test
        public void TOH(int n, int A, int B, int C)
        {
            if(n > 0)
            {
                TOH(n - 1, A, C, B);
                Debug.Log(string.Format("disc moved from {0} to {1}", A, C));
                TOH(n - 1, B, A, C);
            }
        }

        public void OnTowerPressed(TowerController value)
        {
            if(focusedTower == null)
            {
                focusedTower = value;
            } 
            else if(focusedTower != null && targetTower == null)
            {
                targetTower = value;
                //Debug.Log(string.Format("Start {0} - Destination {1}", focusedTower.gameObject.name, targetTower.gameObject.name));
                
                // move top object if there is any
                PegController toMovePeg = focusedTower.GetTopPeg();
                PegController targetTopPeg = targetTower.GetTopPeg();

                if(toMovePeg)
                {
                    // do check if target tower top peg is bigger than to move peg!
                    int startPegIndex = toMovePeg.GetPegIndex();
                    int targetTopPegIndex = targetTopPeg != null ? targetTopPeg.GetPegIndex() : -1;

                    if(startPegIndex < targetTopPegIndex || targetTopPegIndex == -1)
                    {
                        // transfer top peg to target tower
                        targetTower.AddPeg(toMovePeg);
                        focusedTower.RemoveTopPeg();
                    }
                }

                // clear selected targets
                focusedTower = null;
                targetTower = null;
            }
        }
    }
}