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
    /// Scales to a destination scale following a <see cref="AnimationCurve"/>.
    /// </summary>
    [HelpURL("https://kitbashery.com/docs/tween-components/tween-scale.html")]
    [DisallowMultipleComponent]
    [AddComponentMenu("Kitbashery/Tween/Tween Scale")]
    public class TweenScale : TweenBase
    {
        [Header("Scale:")]
        public TranslationTypes scaleType;
        public Directions direction;
        public Vector3 target;
        public float scaleFactor = 0.5f;
        public bool smoothInterpolate = true;

        private Vector3 initialScale;
        private Vector3 targetScale;
        private Directions initialScaleDirection;

        private Renderer gizmoRend;

        // Start is called before the first frame update
        void Start()
        {
            initialScale = transform.localScale;
            initialScaleDirection = direction;
            SetTargetScale();
        }

        private void OnDrawGizmosSelected()
        {
            if(gizmoRend == null)
            {
                gizmoRend = GetComponent<Renderer>();
            }
            else
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(transform.position, new Vector3(gizmoRend.bounds.size.x * targetScale.x, gizmoRend.bounds.size.y * targetScale.y, gizmoRend.bounds.size.z * targetScale.z));
            }
        }

        public override void UpdateTween()
        {
            if (direction != initialScaleDirection)
            {
                SetTargetScale();
                initialScale = Vector3.one;
            }
            switch (scaleType)
            {
                case TranslationTypes.pingpong:

                    if (smoothInterpolate == false)
                    {
                        Vector3 lerp = Vector3.Lerp(initialScale, targetScale, Mathf.PingPong(speed * Time.time, scaleFactor));
                        transform.localScale = new Vector3(Mathf.RoundToInt(lerp.x), Mathf.RoundToInt(lerp.y), Mathf.RoundToInt(lerp.z));
                    }
                    else
                    {
                        transform.localScale = Vector3.Lerp(initialScale, targetScale, Mathf.PingPong(speed * Time.time, scaleFactor));
                    }

                    break;

                case TranslationTypes.toInitial:

                    if (smoothInterpolate == false)
                    {
                        Vector3 move = Vector3.MoveTowards(transform.localScale, initialScale, speed * Time.deltaTime);
                        transform.localScale = new Vector3(Mathf.RoundToInt(move.x), Mathf.RoundToInt(move.y), Mathf.RoundToInt(move.z));
                    }
                    else
                    {
                        transform.localScale = Vector3.MoveTowards(transform.localScale, initialScale, speed * Time.deltaTime);
                    }

                    if (transform.localScale == initialScale)
                    {
                        tweening = false;
                    }

                    break;

                case TranslationTypes.toTarget:

                    if (smoothInterpolate == false)
                    {
                        Vector3 move = Vector3.MoveTowards(transform.localScale, target, speed * Time.deltaTime);
                        transform.localScale = new Vector3(Mathf.RoundToInt(move.x), Mathf.RoundToInt(move.y), Mathf.RoundToInt(move.z));
                    }
                    else
                    {
                        transform.localScale = Vector3.MoveTowards(transform.localScale, target, speed * Time.deltaTime);
                    }

                    if (transform.localScale == target)
                    {
                        tweening = false;
                    }

                    break;
            }
        }

        public void ScaleToTarget()
        {
            scaleType = TranslationTypes.toTarget;
            tweening = true;
        }

        public void ScaleToInitialScale()
        {
            scaleType = TranslationTypes.toInitial;
            tweening = true;
        }

        private void SetTargetScale()
        {
            switch (direction)
            {
                case Directions.back:

                    targetScale = transform.localScale + (Vector3.back * scaleFactor);

                    break;

                case Directions.down:

                    targetScale = transform.localScale + (Vector3.down * scaleFactor);

                    break;

                case Directions.forward:

                    targetScale = transform.localScale + (Vector3.forward * scaleFactor);

                    break;

                case Directions.left:

                    targetScale = transform.localScale + (Vector3.left * scaleFactor);

                    break;

                case Directions.right:

                    targetScale = transform.localScale + (Vector3.right * scaleFactor);

                    break;

                case Directions.up:

                    targetScale = transform.localScale + (Vector3.up * scaleFactor);

                    break;

                case Directions.all:

                    targetScale = transform.localScale + (Vector3.one * scaleFactor);

                    break;
            }
        }

        public void ResetToInitial()
        {
            transform.localScale = initialScale;
            tweening = tweeningAtStart;
        }
    }
}