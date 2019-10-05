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
    public class RubberEffect : MonoBehaviour
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

        private Mesh WorkingMesh;
        private Mesh OriginalMesh;

        private VertexRubber[] vr;
        private Vector3[] V3_WorkingMesh;
        private MeshRenderer Renderer;

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

            MeshFilter filter = (MeshFilter)GetComponent(typeof(MeshFilter));
            OriginalMesh = filter.sharedMesh;

            WorkingMesh = Instantiate(filter.sharedMesh) as Mesh;
            filter.sharedMesh = WorkingMesh;

            ArrayList ActiveVertex = new ArrayList();

            for (int i = 0; i < WorkingMesh.vertices.Length; i++)
            {
                ActiveVertex.Add(i);
            }

            vr = new VertexRubber[ActiveVertex.Count];

            for (int i = 0; i < ActiveVertex.Count; i++)
            {
                int ref_index = (int)ActiveVertex[i];
                vr[i] = new VertexRubber(transform.TransformPoint(WorkingMesh.vertices[ref_index]), m_Mass, m_Stiffness, m_Damping);
                vr[i].indexId = ref_index;
            }

            Renderer = GetComponent<MeshRenderer>();

            WakeUp();
        }

        void WakeUp()
        {
            for (int i = 0; i < vr.Length; i++)
            {
                vr[i].sleeping = false;
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
                V3_WorkingMesh = OriginalMesh.vertices;

                int v_sleeping_counter = 0;

                for (int i = 0; i < vr.Length; i++)
                {
                    if (vr[i].sleeping)
                    {
                        v_sleeping_counter++;
                    }
                    else
                    {
                        Vector3 v3_target = transform.TransformPoint(V3_WorkingMesh[vr[i].indexId]);

                        vr[i].mass = m_Mass;
                        vr[i].stiffness = m_Stiffness;
                        vr[i].damping = m_Damping;

                        vr[i].intensity = (1 - (Renderer.bounds.max.y - v3_target.y) / Renderer.bounds.size.y) * m_EffectIntensity;
                        vr[i].update(v3_target);

                        v3_target = transform.InverseTransformPoint(vr[i].pos);

                        V3_WorkingMesh[vr[i].indexId] = Vector3.Lerp(V3_WorkingMesh[vr[i].indexId], v3_target, vr[i].intensity);

                    }
                }

                WorkingMesh.vertices = V3_WorkingMesh;

                if (this.transform.position == last_world_position && this.transform.rotation == last_world_rotation && v_sleeping_counter == vr.Length)
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