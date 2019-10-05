/***********************************************************************************************************
 * JELLY CUBE - GAME STARTER KIT - Compatible with Unity 5                                                 *
 * Produced by TROPIC BLOCKS - http://www.tropicblocks.com - http://www.twitter.com/tropicblocks           *
 * Developed by Rodrigo Pegorari - http://www.rodrigopegorari.com                                          *
 ***********************************************************************************************************/

using UnityEngine;
using System.Collections;

namespace JellyCube
{
    //This Sound component plays a continuous song during all the game
    public class Sound : MonoBehaviour
    {
        public static bool m_Initialized = false;

        public void Awake()
        {
            //Prevent two sound components to play simultaneous
            if (m_Initialized)
            {
                Destroy(this.gameObject);
            }

            m_Initialized = true;
        }

        public void Start()
        {
            DontDestroyOnLoad(gameObject);

            GetComponent<AudioSource>().loop = true;
        }
    }
}