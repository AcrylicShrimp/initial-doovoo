/***********************************************************************************************************
 * JELLY CUBE - GAME STARTER KIT - Compatible with Unity 5                                                 *
 * Produced by TROPIC BLOCKS - http://www.tropicblocks.com - http://www.twitter.com/tropicblocks           *
 * Developed by Rodrigo Pegorari - http://www.rodrigopegorari.com                                          *
 ***********************************************************************************************************/

using UnityEngine;
using System.Collections;

namespace JellyCube
{
    public class CheckPoint : MonoBehaviour
    {
        public string m_CubeTag = "Player"; //fill this variable according to the Cube tag

        [HideInInspector]
        public bool m_Success = false;

        void OnTriggerEnter(Collider collider)
        {
            //check if the cube object has the same tag of this checkpoint var 'cubeTag'
            if (collider.tag == m_CubeTag)
            {
                m_Success = true;
                GameManager.Instance.CheckGame();
            }
        }

        void OnTriggerExit(Collider collider)
        {
            m_Success = false;
        }
    }
}