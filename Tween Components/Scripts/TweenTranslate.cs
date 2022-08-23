using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    /// Translates to a destination point following a <see cref="AnimationCurve"/>.
    /// </summary>
    [HelpURL("https://kitbashery.com/docs/tween-components/tween-translate.html")]
    [DisallowMultipleComponent]
    [AddComponentMenu("Kitbashery/Tween/Tween Translate")]
    public class TweenTranslate : TweenBase
    {
        #region Variables:

        [Header("Translate:")]
        public Vector3 target = Vector3.forward;
        public UnityEvent onTargetReached;

        private Vector3 initialPos;
        private Vector3 initialTarget;
        private Vector3 worldspaceTarget;
        private Vector3 currentTarget;

        [HideInInspector]
        public Vector3 nextPos;

        #endregion

        #region Initialization & Updates:

        private void Start()
        {
            initialPos = transform.position;
            initialTarget = target;
            nextPos = initialPos;
            UpdateTarget();
        }

        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
            {
                if (initialPos != transform.position)
                {
                    initialPos = transform.position;
                }
                UpdateTarget();
            }
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, currentTarget);
            Gizmos.DrawSphere(currentTarget, 0.1f);
        }

        #endregion

        #region Core Functions:

        public override void UpdateTween()
        {
            if (target != initialTarget)
            {
                UpdateTarget();
            }

            if (transform.position != currentTarget)
            {
                lerpTime += Time.deltaTime;
                nextPos = Vector3.Lerp(transform.position, currentTarget, curve.Evaluate(lerpTime));
                transform.position = nextPos;
                onTweenUpdate.Invoke();
            }
            else
            {
                switch (wrapMode)
                {
                    case WrapMode.PingPong:

                        if (currentTarget == worldspaceTarget)
                        {
                            currentTarget = initialPos;
                        }
                        else
                        {
                            currentTarget = worldspaceTarget;
                        }
                        lerpTime = 0;

                        break;


                    case WrapMode.Loop:

                        transform.position = initialPos;
                        lerpTime = 0;

                        break;


                    case WrapMode.Once:

                        transform.position = initialPos;
                        tweening = false;

                        break;

                    case WrapMode.ClampForever:

                        transform.position = currentTarget;
                        tweening = false;

                        break;

                    case WrapMode.Default:

                        lerpTime = 0;

                        break;

                    default:

                        lerpTime = 0;

                        break;
                }

                onTargetReached.Invoke();
            }
        }

        public void MoveTowardTarget()
        {
            currentTarget = worldspaceTarget;
            tweening = true;
        }

        public void MoveToInitialPos()
        {
            currentTarget = initialPos;
            tweening = true;
        }

        public void SetTarget(Transform t)
        {
            target = t.position;
            UpdateTarget();
        }

        private void UpdateTarget()
        {
            worldspaceTarget = initialPos + target;
            if (currentTarget != initialPos)
            {
                currentTarget = worldspaceTarget;
            }
        }

        public void ResetToInitial()
        {
            transform.position = initialPos;
            target = initialTarget;
            UpdateTarget();
            ResetTweenState();
        }

        #endregion
    }
}