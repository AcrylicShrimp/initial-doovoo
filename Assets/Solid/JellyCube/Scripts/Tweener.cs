/***********************************************************************************************************
 * Tweener.cs                                                                                              *
 * by Rodrigo Pegorari - 2015 - http://www.rodrigopegorari.com                                             *
 ***********************************************************************************************************/

/* TERMS OF USE - EASING EQUATIONS
 * Open source under the BSD License.
 * Copyright (c)2001 Robert Penner
 * All rights reserved.
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 * Neither the name of the author nor the names of contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using UnityEngine;
using System.Collections.Generic;

namespace JellyCube
{
    public class Tweener : MonoBehaviour
    {
        #region EaseType
        public enum TweenerEaseType
        {
            Linear = 0,
            EaseInQuad = 1,
            EaseOutQuad = 2,
            EaseInOutQuad = 3,
            EaseOutInQuad = 4,
            EaseInCubic = 5,
            EaseOutCubic = 6,
            EaseInOutCubic = 7,
            EaseOutInCubic = 8,
            EaseInQuart = 9,
            EaseOutQuart = 10,
            EaseInOutQuart = 11,
            EaseOutInQuart = 12,
            EaseInQuint = 13,
            EaseOutQuint = 14,
            EaseInOutQuint = 15,
            EaseOutInQuint = 16,
            EaseInSine = 17,
            EaseOutSine = 18,
            EaseInOutSine = 19,
            EaseOutInSine = 20,
            EaseInExpo = 21,
            EaseOutExpo = 22,
            EaseInOutExpo = 23,
            EaseOutInExpo = 24,
            EaseInCirc = 25,
            EaseOutCirc = 26,
            EaseInOutCirc = 27,
            EaseOutInCirc = 28,
            EaseInElastic = 29,
            EaseOutElastic = 30,
            EaseInOutElastic = 31,
            EaseOutInElastic = 32,
            EaseInBack = 33,
            EaseOutBack = 34,
            EaseInOutBack = 35,
            EaseOutInBack = 36,
            EaseInBounce = 37,
            EaseOutBounce = 38,
            EaseInOutBounce = 39,
            EaseOutInBounce = 40
        }
        #endregion

        #region EaseMethods
        static EaseDelegate[] methods = new EaseDelegate[]
    {
		EaseNone,
		EaseInQuad,
		EaseOutQuad,
		EaseInOutQuad,
		EaseOutInQuad,
		EaseInCubic,
		EaseOutCubic,
		EaseInOutCubic,
		EaseOutInCubic,
		EaseInQuart, 
		EaseOutQuart,  
		EaseInOutQuart,  
		EaseOutInQuart,  
		EaseInQuint,  
		EaseOutQuint,  
		EaseInOutQuint,  
		EaseOutInQuint,
		EaseInSine, 
		EaseOutSine, 
		EaseInOutSine, 
		EaseOutInSine, 
		EaseInExpo,  
		EaseOutExpo,  
		EaseInOutExpo,  
		EaseOutInExpo,
		EaseInCirc,  
		EaseOutCirc,  
		EaseInOutCirc,  
		EaseOutInCirc,  
		EaseInElastic,  
		EaseOutElastic,  
		EaseInOutElastic,  
		EaseOutInElastic,
		EaseInBack, 
		EaseOutBack, 
		EaseInOutBack, 
		EaseOutInBack, 
		EaseInBounce, 
		EaseOutBounce, 
		EaseInOutBounce,  
		EaseOutInBounce
	};
        #endregion

        public enum TransformProperty
        {
            Position,
            Rotation,
            Scale,
            LocalPosition
        }

        private Transform m_Transform;
        private TransformProperty m_TweenProperty;
        private Vector3 m_From;
        private Vector3 m_To;
        private float m_Timer;
        private float m_Duration;
        private TweenerEaseType m_EaseType;
        private Action m_OnTweenCompleteCallback { get; set; }
        private bool m_Init = false;
        private Vector3 m_TweenValue = Vector3.zero;

        public static void MoveTo(GameObject element, Vector3 from, Vector3 to, float duration = 1, float delay = 0, TweenerEaseType easeType = TweenerEaseType.EaseInOutSine, Action onTweenCompleteCallback = null)
        {
            Tweener tweener = element.AddComponent<Tweener>();
            tweener.Tween(element.transform, TransformProperty.Position, from, to, duration, delay, easeType, onTweenCompleteCallback);
        }

        public static void RotateTo(GameObject element, Vector3 from, Vector3 to, float duration = 1, float delay = 0, TweenerEaseType easeType = TweenerEaseType.EaseInOutSine, Action onTweenCompleteCallback = null)
        {
            Tweener tweener = element.AddComponent<Tweener>();
            tweener.Tween(element.transform, TransformProperty.Rotation, from, to, duration, delay, easeType, onTweenCompleteCallback);
        }

        public static void ScaleTo(GameObject element, Vector3 from, Vector3 to, float duration = 1, float delay = 0, TweenerEaseType easeType = TweenerEaseType.EaseInOutSine, Action onTweenCompleteCallback = null)
        {
            Tweener tweener = element.AddComponent<Tweener>();
            tweener.Tween(element.transform, TransformProperty.Scale, from, to, duration, delay, easeType, onTweenCompleteCallback);
        }

        private void Tween(Transform element, TransformProperty property, Vector3 from, Vector3 to, float duration, float delay, TweenerEaseType easeType, Action onTweenCompleteCallback)
        {
            m_Transform = element;
            m_TweenProperty = property;
            m_From = from;
            m_To = to;
            m_EaseType = easeType;
            m_OnTweenCompleteCallback += onTweenCompleteCallback;
            m_Timer -= delay;
            m_Duration = duration;
            m_Init = true;
        }

        void FixedUpdate()
        {
            if (!m_Init)
            {
                return;
            }

            if (m_Timer > 0)
            {
                if (m_Timer < m_Duration)
                {
                    m_TweenValue = ChangeVector(m_Timer, m_From, m_To, m_Duration, m_EaseType);
                    UpdateTweener();
                }
                else
                {
                    m_TweenValue = m_To;
                    Complete();
                }
            }

            m_Timer += Time.deltaTime;
        }

        private void UpdateTweener()
        {
            switch (m_TweenProperty)
            {
                case TransformProperty.LocalPosition:
                    m_Transform.localPosition = m_TweenValue;
                    break;
                case TransformProperty.Position:
                    m_Transform.position = m_TweenValue;
                    break;
                case TransformProperty.Rotation:
                    m_Transform.localRotation = Quaternion.Euler(m_TweenValue);
                    break;
                case TransformProperty.Scale:
                    m_Transform.localScale = m_TweenValue;
                    break;
            }
        }

        private void Complete()
        {
            m_Init = false;

            UpdateTweener();

            if (m_OnTweenCompleteCallback != null)
            {
                m_OnTweenCompleteCallback();
                m_OnTweenCompleteCallback = null;
            }

            Destroy(this);
        }

        public static void Stop(GameObject obj)
        {
            Tweener t = obj.GetComponent<Tweener>();

            if (t != null)
            {
                t.m_TweenValue = t.m_To;
                t.Complete();
            }
        }

        /*
        public void StopAll()
        {
            m_DestroyObjects.Clear();

            foreach (TweenerObject tweenerObject in m_TweenerObjects)
            {
                m_DestroyObjects.Add(tweenerObject);
            }
        
            foreach (TweenerObject tweenerObject in m_DestroyObjects)
            {
                if (m_TweenerObjects.Contains(tweenerObject))
                {
                    m_TweenerObjects.Remove(tweenerObject);
                }
            }
        
            m_DestroyObjects.Clear();
        }

        public void StopObject(Transform element)
        {
            m_DestroyObjects.Clear();
        
            foreach (TweenerObject tweenerObject in m_TweenerObjects)
            {
                if (element.Equals(element)) m_DestroyObjects.Add(tweenerObject);
            }
        
            foreach (TweenerObject tweenerObject in m_DestroyObjects)
            {
                if (m_TweenerObjects.Contains(tweenerObject)) m_TweenerObjects.Remove(tweenerObject);
            }
        
            m_DestroyObjects.Clear();
        }
        */


        #region equations
        // TWEENING EQUATIONS floats -----------------------------------------------------------------------------------------------------
        // (the original equations are Robert Penner's work as mentioned on the disclaimer)

        /**
         * Easing equation float for a simple linear tweening, with no easing.
         *
         * @param t		Current time (in frames or seconds).
         * @param b		Starting value.
         * @param c		Change needed in value.
         * @param d		Expected easing duration (in frames or seconds).
         * @return		The correct value.
         */

        public static float EaseNone(float t, float b, float c, float d)
        {
            c -= b;
            return c * t / d + b;
        }

        /**
         * Easing equation float for a quadratic (t^2) easing in: accelerating from zero velocity.
         *
         * @param t		Current time (in frames or seconds).
         * @param b		Starting value.
         * @param c		Change needed in value.
         * @param d		Expected easing duration (in frames or seconds).
         * @return		The correct value.
         */
        public static float EaseInQuad(float t, float b, float c, float d)
        {
            c -= b;
            return c * (t /= d) * t + b;
        }

        /**
         * Easing equation float for a quadratic (t^2) easing out: decelerating to zero velocity.
         *
         * @param t		Current time (in frames or seconds).
         * @param b		Starting value.
         * @param c		Change needed in value.
         * @param d		Expected easing duration (in frames or seconds).
         * @return		The correct value.
         */
        public static float EaseOutQuad(float t, float b, float c, float d)
        {
            c -= b;
            return -c * (t /= d) * (t - 2) + b;
        }

        /**
         * Easing equation float for a quadratic (t^2) easing in/out: acceleration until halfway, then deceleration.
         *
         * @param t		Current time (in frames or seconds).
         * @param b		Starting value.
         * @param c		Change needed in value.
         * @param d		Expected easing duration (in frames or seconds).
         * @return		The correct value.
         */
        public static float EaseInOutQuad(float t, float b, float c, float d)
        {
            c -= b;

            if ((t /= d / 2) < 1) return c / 2 * t * t + b;

            return -c / 2 * ((--t) * (t - 2) - 1) + b;
        }

        /**
         * Easing equation float for a quadratic (t^2) easing out/in: deceleration until halfway, then acceleration.
         *
         * @param t		Current time (in frames or seconds).
         * @param b		Starting value.
         * @param c		Change needed in value.
         * @param d		Expected easing duration (in frames or seconds).
         * @return		The correct value.
         */
        public static float EaseOutInQuad(float t, float b, float c, float d)
        {
            c -= b;
            if (t < d / 2) return EaseOutQuad(t * 2, b, c / 2, d);
            return EaseInQuad((t * 2) - d, b + c / 2, c / 2, d);
        }

        /**
         * Easing equation float for a cubic (t^3) easing in: accelerating from zero velocity.
             *
         * @param t		Current time (in frames or seconds).
         * @param b		Starting value.
         * @param c		Change needed in value.
         * @param d		Expected easing duration (in frames or seconds).
         * @return		The correct value.
         */
        public static float EaseInCubic(float t, float b, float c, float d)
        {
            c -= b;
            return c * (t /= d) * t * t + b;
        }

        /**
         * Easing equation float for a cubic (t^3) easing out: decelerating from zero velocity.
             *
         * @param t		Current time (in frames or seconds).
         * @param b		Starting value.
         * @param c		Change needed in value.
         * @param d		Expected easing duration (in frames or seconds).
         * @return		The correct value.
         */
        public static float EaseOutCubic(float t, float b, float c, float d)
        {
            c -= b;
            return c * ((t = t / d - 1) * t * t + 1) + b;
        }

        /**
         * Easing equation float for a cubic (t^3) easing in/out: acceleration until halfway, then deceleration.
             *
         * @param t		Current time (in frames or seconds).
         * @param b		Starting value.
         * @param c		Change needed in value.
         * @param d		Expected easing duration (in frames or seconds).
         * @return		The correct value.
         */
        public static float EaseInOutCubic(float t, float b, float c, float d)
        {
            c -= b;
            if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
            return c / 2 * ((t -= 2) * t * t + 2) + b;
        }

        /**
         * Easing equation float for a cubic (t^3) easing out/in: deceleration until halfway, then acceleration.
             *
         * @param t		Current time (in frames or seconds).
         * @param b		Starting value.
         * @param c		Change needed in value.
         * @param d		Expected easing duration (in frames or seconds).
         * @return		The correct value.
         */
        public static float EaseOutInCubic(float t, float b, float c, float d)
        {
            c -= b;
            if (t < d / 2) return EaseOutCubic(t * 2, b, c / 2, d);
            return EaseInCubic((t * 2) - d, b + c / 2, c / 2, d);
        }

        /**
        * Easing equation float for a quartic (t^4) easing in: accelerating from zero velocity.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseInQuart(float t, float b, float c, float d)
        {
            c -= b;
            return c * (t /= d) * t * t * t + b;
        }

        /**
        * Easing equation float for a quartic (t^4) easing out: decelerating from zero velocity.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseOutQuart(float t, float b, float c, float d)
        {
            c -= b;
            return -c * ((t = t / d - 1) * t * t * t - 1) + b;
        }

        /**
        * Easing equation float for a quartic (t^4) easing in/out: acceleration until halfway, then deceleration.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseInOutQuart(float t, float b, float c, float d)
        {
            c -= b;
            if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b;
            return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
        }

        /**
        * Easing equation float for a quartic (t^4) easing out/in: deceleration until halfway, then acceleration.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseOutInQuart(float t, float b, float c, float d)
        {
            c -= b;
            if (t < d / 2) return EaseOutQuart(t * 2, b, c / 2, d);
            return EaseInQuart((t * 2) - d, b + c / 2, c / 2, d);
        }

        /**
        * Easing equation float for a quintic (t^5) easing in: accelerating from zero velocity.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseInQuint(float t, float b, float c, float d)
        {
            c -= b;
            return c * (t /= d) * t * t * t * t + b;
        }

        /**
        * Easing equation float for a quintic (t^5) easing out: decelerating from zero velocity.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseOutQuint(float t, float b, float c, float d)
        {
            c -= b;
            return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
        }

        /**
        * Easing equation float for a quintic (t^5) easing in/out: acceleration until halfway, then deceleration.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseInOutQuint(float t, float b, float c, float d)
        {
            c -= b;
            if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b;
            return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
        }

        /**
        * Easing equation float for a quintic (t^5) easing out/in: deceleration until halfway, then acceleration.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseOutInQuint(float t, float b, float c, float d)
        {
            c -= b;
            if (t < d / 2) return EaseOutQuint(t * 2, b, c / 2, d);
            return EaseInQuint((t * 2) - d, b + c / 2, c / 2, d);
        }

        /**
        * Easing equation float for a sinusoidal (sin(t)) easing in: accelerating from zero velocity.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseInSine(float t, float b, float c, float d)
        {
            c -= b;
            return -c * Mathf.Cos(t / d * (Mathf.PI / 2)) + c + b;
        }

        /**
        * Easing equation float for a sinusoidal (sin(t)) easing out: decelerating from zero velocity.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseOutSine(float t, float b, float c, float d)
        {
            c -= b;
            return c * Mathf.Sin(t / d * (Mathf.PI / 2)) + b;
        }

        /**
        * Easing equation float for a sinusoidal (sin(t)) easing in/out: acceleration until halfway, then deceleration.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseInOutSine(float t, float b, float c, float d)
        {
            c -= b;
            return -c / 2 * (Mathf.Cos(Mathf.PI * t / d) - 1) + b;
        }

        /**
        * Easing equation float for a sinusoidal (sin(t)) easing out/in: deceleration until halfway, then acceleration.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseOutInSine(float t, float b, float c, float d)
        {
            c -= b;
            if (t < d / 2) return EaseOutSine(t * 2, b, c / 2, d);
            return EaseInSine((t * 2) - d, b + c / 2, c / 2, d);
        }

        /**
        * Easing equation float for an exponential (2^t) easing in: accelerating from zero velocity.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseInExpo(float t, float b, float c, float d)
        {
            c -= b;
            return (t == 0) ? b : c * Mathf.Pow(2, 10 * (t / d - 1)) + b - c * 0.001f;
        }

        /**
        * Easing equation float for an exponential (2^t) easing out: decelerating from zero velocity.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseOutExpo(float t, float b, float c, float d)
        {
            c -= b;
            return (t == d) ? b + c : c * 1.001f * (-Mathf.Pow(2, -10 * t / d) + 1) + b;
        }

        /**
        * Easing equation float for an exponential (2^t) easing in/out: acceleration until halfway, then deceleration.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseInOutExpo(float t, float b, float c, float d)
        {
            c -= b;
            if (t == 0) return b;
            if (t == d) return b + c;
            if ((t /= d / 2) < 1) return c / 2 * Mathf.Pow(2, 10 * (t - 1)) + b - c * 0.0005f;
            return c / 2 * 1.0005f * (-Mathf.Pow(2, -10 * --t) + 2) + b;
        }

        /**
        * Easing equation float for an exponential (2^t) easing out/in: deceleration until halfway, then acceleration.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseOutInExpo(float t, float b, float c, float d)
        {
            c -= b;
            if (t < d / 2) return EaseOutExpo(t * 2, b, c / 2, d);
            return EaseInExpo((t * 2) - d, b + c / 2, c / 2, d);
        }

        /**
        * Easing equation float for a circular (sqrt(1-t^2)) easing in: accelerating from zero velocity.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseInCirc(float t, float b, float c, float d)
        {
            c -= b;
            return -c * (Mathf.Sqrt(1 - (t /= d) * t) - 1) + b;
        }

        /**
        * Easing equation float for a circular (sqrt(1-t^2)) easing out: decelerating from zero velocity.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseOutCirc(float t, float b, float c, float d)
        {
            c -= b;
            return c * Mathf.Sqrt(1 - (t = t / d - 1) * t) + b;
        }

        /**
        * Easing equation float for a circular (sqrt(1-t^2)) easing in/out: acceleration until halfway, then deceleration.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseInOutCirc(float t, float b, float c, float d)
        {
            c -= b;
            if ((t /= d / 2) < 1) return -c / 2 * (Mathf.Sqrt(1 - t * t) - 1) + b;
            return c / 2 * (Mathf.Sqrt(1 - (t -= 2) * t) + 1) + b;
        }

        /**
        * Easing equation float for a circular (sqrt(1-t^2)) easing out/in: deceleration until halfway, then acceleration.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseOutInCirc(float t, float b, float c, float d)
        {
            c -= b;
            if (t < d / 2) return EaseOutCirc(t * 2, b, c / 2, d);
            return EaseInCirc((t * 2) - d, b + c / 2, c / 2, d);
        }

        /**
        * Easing equation float for an elastic (exponentially decaying sine wave) easing in: accelerating from zero velocity.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @param a		Amplitude.
        * @param p		Period.
        * @return		The correct value.
        */
        public static float EaseInElastic(float t, float b, float c, float d)
        {
            c -= b;
            if (t == 0) return b;
            if ((t /= d) == 1) return b + c;
            float p = d * .3f;
            float s = 0;
            float a = 0;
            if (a == 0f || a < Mathf.Abs(c))
            {
                a = c;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(c / a);
            }
            return -(a * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + b;
        }

        /**
        * Easing equation float for an elastic (exponentially decaying sine wave) easing out: decelerating from zero velocity.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @param a		Amplitude.
        * @param p		Period.
        * @return		The correct value.
        */
        public static float EaseOutElastic(float t, float b, float c, float d)
        {
            c -= b;
            if (t == 0) return b;
            if ((t /= d) == 1) return b + c;
            float p = d * .3f;
            float s = 0;
            float a = 0;
            if (a == 0f || a < Mathf.Abs(c))
            {
                a = c;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(c / a);
            }
            return (a * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) + c + b);
        }

        /*
        * Easing equation float for an elastic (exponentially decaying sine wave) easing in/out: acceleration until halfway, then deceleration.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @param a		Amplitude.
        * @param p		Period.
        * @return		The correct value.
        */
        public static float EaseInOutElastic(float t, float b, float c, float d)
        {
            c -= b;
            if (t == 0) return b;
            if ((t /= d / 2) == 2) return b + c;
            float p = d * (.3f * 1.5f);
            float s = 0;
            float a = 0;
            if (a == 0f || a < Mathf.Abs(c))
            {
                a = c;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(c / a);
            }
            if (t < 1) return -.5f * (a * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + b;
            return a * Mathf.Pow(2, -10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) * .5f + c + b;
        }

        /**
        * Easing equation float for an elastic (exponentially decaying sine wave) easing out/in: deceleration until halfway, then acceleration.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @param a		Amplitude.
        * @param p		Period.
        * @return		The correct value.
        */
        public static float EaseOutInElastic(float t, float b, float c, float d)
        {
            c -= b;
            if (t < d / 2) return EaseOutElastic(t * 2, b, c / 2, d);
            return EaseInElastic((t * 2) - d, b + c / 2, c / 2, d);
        }

        /**
        * Easing equation float for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in: accelerating from zero velocity.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @param s		Overshoot ammount: higher s means greater overshoot (0 produces cubic easing with no overshoot, and the default value of 1.70158 produces an overshoot of 10 percent).
        * @return		The correct value.
        */
        public static float EaseInBack(float t, float b, float c, float d)
        {
            c -= b;
            float s = 1.70158f;
            return c * (t /= d) * t * ((s + 1) * t - s) + b;
        }

        /**
        * Easing equation float for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out: decelerating from zero velocity.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @param s		Overshoot ammount: higher s means greater overshoot (0 produces cubic easing with no overshoot, and the default value of 1.70158 produces an overshoot of 10 percent).
        * @return		The correct value.
        */
        public static float EaseOutBack(float t, float b, float c, float d)
        {
            c -= b;
            float s = 1.70158f;
            return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
        }

        /**
        * Easing equation float for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in/out: acceleration until halfway, then deceleration.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @param s		Overshoot ammount: higher s means greater overshoot (0 produces cubic easing with no overshoot, and the default value of 1.70158 produces an overshoot of 10 percent).
        * @return		The correct value.
        */
        public static float EaseInOutBack(float t, float b, float c, float d)
        {
            c -= b;
            float s = 1.70158f;
            if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b;
            return c / 2 * ((t -= 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b;
        }

        /**
        * Easing equation float for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out/in: deceleration until halfway, then acceleration.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @param s		Overshoot ammount: higher s means greater overshoot (0 produces cubic easing with no overshoot, and the default value of 1.70158 produces an overshoot of 10 percent).
        * @return		The correct value.
        */
        public static float EaseOutInBack(float t, float b, float c, float d)
        {
            c -= b;
            if (t < d / 2) return EaseOutBack(t * 2, b, c / 2, d);
            return EaseInBack((t * 2) - d, b + c / 2, c / 2, d);
        }

        /**
        * Easing equation float for a bounce (exponentially decaying parabolic bounce) easing in: accelerating from zero velocity.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseInBounce(float t, float b, float c, float d)
        {
            c -= b;
            return c - EaseOutBounce(d - t, 0, c, d) + b;
        }

        /**
        * Easing equation float for a bounce (exponentially decaying parabolic bounce) easing out: decelerating from zero velocity.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseOutBounce(float t, float b, float c, float d)
        {
            c -= b;
            if ((t /= d) < (1 / 2.75f))
            {
                return c * (7.5625f * t * t) + b;
            }
            else if (t < (2 / 2.75f))
            {
                return c * (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f) + b;
            }
            else if (t < (2.5f / 2.75f))
            {
                return c * (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f) + b;
            }
            else
            {
                return c * (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f) + b;
            }
        }

        /**
        * Easing equation float for a bounce (exponentially decaying parabolic bounce) easing in/out: acceleration until halfway, then deceleration.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseInOutBounce(float t, float b, float c, float d)
        {
            c -= b;
            if (t < d / 2) return EaseInBounce(t * 2, 0, c, d) * .5f + b;
            else return EaseOutBounce(t * 2 - d, 0, c, d) * .5f + c * .5f + b;
        }

        /**
        * Easing equation float for a bounce (exponentially decaying parabolic bounce) easing out/in: deceleration until halfway, then acceleration.
        *
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @return		The correct value.
        */
        public static float EaseOutInBounce(float t, float b, float c, float d)
        {
            c -= b;
            if (t < d / 2) return EaseOutBounce(t * 2, b, c / 2, d);
            return EaseInBounce((t * 2) - d, b + c / 2, c / 2, d);
        }
        #endregion

        delegate float EaseDelegate(float t, float b, float c, float d);

        public static Vector3 ChangeVector(float t, Vector3 b, Vector3 c, float d, TweenerEaseType ease)
        {
            float x = methods[(int)ease](t, b.x, c.x, d);
            float y = methods[(int)ease](t, b.y, c.y, d);
            float z = methods[(int)ease](t, b.z, c.z, d);
            return new Vector3(x, y, z);
        }

        /**
        * @param t		Current time (in frames or seconds).
        * @param b		Starting value.
        * @param c		Change needed in value.
        * @param d		Expected easing duration (in frames or seconds).
        * @param Ease	EaseType
        * @return		The correct value.
        */
        public static float ChangeFloat(float t, float b, float c, float d, TweenerEaseType ease)
        {
            return methods[(int)ease](t, b, c, d);
        }
    }
}