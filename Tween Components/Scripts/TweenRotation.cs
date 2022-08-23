using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 MIT License

Copyright (c) 2022 Kitbashery

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.



Need support or additional features? Please visit https://kitbashery.com/
*/

namespace Kitbashery.Tween
{
    /// <summary>
    /// Rotates to a destination rotation following a <see cref="AnimationCurve"/>.
    /// </summary>
    [HelpURL("https://kitbashery.com/docs/tween-components/tween-rotation.html")]
    [DisallowMultipleComponent]
    [AddComponentMenu("Kitbashery/Tween/Tween Rotation")]
    public class TweenRotation : TweenBase
    {
        [Header("Rotation:")]
        public RotationTypes rotationType;
        public Directions direction;
        [Range(0, 360)]
        public float targetAngle = 15;

        private Vector3 initialRot;
        private Directions initialRotDirection;
        private Vector3 rotVector;

        /// <summary>
        /// 
        /// </summary>
        [HideInInspector]
        public Quaternion nextRot;

        // Start is called before the first frame update
        void Start()
        {
            initialRotDirection = direction;
            initialRot = transform.localEulerAngles;
            nextRot = Quaternion.Euler(initialRot);
            SetRotDirectionVector();
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, targetAngle, 1f);
#endif
        }

        public override void UpdateTween()
        {
            if (direction != initialRotDirection)
            {
                SetRotDirectionVector();
                initialRotDirection = direction;
            }
            switch (rotationType)
            {
                case RotationTypes.pingpong:

                    transform.localRotation = Quaternion.Lerp(Quaternion.Euler(initialRot), Quaternion.Euler(rotVector * targetAngle), 0.5f * (1.0f + Mathf.Sin(Mathf.PI * Time.realtimeSinceStartup * speed)));

                    break;

                case RotationTypes.spin:

                    transform.Rotate(rotVector, (targetAngle * speed) * Time.deltaTime);

                    break;

                case RotationTypes.instant:

                    transform.Rotate(rotVector, targetAngle);

                    break;
            }
        }

        public void SetRotDirectionVector()
        {
            switch (direction)
            {
                case Directions.back:

                    rotVector = Vector3.back;

                    break;

                case Directions.down:

                    rotVector = Vector3.down;

                    break;

                case Directions.forward:

                    rotVector = Vector3.forward;

                    break;

                case Directions.left:

                    rotVector = Vector3.left;

                    break;

                case Directions.right:

                    rotVector = Vector3.right;

                    break;

                case Directions.up:

                    rotVector = Vector3.up;

                    break;

                case Directions.all:

                    rotVector = Vector3.one;

                    break;
            }
        }

        public void ResetToInitial()
        {
            transform.localRotation = Quaternion.Euler(initialRot);
            ResetTweenState();
        }
    }
}