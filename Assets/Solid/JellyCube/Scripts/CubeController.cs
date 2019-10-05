/***********************************************************************************************************
 * JELLY CUBE - GAME STARTER KIT - Compatible with Unity 5                                                 *
 * Produced by TROPIC BLOCKS - http://www.tropicblocks.com - http://www.twitter.com/tropicblocks           *
 * Developed by Rodrigo Pegorari - http://www.rodrigopegorari.com                                          *
 ***********************************************************************************************************/

using UnityEngine;
using System.Collections;

namespace JellyCube
{
    public class CubeController : MonoBehaviour
    {
        public Collider m_Cube;

        public enum MovementType
        {
            Roll,
            Slide
        }

        public enum PushType
        {
            DontPushCubes,
            PushCubesWhenMove,  //This cube will try to push another cube when it stops moving (if the another cube can move)
            PushCubesAfterMove  //This cube will try to push another cube when it starts moving (if the another cube can move)
        }

        [Header("Actions")]

        [Tooltip("This cube can be controlled (cube player)")]
        public bool m_CanControl = true;

        [Tooltip("This cube will try to push another cube when moving or after move completion (if the another cube can be pushed)")]
        public PushType m_PushCubeType = PushType.PushCubesAfterMove;

        [Header("Reactions")]

        [Tooltip("This will shake after a collision with a cube (moving or rolling) and there is no space to move")]
        public bool m_CanShake = true;

        [Tooltip("This cube will move when another cube (moving or rolling) collides")]
        public bool m_CanBePushed = true;

        [Header("Settings")]

        public MovementType m_MoveType = MovementType.Roll;
        public float m_MoveSpeed = 0.15f;

        [Header("Decals")]

        public Transform m_Trails;

        public Transform m_Splashs;

        [HideInInspector]
        public Vector3 m_LastMove = Vector3.zero;

        [HideInInspector]
        public Vector3 m_LastDir = Vector3.zero;

        private const float SHAKE_SCALE = 1.2f; 

        void Start()
        {
            CubeManager.Instance.Register(this);
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawSphere(m_LastMove, 0.1f);
        }

        Collider CheckCollisionRecursive(Transform cubeTransform, Vector3 dir)
        {
            Vector3 origin = cubeTransform.position;

            RaycastHit outHit;

            if (Physics.Linecast(origin, origin + dir, out outHit))
            {
                CubeController cubeCollider = outHit.transform.gameObject.GetComponent<CubeController>();

                if (cubeCollider != null && cubeCollider.m_CanBePushed)
                { 
                    return CheckCollisionRecursive(cubeCollider.m_Cube.transform, dir);
                }
                else
                {
                    return outHit.collider;
                }
            }
            else
            {
                return null;
            }
        }

        public CubeController DoMove(Vector3 dir)
        {
            //If there is a neighbor cube in this direction (CheckCollisionRecursive != null)
            //and this neighbor was pushed (DoPush != null)
            //then this cube can move too
            if (CheckCollisionRecursive(m_Cube.transform, dir) != null)
            {
                bool didPush = false;

                if (m_PushCubeType.Equals(PushType.PushCubesWhenMove))
                {
                    CubeController pushedCube = DoPush(dir);

                    didPush = pushedCube != null;
                }

                if (!didPush)
                {
                    return null;
                }
            }

            ResetPosition();

            CubeManager.Instance.RegisterMove(this);

            switch (m_MoveType)
            {
                case MovementType.Roll:
                    DoRoll(dir);
                    break;

                case MovementType.Slide:
                    DoSlide(dir);
                    break;
            }

            return this;
        }

        private void DoRoll(Vector3 dir)
        {
            Vector3 ndir = Vector3.zero;

            if (Mathf.Abs(dir.x) == 1)
            {
                ndir = dir * m_Cube.bounds.size.x / 2f;
            }
            else if (Mathf.Abs(dir.z) == 1)
            {
                ndir = dir * m_Cube.bounds.size.z / 2f;
            }

            Vector3 newAxis = m_Cube.ClosestPointOnBounds(m_Cube.transform.position + ndir);
            newAxis.y = m_Cube.bounds.min.y;

            Vector3 thisPos = new Vector3();
            thisPos = m_Cube.transform.position;

            Quaternion thisRot = new Quaternion();
            thisRot = m_Cube.transform.rotation;

            transform.position = newAxis;
            transform.rotation = Quaternion.identity;
                
            m_Cube.transform.rotation = thisRot;
            m_Cube.transform.position = thisPos;

            Vector3 targetRotation = new Vector3(dir.z, dir.y, -dir.x) * 90;

            m_LastDir = dir;
            m_LastMove = m_Cube.transform.position + ndir;

            //You can replace with iTween or any tweener you like
            Tweener.RotateTo(this.gameObject, transform.rotation.eulerAngles, targetRotation, m_MoveSpeed, 0, Tweener.TweenerEaseType.Linear, Complete);
        }

