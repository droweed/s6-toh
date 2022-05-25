using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gotoandplay
{
    public class DiskController : MonoBehaviour
    {
        public Transform diskModelRoot;
        [SerializeField]
        private int diskIndex = -1;

        private void Start()
        {
            
        }

        public void SetDiskIndex(int value)
        {
            diskIndex = value;
        }

        public int GetDiskIndex()
        {
            return diskIndex;
        }

        public void SetDiskScale(Vector3 mScale)
        {
            diskModelRoot.localScale = mScale;
        }
    }
}
