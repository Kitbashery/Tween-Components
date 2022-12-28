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
    /// Translates to a destination point following a <see cref="AnimationCurve"/> along a spline path.
    /// </summary>
    [HelpURL("https://kitbashery.com/docs/tween-components/tween-path.html")]
    [DisallowMultipleComponent]
    [AddComponentMenu("Kitbashery/Tween/Tween Path")]
    public class TweenPath : TweenBase
    {
        public Vector3 startPoint;
        public Vector3 controlPoint1;
        public Vector3 controlPoint2;
        public Vector3 endPoint;

        // Property for the spline type
        public SplineType currentSplineType = SplineType.Bezier;

        // Property for the number of segments to use for drawing the spline
        [Min(2)]
        public int segments = 10;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Vector3 prevPoint = startPoint;

            for (int i = 0; i < segments; i++)
            {
                // Calculate parameter t for this segment
                float t = i / (segments - 1);

                // Calculate point on the Bezier spline
                Vector3 point = Vector3.zero;

                switch (currentSplineType)
                {
                    case SplineType.Bezier:

                        point = CalculateBezierPoint(t, startPoint, controlPoint2, controlPoint1, endPoint);

                        break;
                    case SplineType.Hermite:

                        point = CalculateHermitePoint(t, startPoint, endPoint, controlPoint1, controlPoint2);

                        break;
                    case SplineType.CatmullRom:

                        point = CalculateCatmullRomPoint(t, startPoint, controlPoint1, endPoint, controlPoint2);

                        break;
                    case SplineType.BSpline:

                        point = CalculateBSplinePoint(t, startPoint, controlPoint2, controlPoint1, endPoint);

                        break;
                    case SplineType.Linear:

                        point = CalculateLinearPoint(t, startPoint, endPoint);

                        break;
                }

                // Draw line between this point and the previous point
                Gizmos.DrawLine(prevPoint, point);

                // Set this point as the previous point for the next iteration
                prevPoint = point;
            }
        }

        public Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 p = uuu * p0;
            p += 3 * uu * t * p1;
            p += 3 * u * tt * p2;
            p += ttt * p3;

            return p;
        }

        public Vector3 CalculateHermitePoint(float t, Vector3 p0, Vector3 p1, Vector3 m0, Vector3 m1)
        {
            float tt = t * t;
            float ttt = tt * t;
            float h1 = 2 * ttt - 3 * tt + 1;
            float h2 = -2 * ttt + 3 * tt;
            float h3 = ttt - 2 * tt + t;
            float h4 = ttt - tt;

            Vector3 p = h1 * p0 + h2 * p1 + h3 * m0 + h4 * m1;

            return p;
        }

        public Vector3 CalculateCatmullRomPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float tt = t * t;
            float ttt = tt * t;

            Vector3 p = 0.5f * (2 * p1 + (-p0 + p2) * t + (2 * p0 - 5 * p1 + 4 * p2 - p3) * tt + (-p0 + 3 * p1 - 3 * p2 + p3) * ttt);

            return p;
        }

        public Vector3 CalculateBSplinePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float tt = t * t;
            float ttt = tt * t;

            Vector3 p = (1 / 6f) * (p0 * (-ttt + 3 * tt - 3 * t + 1) + p1 * (3 * ttt - 6 * tt + 4) + p2 * (-3 * ttt + 3 * tt + 3 * t + 1) + p3 * (ttt));

            return p;
        }

        public Vector3 CalculateLinearPoint(float t, Vector3 p0, Vector3 p1)
        {
            Vector3 p = p0 + t * (p1 - p0);

            return p;
        }
    }

    // Enumeration for different spline types
    public enum SplineType
    {
        Bezier,
        Hermite,
        CatmullRom,
        BSpline,
        Linear
    }
}