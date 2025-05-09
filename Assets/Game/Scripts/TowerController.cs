using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gotoandplay
{
    public class TowerController : MonoBehaviour
    {
        public GameObject diskPrefab;
        public Transform diskSpawnPosition;
        public Renderer towerRenderer;

        public bool startingTower;
        public TowerType towerType = TowerType.GENERIC;     // Default to generic, set it to goal if its the target tower

        public List<DiskController> disks = new List<DiskController>();

        [Header("Tower State Materials")]
        public Material defaultTowerMat;
        public Material selectedTowerMat;
        public Material hoveredTowerMat;

        private Material currentTowerMat;

        private float minScaleValue = 1.2f;
        private float maxScaleValue = 2.5f;
        private float baseDownScale = 0.2f; // the value the disk shrinks at as it increments
        private const float diskYOffset = 0.45f;

        private bool isSelected;

        private void Start()
        {
            Init();
        }

        void Init()
        {
            SubscribeEvents();
            currentTowerMat = defaultTowerMat = towerRenderer.material;
        }

        private void OnDestroy()
        {
            UnSubscribeEvents();
        }

        void SubscribeEvents()
        {
            if(GameController.Instance)
                GameController.Instance.evStartGame.AddListener(OnGameStart);
        }

        void UnSubscribeEvents()
        {
            if(GameController.Instance)
                GameController.Instance.evStartGame.RemoveListener(OnGameStart);
        }

        void OnGameStart(int diskCount)
        {
            if(startingTower)
            {
                int startingDiskIndex = diskCount - 1;
                float baseScale = 2f;
                if (diskCount > 9)
                {
                    baseScale = maxScaleValue;
                    baseDownScale = (maxScaleValue - minScaleValue) / diskCount;
                } 
                else
                {
                    baseDownScale = (baseScale - minScaleValue) / diskCount;
                }

                for(int i = 0; i < diskCount; i++)
                {
                    // spawn and position disk
                    Vector3 targetPosition = transform.position;
                    targetPosition.y = transform.position.y + (i * diskYOffset);

                    GameObject disk = Instantiate(diskPrefab, targetPosition, Quaternion.identity, diskSpawnPosition);
                    DiskController diskControl = disk.GetComponent<DiskController>();

                    // calculate target disk scale
                    float scaleValue = (baseScale - (i * baseDownScale));
                    scaleValue = Mathf.Clamp(scaleValue, minScaleValue, maxScaleValue);
                    Vector3 targetScale = Vector3.one * scaleValue;
                    targetScale.y = 2;

                    diskControl.SetDiskIndex(startingDiskIndex - i);
                    diskControl.SetDiskScale(targetScale);

                    disks.Add(diskControl);
                }
            }
        }

        public void SetMaterialByState(bool selected)
        {
            isSelected = selected;
            towerRenderer.material = currentTowerMat = selected ? selectedTowerMat : defaultTowerMat;
        }

        public DiskController GetTopDisk()
        {
            if(disks.Count > 0)
                return disks[disks.Count - 1];
            else
                return null;
        }

        public void RemoveTopDisk()
        {
            if(disks.Count > 0)
            {
                disks.RemoveAt(disks.Count - 1);
            }
        }

        public void AddDisk(DiskController value)
        {
            disks.Add(value);

            // change parent
            value.transform.parent = diskSpawnPosition;

            Vector3 mPosition = value.gameObject.transform.localPosition;
            // reset x, z
            mPosition.x = mPosition.z = 0;
            // calculate new y based on list count;
            mPosition.y = transform.position.y + ((disks.Count - 1) * diskYOffset);

            value.transform.localPosition = mPosition;
        }

        /// <summary>
        /// tiny check to disable the mouse overs and clicks of the tower based on the game state
        /// </summary>
        /// <returns></returns>
        private bool IsInteractable()
        {
            if(GameController.Instance)
            {
                if (GameController.Instance.GetGameState() != GameState.IN_GAME)
                    return false;
            }
            return true;
        }

        #region - object interaction
        private void OnMouseOver()
        {
            if (!IsInteractable())
                return;

            if (isSelected)
                return;

            if (towerRenderer)
                towerRenderer.material = hoveredTowerMat;
        }

        private void OnMouseExit()
        {
            if (!IsInteractable())
                return;

            if (towerRenderer)
                towerRenderer.material = currentTowerMat;
        }

        private void OnMouseDown()
        {
            if (!IsInteractable())
                return;

            if (GameController.Instance)
            {
                GameController.Instance.OnTowerPressed(this);
            }
        }
        #endregion

    }

    public enum TowerType {
        GENERIC,
        GOAL
    }
}