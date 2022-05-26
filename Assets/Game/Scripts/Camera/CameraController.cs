using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gotoandplay
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;

        [Header("Shake Properties")]
        public float shakeDuration = 0.2f;
        public float shakeIntensity = 0.25f;

        private void Start()
        {
            
        }

        public void Shake()
        {
            StopCoroutine("Shake");
            StartCoroutine(ShakeCoroutine(shakeDuration, shakeIntensity));
        }

        private IEnumerator ShakeCoroutine(float duration, float intensity)
        {
            Vector3 orgPos = target.localPosition;
            float elapsed = 0f;

            while(elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * intensity;
                float y = Random.Range(-1f, 1f) * intensity;

                target.localPosition = new Vector3(x, y, orgPos.z);

                elapsed += Time.deltaTime;
                yield return null;
            }

            target.localPosition = orgPos;
        }
    }
}