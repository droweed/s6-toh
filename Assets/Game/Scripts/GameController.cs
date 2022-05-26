using UnityEngine;
using UnityEngine.Events;

namespace gotoandplay
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;
        [HideInInspector]
        public UnityEvent<int> evStartGame;
        [HideInInspector]
        public UnityEvent evLevelComplete;

        private GameView gameView;

        public TowerController focusedTower;
        public TowerController targetTower;

        public TowerController goalTower;               // our winning tower

        private int currentGameDiskCount = 0;

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

        public void StartGame(int diskCount)
        {
            currentGameDiskCount = diskCount;
            evStartGame.Invoke(diskCount);
        }

        // simulate 3 disk toh test
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
                // only allow to focus if the current tower has at least one disk
                if(value.disks.Count > 0)
                {
                    focusedTower = value;

                    focusedTower.SetMaterialByState(true);
                }
            }
            else if(focusedTower != null && targetTower == null)
            {
                targetTower = value;

                // move top object if there is any
                DiskController toMoveDisk = focusedTower.GetTopDisk();
                DiskController targetTopDisk = targetTower.GetTopDisk();

                if(toMoveDisk)
                {
                    // do check if target tower top disk is bigger than the other to move disk!
                    int startDiskIndex = toMoveDisk.GetDiskIndex();
                    int targetTopDiskIndex = targetTopDisk != null ? targetTopDisk.GetDiskIndex() : -1;

                    if(startDiskIndex < targetTopDiskIndex || targetTopDiskIndex == -1)
                    {
                        // transfer top disk to target tower
                        targetTower.AddDisk(toMoveDisk);
                        focusedTower.RemoveTopDisk();

                        CheckGameComplete();

                        // TODO - you can add move count here, and other actions relating to moving the disk to another spot.
                    }
                }

                // reset material state
                focusedTower.SetMaterialByState(false);
                targetTower.SetMaterialByState(false);

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
                // check for level complete condition
                if (goalTower.disks.Count == currentGameDiskCount)
                {
                    Debug.Log("We win!");
                    evLevelComplete.Invoke();
                }
            }
        }
    }
}