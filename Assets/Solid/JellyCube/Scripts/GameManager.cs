/***********************************************************************************************************
 * JELLY CUBE - GAME STARTER KIT - Compatible with Unity 5                                                 *
 * Produced by TROPIC BLOCKS - http://www.tropicblocks.com - http://www.twitter.com/tropicblocks           *
 * Developed by Rodrigo Pegorari - http://www.rodrigopegorari.com                                          *
 ***********************************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace JellyCube
{
    /// <summary>
    /// This is the Main class of the game
    /// Here you set what happens when all Checkpoints are complete
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        //Next level name to be loaded after the level completion
        public string m_NextLevelName;

        public Animator m_Animator;

        private Dictionary<string,float> m_AnimationClipsLength =  new Dictionary<string,float>();

        //Array of all scene checkpoints
        private CheckPoint[] m_Checkpoints;

        private bool m_CheckGame = false;

        private CameraController m_CameraController;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            //Get all checkpoints of scene
            m_Checkpoints = GameObject.FindObjectsOfType(typeof(CheckPoint)) as CheckPoint[];

            foreach (AnimationClip clip in m_Animator.runtimeAnimatorController.animationClips)
            {
                m_AnimationClipsLength.Add(clip.name, clip.length);
            }

            m_CameraController = CameraController.Instance;
        }

        public void CheckLevelCompletion()
        {
            //if m_CheckGame variable is enabled, means a check box collider was triggered
            if (m_CheckGame)
            {
                int activeCheckpointsNumber = 0;

                //Check all checkpoints in scene and verify if the checkpoint was fullfilled with a box
                foreach (CheckPoint cp in m_Checkpoints)
                {
                    //m_Success is true when a Cube Collider has the same Tag name than the field m_CubeTag in the CheckPoint.cs 
                    if (cp.m_Success)
                    {
                        activeCheckpointsNumber++;
                    }
                }

                //If the number of success checkpoints is the same of the checkpoint number, the level is complete
                if (activeCheckpointsNumber == m_Checkpoints.Length)
                {
                    Debug.Log("Level complete!");

                    //Lock controls
                    InputManager.Instance.LockControls();

                    //End of level effect transition
                    StartCoroutine(LevelComplete());
                }

                m_CheckGame = false;

            }

            //In this place you can implement a rule to limit the number of moves if you wish
            //Debug.Log(CubeManager.Instance.m_NumberOfMoves);
        }

        //Check if all checkpoints are filled
        public void CheckGame()
        {
            m_CheckGame = true;
        }

        public void ReloadLevel()
        {
            StartCoroutine(ReloadLevelRoutine());
        }

        private IEnumerator ReloadLevelRoutine()
        {
            InputManager.Instance.LockControls();
            
            m_Animator.Play("LevelFadeIn");

            //Wait for the current animation time (plus one second) then load the next level
            yield return new WaitForSeconds(m_Animator.GetCurrentAnimatorStateInfo(0).length + 1);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //Finish Animation Sequence
        IEnumerator LevelComplete()
        {
            m_Animator.Play("LevelComplete");

            if (m_CameraController != null)
            {
                m_CameraController.SetLevelCompleteCamera();
            }

            //Wait for the LevelComplete UI Animation
            yield return new WaitForSeconds(m_AnimationClipsLength["LevelComplete"]);

            if (m_NextLevelName != "")
            {
                SceneManager.LoadScene(m_NextLevelName);
            }
            else
            {
                Debug.Log("Assign the var 'NextLevelName' of the component 'GameManager.cs' to load a next level.");
            }
        }
    }
}