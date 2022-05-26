using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gotoandplay
{
    public class CameraController : MonoBehaviour
    {

        [Header("Shake Properties")]
        public Transform shakeTarget;
        [Space]
        public float shakeDuration = 0.2f;
        public float shakeIntensity = 0.25f;

        [Header("Rotate around target")]
        [SerializeField]
        private float mouseSensitivity = 3.0f;

        private float rotY;
        private float rotX;

        public float yTargetOffset;
        public bool invertYAxis;

        [SerializeField]
        private Transform targetObject;

        [SerializeField]
        private float targetDistance = 3.0f;

        private Vector3 currRotation;
        private Vector3 smoothingVel = Vector3.zero;

        [SerializeField]
        private float smoothTime = 0.2f;

        [SerializeField]
        private Vector2 rotationXMinMax = new Vector2(-40, 40);

        private bool mouseIsDown;
        private Vector3 targetPosition;

        private void Start()
        {
            
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(1))
            {
                mouseIsDown = true;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                mouseIsDown = false;
            }

            if(mouseIsDown)
            {
                RotateAroundObject();
            }
        }

        #region - shake methods
        public void Shake()
        {
            StopCoroutine("Shake");
            StartCoroutine(ShakeCoroutine(shakeDuration, shakeIntensity));
        }

        private IEnumerator ShakeCoroutine(float duration, float intensity)
        {
            Vector3 orgPos = shakeTarget.localPosition;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * intensity;
                float y = Random.Range(-1f, 1f) * intensity;

                shakeTarget.localPosition = new Vector3(x, y, orgPos.z);

                elapsed += Time.deltaTime;
                yield return null;
            }

            shakeTarget.localPosition = orgPos;
        }
        #endregion

        #region - rotate around object
        private void RotateAroundObject()
        {
            if (targetObject == null)
                return;

            targetPosition = targetObject.position;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            if (invertYAxis)
                mouseY *= -1;

            rotY += mouseX;
            rotX += mouseY;
            rotX = Mathf.Clamp(rotX, rotationXMinMax.x, rotationXMinMax.y);
            Vector3 nextRotation = new Vector3(rotX, rotY);

            // add damping
            currRotation = Vector3.SmoothDamp(currRotation, nextRotation, ref smoothingVel, smoothTime);
            transform.localEulerAngles = currRotation;

            // add ytarget yOffset in case you wanna tweak the camera focus height
            targetPosition.y = targetPosition.y + yTargetOffset;
            // subtract forward of the gameobject to point forward to the target
            transform.position = targetPosition - transform.forward * targetDistance;
        }
        #endregion
    }
}