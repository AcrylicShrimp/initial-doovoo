/***********************************************************************************************************
 * JELLY CUBE - GAME STARTER KIT - Compatible with Unity 5                                                 *
 * Produced by TROPIC BLOCKS - http://www.tropicblocks.com - http://www.twitter.com/tropicblocks           *
 * Developed by Rodrigo Pegorari - http://www.rodrigopegorari.com                                          *
 ***********************************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JellyCube
{
    public class CubeManager : MonoBehaviour
    {
        public static CubeManager Instance;
        
        public bool m_JellyEffectEnabled = true;

        [HideInInspector]
        public int m_NumberOfMoves = 0;

        //All cubes in scene
        private List<CubeController> m_CubeControllers;

        //All cubes moving or rolling in scene
        private List<CubeController> m_CubeMoving;


        void Awake()
        {
            Instance = this;

            m_CubeControllers = new List<CubeController>();
            
            m_CubeMoving = new List<CubeController>();
        }

        //Add the CubeController into the general list. The Cubes never are removed from the list.
        public void Register(CubeController controller)
        {
            controller.m_Cube.GetComponent<RubberEffect>().enabled = m_JellyEffectEnabled;

            m_CubeControllers.Add(controller);
        }

        //Add the CubeController (that is Moving or Rolling) into the moving list
        public void RegisterMove(CubeController controller)
        {
            InputManager.Instance.LockControls();

            m_CubeMoving.Add(controller);
        }

        //Remove the CubeController (that is Moving or Rolling) from the moving list
        public void UnregisterMove(CubeController controller)
        {
            m_CubeMoving.Remove(controller);

            //When the cube moving list count is zero, means there is no cubes moving
            if (m_CubeMoving.Count == 0)
            {
                InputManager.Instance.UnlockControls();

                GameManager.Instance.CheckLevelCompletion();
            }
        }

        public void Move(Vector3 direction)
        {
            //If there is no cube interaction, can move
            if (m_CubeMoving.Count == 0)
            {
                int countMovingCubes = 0;

                foreach (CubeController controller in m_CubeControllers)
                {
                    if (controller.m_CanControl)
                    {
                        CubeController moved = controller.DoMove(direction);

                        if (moved != null)
                        {
                            countMovingCubes++;
                        }
                    }
                }

                if (countMovingCubes > 0)
                {
                    m_NumberOfMoves++;
                    //Debug.Log("Number of moves: " + m_NumberOfMoves);
                }
            }
        }

    }
}