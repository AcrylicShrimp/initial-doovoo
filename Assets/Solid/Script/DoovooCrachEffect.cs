using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoovooCrachEffect : MonoBehaviour
{
    public static Action<bool> crachCall;
    [SerializeField]
    private GameObject leftEffect;
    [SerializeField]
    private GameObject rightEffect;
    [Serializable]
    private class Part
    {
        public PartLocation location;
        public Transform pivot;
        //private GameObject[] junks;
        public Rigidbody[] junkRigids;
        private Transform[] junkTrans;
        public GameObject particle;
        public Animator humman;
        public AudioSource[] sounds;
        public GameObject[] timelineObj;
        public void init()
        {
            //Debug.Log("초기화 합니다" + location.ToString());
            //junks = pivot.GetComponentsInChildren<GameObject>();
            junkRigids = pivot.GetComponentsInChildren<Rigidbody>();
            junkTrans = new Transform[junkRigids.Length];
            for (int i = 0; i < junkRigids.Length; i++)
            {
                //junkRigids[i] = junks[i].GetComponent<Rigidbody>();
                junkTrans[i] = junkRigids[i].GetComponent<Transform>();
            }
        }
        public Transform[] explosion(Vector3 dir, float force, float speeder)
        {
            Vector3 explosionPoint = (dir + Vector3.down * 0.5f).normalized;//pivot.InverseTransformPoint(-dir);
            //Debug.Log(explosionPoint);
            particle.transform.SetParent(null);
            particle.SetActive(true);
            GuageCtroller.instance.removeHuman(humman);

            humman.SetFloat("AddSpeed", 15f);
            for (int i = 0; i < sounds.Length; i++)
            {
                Debug.Log("사운드!" + sounds[i].gameObject.name);
                sounds[i].Play();
            }
            for (int i = 0; i < timelineObj.Length; i++)
            {
                timelineObj[i].SetActive(false);
            }
            for (int i = 0; i < junkRigids.Length; i++)
            {
                junkRigids[i].isKinematic = false;
                junkRigids[i].AddExplosionForce(force, explosionPoint + pivot.position, 100f);
                junkRigids[i].AddForce(Vector3.forward * speeder * 30f);
                junkRigids[i].AddTorque(new Vector3(UnityEngine.Random.Range(-180f, 180f), UnityEngine.Random.Range(-180f, 180f),
                       UnityEngine.Random.Range(-180f, 180f)));
                junkTrans[i].SetParent(null);
            }
            return junkTrans;
        }

    }
    [SerializeField]
    private Part[] parts;
    [SerializeField]
    private float explosionForce;
    private Dictionary<PartLocation, Part> partDic = new Dictionary<PartLocation, Part>();
    private int rightCount;
    private int leftCount;
    private PathReceiver pathReceiver;

    public float m_EffectIntensity = 1;
    public float m_Damping = 0.7f;
    public float m_Mass = 1;
    public float m_Stiffness = 0.2f;

    public int lifeCount { get { return rightCount + leftCount; } }

    private Vector3 agoPos;
    private void Awake()
    {
        crachCall = crachEffect;
        agoPos = transform.position;
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].init();
            partDic.Add(parts[i].location, parts[i]);
        }
        rightCount = 3;
        leftCount = 3;
        pathReceiver = GetComponentInParent<PathReceiver>();
    }
    public void crachEffect(bool isRight)
    {
        Vector3 dir = agoPos - transform.position;

        if ((isRight && rightCount == 0) || (!isRight && leftCount == 0))
            isRight = !isRight;

        if (isRight && rightCount > 0)
        {
            StartCoroutine(disableJunk(
              partDic[(PartLocation)(3 - --rightCount)].explosion((dir + Vector3.right * 0.1f).normalized, explosionForce,
              Vector3.Distance(agoPos, transform.position))
                   ));
            if (rightCount == 0) rightEffect.SetActive(false);
            CleraManager.instance.hpCount -= 1;
        }
        else if (leftCount > 0)
        {
            StartCoroutine(disableJunk(
               partDic[(PartLocation)(-3 + --leftCount)].explosion((dir - Vector3.right * 0.1f).normalized, explosionForce,
               Vector3.Distance(agoPos, transform.position))
             ));
            if (leftCount == 0) leftEffect.SetActive(false);
            CleraManager.instance.hpCount -= 1;
        }
    }
    private void LateUpdate()
    {
        agoPos = transform.position;
    }

    //private void OnGUI()
    //{
    //    if (GUILayout.Button("Right"))
    //    {
    //        crachEffect(true);
    //    }
    //    if (GUILayout.Button("Left"))
    //    {
    //        crachEffect(false);
    //    }
    //}

    public IEnumerator disableJunk(Transform[] junkTrans)
    {
        yield return new WaitForSeconds(4f);
        Debug.Log("꺼주기");
        for (int i = 0; i < junkTrans.Length; i++)
        {
            junkTrans[i].gameObject.SetActive(false);
        }
    }
}
public enum PartLocation
{
    R_1 = 1, R_2 = 2, R_3 = 3,
    L_1 = -1, L_2 = -2, L_3 = -3,
}