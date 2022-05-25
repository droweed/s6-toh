using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gotoandplay
{
    public class PegController : MonoBehaviour
    {
        public Transform pegModelRoot;
        [SerializeField]
        private int pegIndex = -1;

        private void Start()
        {
            
        }

        public void SetPegIndex(int value)
        {
            pegIndex = value;
        }

        public int GetPegIndex()
        {
            return pegIndex;
        }

        public void SetPegScale(Vector3 mScale)
        {
            pegModelRoot.localScale = mScale;
        }
    }
}
