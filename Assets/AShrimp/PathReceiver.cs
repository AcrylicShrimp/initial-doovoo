
using UnityEngine;

public class PathReceiver : MonoBehaviour
{
    public float PathSpeed { get; private set; }
    public Vector3 PathDirection { get; private set; }

    private void Awake()
    {
        this.PathSpeed = 0f;
        this.PathDirection = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        var path = other.GetComponent<Path>();
        if (path == null) return;
        this.PathSpeed = other.GetComponent<Path>()._Speed;
        this.PathDirection = other.transform.rotation * Vector3.forward;
    }
}