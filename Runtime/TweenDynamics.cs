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
    /// Implements second order dynamics.
    /// </summary>
    public class TweenDynamics : TweenBase
    {
        #region Properties:

        [Tooltip("The mass of the GameObject")]
        public float mass = 1.0f;

        [Tooltip("The desired position of the GameObject")]
        public Vector3 target;

        [Tooltip("The current position of the GameObject")]
        private Vector3 position;

        [Tooltip("The current velocity of the GameObject")]
        private Vector3 velocity;

        [Tooltip("The current acceleration of the GameObject")]
        private Vector3 acceleration;

        [Tooltip("The spring constant of the system")]
        public float springConstant = 1.0f;

        [Tooltip("The damping constant of the system")]
        public float dampingConstant = 1.0f;

        [Tooltip("The natural frequency of the system")]
        public float naturalFrequency = 1.0f;

        [Tooltip("The time step of the simulation")]
        public float timeStep = 0.01f;

        [Tooltip("The maximum acceleration of the system")]
        public float maxAcceleration = 10.0f;

        [Tooltip("The maximum velocity of the system")]
        public float maxVelocity = 10.0f;

        private Vector3 force;
        private float omega, a, b;

        #endregion

        #region Initialization & Updates:

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, target);
            Gizmos.DrawSphere(target, 0.5f);
        }

        #endregion

        #region Methods:

        public override void UpdateTween()
        {
            // Calculate the force on the GameObject
            force = springConstant * (target- position) - dampingConstant * velocity;

            // Calculate the acceleration of the GameObject
            acceleration = force / mass;

            // Clamp the acceleration to the maximum acceleration
            acceleration = Vector3.ClampMagnitude(acceleration, maxAcceleration);

            // Calculate the pole zero matching constants
            omega = naturalFrequency * Mathf.Sqrt(1 - Mathf.Pow(dampingConstant / (2 * mass * naturalFrequency), 2));
            a = (acceleration.magnitude / omega) / (2 * mass * omega);
            b = (velocity.magnitude / omega) - a;

            // Update the velocity of the GameObject using pole zero matching
            velocity += (a * acceleration + b * force / mass) * timeStep;

            // Clamp the velocity to the maximum velocity
            velocity = Vector3.ClampMagnitude(velocity, maxVelocity);

            // Update the position of the GameObject
            position += velocity * timeStep;

            // Set the position of the GameObject
            transform.position = position;
        }

        #endregion
    }

}