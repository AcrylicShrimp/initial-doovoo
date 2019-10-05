/***********************************************************************************************************
 * JELLY CUBE - GAME STARTER KIT - Compatible with Unity 5                                                 *
 * Produced by TROPIC BLOCKS - http://www.tropicblocks.com - http://www.twitter.com/tropicblocks           *
 * Developed by Rodrigo Pegorari - http://www.rodrigopegorari.com                                          *
 ***********************************************************************************************************/

using UnityEngine;
using System.Collections;

namespace JellyCube
{
    public class JellyDecal : MonoBehaviour
    {
        public float m_Tint = 1;
        
        public float m_FadeDamping = 1f;

        private const float TINT_LIMIT = 0.05f;

        void Start()
        {
            m_Tint = GetComponent<Renderer>().material.GetColor("_Color").a;
        }

        void Update()
        {
            m_Tint = Mathf.Lerp(m_Tint, 0f, Time.deltaTime * m_FadeDamping);

            Color color = GetComponent<Renderer>().material.GetColor("_Color");
            
            GetComponent<Renderer>().material.SetColor("_Color", new Color(color.r, color.g, color.b, m_Tint));

            if (m_Tint < TINT_LIMIT)
            {
                Destroy(gameObject);
            }
        }
    }
}
