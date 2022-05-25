using UnityEngine;
using UnityEngine.Events;

namespace gotoandplay
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;
        [HideInInspector]
        public UnityEvent<int> evStartGame;

        private GameView gameView;

        public TowerController focusedTower;
        public TowerController targetTower;

        public TowerController goalTower;               // our winning tower

        private int currentGamePegCount = 0;

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
            currentGamePegCount = pegCount;
            evStartGame.Invoke(pegCount);
        }

        // simulate 3 peg toh test
        public void TOH(int n, int A, int B, int C)
        {
            if(n > 0)
            {
                TOH(n - 1, A, C, B);
                Debug.Log(string.Format("torus moved from {0} to {1}", A, C));
                TOH(n - 1, B, A, C);
            }
        }

        public void OnTowerPressed(TowerController value)
        {
            if(focusedTower == null)
            {
                // only allow to focus if the current tower has at least one peg
                if(value.pegs.Count > 0)
                {
                    focusedTower = value;
                }
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

                        CheckGameComplete();

                        // TODO - you can add move count here, and other actions relating to moving the peg to another spot.
                    }
                }

                // clear selected targets
                focusedTower = null;
                targetTower = null;
            }
        }

        private void CheckGameComplete()
        {
            if(goalTower == null)
            {
                TowerController[] towerControllers = FindObjectsOfType<TowerController>();
                foreach(var tower in towerControllers)
                {
                    if(tower.towerType == TowerType.GOAL)
                    {
                        // this is the goal tower
                        goalTower = tower;
                        break;
                    }
                }
            }

            if(goalTower)
            {
                //Debug.Log(string.Format("{0} == {1}", goalTower.pegs.Count, currentGamePegCount));
                if (goalTower.pegs.Count == currentGamePegCount)
                {
                    Debug.Log("We win!");
                }
            }
        }
    }
}