
using UnityEngine;

public class PathReceiver : MonoBehaviour
{
    public Vector3 PathNormal { get; private set; }
    public Vector3 PathDirection { get; private set; }

    private void Awake()
    {
        this.PathNormal = Vector3.down;
        this.PathDirection = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        this.PathNormal = other.transform.rotation * Vector3.up;
        this.PathDirection = other.transform.rotation * Vector3.forward;
    }
}