using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gotoandplay
{
    public class GameController : MonoBehaviour
    {
        private void Start()
        {
            // Test tower of hanoi logic
            //TOH(3, 1, 2, 3);
        }

        public void TOH(int n, int A, int B, int C)
        {
            if(n > 0)
            {
                TOH(n - 1, A, C, B);
                Debug.Log(string.Format("disc moved from {0} to {1}", A, C));
                TOH(n - 1, B, A, C);
            }
        }
    }
}