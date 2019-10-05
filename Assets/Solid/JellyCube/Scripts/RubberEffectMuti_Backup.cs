/***********************************************************************************************************
 * RUBBER EFFECT                                                                                        *
 * Changes: if the object is stopped, the vertices will sleep, saving CPU                                  *
 * by Rodrigo Pegorari - 2010 - http://rodrigopegorari.com                                                 *
 * based on the Processing 'Chain' code example (http://www.processing.org/learning/topics/chain.html)     *
 ***********************************************************************************************************/

using UnityEngine;
using System.Collections;

namespace JellyCube
{
    public class RubberEffectMuti_Backup : MonoBehaviour
    {
        public RubberType m_Presets;

        public enum RubberType
        {
            Custom,
            RubberDuck,
            HardRubber,
            Jelly,
            SoftLatex
        }

        public float m_EffectIntensity = 1;
        public float m_Damping = 0.7f;
        public float m_Mass = 1;
        public float m_Stiffness = 0.2f;

        [System.Serializable]
        private class Unit
        {
            public Transform target;

            public Mesh WorkingMesh;
            public Mesh OriginalMesh;

            public VertexRubber[] vr;
            public Vector3[] V3_WorkingMesh;
            public MeshRenderer Renderer;
        }
        [SerializeField]
        private GameObject[] targets;
        private Unit[] units;
        [SerializeField]
        private float temp;
        [SerializeField]
        private Transform pivotTran;

        public bool sleeping = true;

        private Vector3 last_world_position;
        private Quaternion last_world_rotation;

        internal class VertexRubber
        {
            public int indexId;
            public float mass;
            public float stiffness;
            public float damping;
            public float intensity;
            public Vector3 pos;
            public Vector3 target;
            public Vector3 force;
            public Vector3 acc;
            private bool v_sleeping = false;


            public bool sleeping
            {
                get { return v_sleeping; }
                set { v_sleeping = value; }
            }

            private const float STOP_LIMIT = 0.001f;

            Vector3 vel = new Vector3();

            public VertexRubber(Vector3 v_target, float m, float s, float d)
            {
                mass = m;
                stiffness = s;
                damping = d;
                intensity = 1;
                pos = target = v_target;
                sleeping = false;
            }

            public void update(Vector3 target)
            {
                if (v_sleeping)
                {
                    return;
                }

                force = (target - pos) * stiffness;
                acc = force / mass;
                vel = (vel + acc) * damping;
                pos += vel;

                if ((vel + force + acc).magnitude < STOP_LIMIT)
                {
                    pos = target;
                    v_sleeping = true;
                }
            }
        }

        void OnValidate()
        {
            checkPreset();
        }

        void Start()
        {
            checkPreset();
            if (pivotTran == null) pivotTran = transform;
            units = new Unit[targets.Length];

            for (int i = 0; i < units.Length; i++)
            {
                Debug.Log("초기화" + i + targets[i].name);
                units[i] = new Unit();
                units[i].target = targets[i].GetComponent<Transform>();
                MeshFilter filter = units[i].target.GetComponent<MeshFilter>();
                units[i].OriginalMesh = filter.sharedMesh;

                units[i].WorkingMesh = Instantiate(filter.sharedMesh) as Mesh;
                filter.sharedMesh = units[i].WorkingMesh;

                ArrayList ActiveVertex = new ArrayList();

                for (int j = 0; j < units[i].WorkingMesh.vertices.Length; j++)
                {
                    ActiveVertex.Add(j);
                }

                units[i].vr = new VertexRubber[ActiveVertex.Count];

                for (int j = 0; j < ActiveVertex.Count; j++)
                {
                    int ref_index = (int)ActiveVertex[j];
                    units[i].vr[j] = new VertexRubber(transform.TransformPoint(units[i].WorkingMesh.vertices[ref_index]), m_Mass, m_Stiffness, m_Damping);
                    units[i].vr[j].indexId = ref_index;
                }

                units[i].Renderer = units[i].target.GetComponent<MeshRenderer>();
            }
            WakeUp();
        }

