using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gotoandplay
{
    public class TowerController : MonoBehaviour
    {
        public GameObject pegPrefab;
        public Transform pegSpawnPosition;

        public bool startingTower;
        public TowerType towerType = TowerType.GENERIC;     // Default to generic, set it to goal if its the target tower

        public List<PegController> pegs = new List<PegController>();

        private const float baseDownScale = 0.2f; // the value the peg shrinks at as it increments
        private const float pegYOffset = 0.45f;

        private void Start()
        {
            SubscribeEvents();
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

        void OnGameStart(int pegCount)
        {
            if(startingTower)
            {
                int startingPegIndex = pegCount - 1;

                for(int i = 0; i < pegCount; i++)
                {
                    // spawn and position peg
                    Vector3 targetPosition = transform.position;
                    targetPosition.y = transform.position.y + (i * pegYOffset);

                    GameObject peg = Instantiate(pegPrefab, targetPosition, Quaternion.identity, pegSpawnPosition);
                    PegController pegControl = peg.GetComponent<PegController>();

                    // calculate target peg scale
                    Vector3 targetScale = Vector3.one * (2 - (i * baseDownScale));
                    targetScale.y = 2;

                    pegControl.SetPegIndex(startingPegIndex - i);
                    pegControl.SetPegScale(targetScale);

                    pegs.Add(pegControl);
                }
            }
        }

        private void OnMouseDown()
        {
            if(GameController.Instance)
            {
                GameController.Instance.OnTowerPressed(this);
            }
        }

        public PegController GetTopPeg()
        {
            if(pegs.Count > 0)
                return pegs[pegs.Count - 1];
            else
                return null;
        }

        public void RemoveTopPeg()
        {
            if(pegs.Count > 0)
            {
                pegs.RemoveAt(pegs.Count - 1);
            }
        }

        public void AddPeg(PegController value)
        {
            pegs.Add(value);

            // change parent
            value.transform.parent = pegSpawnPosition;

            Vector3 mPosition = value.gameObject.transform.localPosition;
            // reset x, z
            mPosition.x = mPosition.z = 0;
            // calculate new y based on list count;
            mPosition.y = transform.position.y + ((pegs.Count - 1) * pegYOffset);

            value.transform.localPosition = mPosition;
        }
    }

    public enum TowerType {
        GENERIC,
        GOAL
    }
}