        private void DoSlide(Vector3 dir)
        {
            m_LastDir = dir;

            CreateSplash();

            //You can replace with iTween or any tweener you like
            Tweener.MoveTo(this.gameObject, transform.position, transform.position + dir, m_MoveSpeed, 0, Tweener.TweenerEaseType.EaseOutSine, Complete);
        }

        public CubeController DoPush(Vector3 dir)
        {
            CubeController didPush = null;

            Vector3 origin = m_Cube.transform.position;

            RaycastHit outHit = new RaycastHit();

            if (Physics.Linecast(origin, origin + dir, out outHit))
            {
                //if has any collision object, look for a CubeController its parent object, and then try to move it
                CubeController cube = outHit.collider.transform.GetComponentInParent<CubeController>();

                if (cube != null)
                {
                    if (cube.m_CanBePushed)
                    {
                        didPush = cube.DoMove(dir);
                    }
                    else if (!cube.m_PushCubeType.Equals(PushType.DontPushCubes))
                    {
                        cube.DoPush(dir);
                    }
                    
                    //if is not possible to move, then try to shake it
                    if (!didPush)
                    {
                        cube.DoShake();
                    }
                }
            }
            
            return didPush;
        }

        /// <summary>
        /// Complete Method is called after a Move event
        /// </summary>
        private void Complete()
        {
            ResetPosition();
            
            CubeManager.Instance.UnregisterMove(this);

            if (m_PushCubeType.Equals(PushType.PushCubesAfterMove))
            {
                DoPush(m_LastDir);
            }

            CreateTrail();
        }

        private void DoShake()
        {
            if (m_CanShake)
            {
                //You can replace with iTween or any tweener you like
                Tweener.ScaleTo(this.gameObject, new Vector3(SHAKE_SCALE, SHAKE_SCALE, SHAKE_SCALE), Vector3.one, .5f, 0, Tweener.TweenerEaseType.EaseOutExpo);
            }
        }

        private void ResetPosition()
        {
            //Snap angles to 90 degrees
            transform.eulerAngles = RoundVector(transform.eulerAngles, 90f);
            m_Cube.transform.localEulerAngles = RoundVector(m_Cube.transform.localEulerAngles, 90f);

            //Snap position to .5 units
            this.transform.position = RoundVector(this.transform.position, .5f);
            this.m_Cube.transform.localPosition = RoundVector(this.m_Cube.transform.localPosition, .5f);
        }

        private Vector3 RoundVector(Vector3 value, float snapValue = 1f)
        {
            value.x = Mathf.Round(value.x / snapValue) * snapValue;
            value.y = Mathf.Round(value.y / snapValue) * snapValue;
            value.z = Mathf.Round(value.z / snapValue) * snapValue;

            return value;
        }

        private void CreateTrail()
        {
            if (m_Trails == null)
            {
                return;
            }

            Vector2 trailOffset = new Vector2(Random.Range(0, 2) * 0.5f, Random.Range(0, 2) * 0.5f);
            Quaternion decalRotation = Quaternion.Euler(new Vector3(90, Random.Range(0, 4) * 90f, 0));
            GameObject trail = Instantiate(m_Trails.gameObject, new Vector3(m_Cube.transform.position.x, m_Cube.bounds.min.y + 0.01f, m_Cube.transform.position.z), decalRotation) as GameObject;
            trail.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", trailOffset);
        }

        private void CreateSplash()
        {
            if (m_Splashs == null)
            {
                return;
            }

            Vector2 splashOffset = new Vector2(Random.Range(0, 4) * 0.25f, Random.Range(0, 4) * 0.25f);
            Quaternion decalRotation = Quaternion.Euler(new Vector3(90, Random.Range(0f, 360f), 0));
            GameObject splash = Instantiate(m_Splashs.gameObject, new Vector3(m_Cube.transform.position.x, m_Cube.bounds.min.y + 0.02f, m_Cube.transform.position.z), decalRotation) as GameObject;
            splash.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", splashOffset);
        }

    }
}
 