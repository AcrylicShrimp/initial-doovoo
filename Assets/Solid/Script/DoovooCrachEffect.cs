using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoovooCrachEffect : MonoBehaviour
{
    [System.Serializable]
    private class Part
    {
        public PartLocation location;
        public GameObject[] junks;
        private Rigidbody[] junkRigids;
        private Transform[] junkTrans;
        public GameObject particle;
        public void init()
        {
            junkRigids = new Rigidbody[junks.Length];
            junkTrans = new Transform[junks.Length];
            for (int i = 0; i < junks.Length; i++)
            {
                junkRigids[i] = junks[i].GetComponent<Rigidbody>();
                junkTrans[i] = junks[i].GetComponent<Transform>();
            }
        }
        public void explosion()
        {

        }
    }
    [SerializeField]
    private Part[] parts;
    private void Awake()
    {
        for (int i = 0; i < parts.Length; i++)
        {

        }
    }
}
public enum PartLocation
{
    R_1 = 1, R_2 = 2, R_3 = 3,
    L_1 = -1, L_2 = -2, L_3 = -3,
}