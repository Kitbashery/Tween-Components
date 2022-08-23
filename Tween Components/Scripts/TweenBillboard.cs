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
    /// Rotates the transform so its forward direction faces the main camera.
    /// </summary>
    [HelpURL("https://kitbashery.com/docs/tween-components/tween-billboard.html")]
    [DisallowMultipleComponent]
    [AddComponentMenu("Kitbashery/Tween/Billboard")]
    public class TweenBillboard : TweenBase
    {
        [Header("Billboard:")]
        [Tooltip("Leave blank to default to main camera.")]
        public Transform lookAtTarget;
        public bool horizontalAxis = false;
        public bool directLook = false;

        private Vector3 lookAtPos;

        private void Awake()
        {
            if (lookAtTarget == null)
            {
                lookAtTarget = Camera.main.transform;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.forward);
        }

        public override void UpdateTween()
        {
            if (directLook == true)
            {
                lookAtPos = 2 * transform.position - lookAtTarget.position;
                if (horizontalAxis == true)
                {
                    lookAtPos.y = transform.position.y;
                }
            }
            else
            {
                if (horizontalAxis == false)
                {
                    lookAtPos = transform.position + lookAtTarget.forward;
                }
                else
                {
                    lookAtPos = transform.position + new Vector3(lookAtTarget.forward.x, 0, lookAtTarget.forward.z);
                }
            }
            transform.LookAt(lookAtPos);
        }
    }
}