        void WakeUp()
        {
            for (int j = 0; j < units.Length; j++)
            {

                for (int i = 0; i < units[j].vr.Length; i++)
                {
                    units[j].vr[i].sleeping = false;
                }
            }

            sleeping = false;
        }

        void FixedUpdate()
        {
            if ((this.transform.position != last_world_position || this.transform.rotation != last_world_rotation))
            {
                WakeUp();
            }

            if (!sleeping)
            {
                int v_sleeping_counter = 0;
                int vrAll = 0;
                for (int k = 0; k < units.Length; k++)
                {

                    units[k].V3_WorkingMesh = units[k].OriginalMesh.vertices;

                    vrAll += units[k].vr.Length;
                    for (int i = 0; i < units[k].vr.Length; i++)
                    {
                        if (units[k].vr[i].sleeping)
                        {
                            v_sleeping_counter++;
                        }
                        else
                        {
                            Vector3 V3_MeshPos = units[k].V3_WorkingMesh[units[k].vr[i].indexId];
                            Vector3 v3_target = transform.TransformPoint(V3_MeshPos);

                            units[k].vr[i].mass = m_Mass;
                            units[k].vr[i].stiffness = m_Stiffness;
                            units[k].vr[i].damping = m_Damping;

                            units[k].vr[i].intensity = (1 - (temp - (v3_target.y - pivotTran.position.y)) / temp) * m_EffectIntensity;
                            units[k].vr[i].update(v3_target);

                            v3_target = transform.InverseTransformPoint(units[k].vr[i].pos);

                            units[k].V3_WorkingMesh[units[k].vr[i].indexId] = Vector3.Lerp(units[k].V3_WorkingMesh[units[k].vr[i].indexId], v3_target, units[k].vr[i].intensity);

                        }
                    }

                    units[k].WorkingMesh.vertices = units[k].V3_WorkingMesh;


                }
                if (this.transform.position == last_world_position && this.transform.rotation == last_world_rotation && v_sleeping_counter == vrAll)
                {
                    sleeping = true;
                }
                else
                {
                    last_world_position = this.transform.position;
                    last_world_rotation = this.transform.rotation;
                }
            }
        }
        /*
        void OnDrawGizmos()
        {
            if (vr == null){
                return;
            }

            for (int i = 0; i < vr.Length; i++)
            {
                Gizmos.color = new Color(vr[i].v_intensity, vr[i].v_intensity, vr[i].v_intensity);
                Gizmos.DrawCube(transform.TransformPoint(WorkingMesh.vertices[vr[i].indexId]), Vector3.one * 0.05f);
            }
        }*/

        void checkPreset()
        {
            switch (m_Presets)
            {
                case RubberType.HardRubber:
                    m_Mass = 8f;
                    m_Stiffness = 0.5f;
                    m_Damping = 0.9f;
                    m_EffectIntensity = 0.5f;
                    break;
                case RubberType.Jelly:
                    m_Mass = 1f;
                    m_Stiffness = 0.95f;
                    m_Damping = 0.95f;
                    m_EffectIntensity = 1f;
                    break;
                case RubberType.RubberDuck:
                    m_Mass = 2f;
                    m_Stiffness = 0.5f;
                    m_Damping = 0.85f;
                    m_EffectIntensity = 1f;
                    break;
                case RubberType.SoftLatex:
                    m_Mass = 0.9f;
                    m_Stiffness = 0.3f;
                    m_Damping = 0.25f;
                    m_EffectIntensity = 1f;
                    break;
            }

            m_Mass = Mathf.Max(m_Mass, 0);
            m_Stiffness = Mathf.Max(m_Stiffness, 0);
            //m_Damping = Mathf.Clamp(m_Damping, 0, 1);
            m_EffectIntensity = Mathf.Clamp(m_EffectIntensity, 0, 1);
        }
    }
}