/***********************************************************************************************************
 * JELLY CUBE - GAME STARTER KIT - Compatible with Unity 5                                                 *
 * Produced by TROPIC BLOCKS - http://www.tropicblocks.com - http://www.twitter.com/tropicblocks           *
 * Developed by Rodrigo Pegorari - http://www.rodrigopegorari.com                                          *
 ***********************************************************************************************************/

using UnityEngine;
using System.Collections;

namespace JellyCube
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;

        public Transform m_LookAtTransform;
        
        public float m_LookAtDamping = 2f; //Damping of look at, when a object is selected;

        public float m_CameraMoveDamping = 2f; //Damping speed of the camera 'level complete' movement

        private const float INITIAL_CAMERA_HEIGHT = 50f;

        private Vector3 m_DefaultCameraPosition;

        private Vector3 m_LookAtTarget;

        private bool m_LevelComplete = false;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            m_DefaultCameraPosition = transform.position;

            //move the camera to the Initial Height position and lerp (on LateUpdate) to the m_DefaultCameraPosition
            transform.position += new Vector3(0, INITIAL_CAMERA_HEIGHT, 0);
        }

        void LateUpdate()
        {
            transform.position = Vector3.Slerp(transform.position, m_DefaultCameraPosition, Time.deltaTime * m_CameraMoveDamping);

            if (m_LookAtTransform != null && !m_LevelComplete)
            {
                m_LookAtTarget = Vector3.Lerp(m_LookAtTarget, m_LookAtTransform.position, Time.deltaTime * m_LookAtDamping);
            }
            else if (m_LevelComplete)
            {
                m_LookAtTarget = Vector3.Lerp(m_LookAtTarget, Vector3.zero, Time.deltaTime * m_LookAtDamping);
            }

            if (m_LookAtTransform != null)
            {
                transform.LookAt(m_LookAtTarget);
            }
        }

        public void SetLevelCompleteCamera()
        {
            m_LevelComplete = true;
        }

    }
}