
using UnityEngine;

[RequireComponent(typeof(PathReceiver), typeof(Rigidbody))]
public class DoovooController : MonoBehaviour
{
    public enum ImpactDirection
    {
        Left,
        Right
    }

    [SerializeField]
    private float _RotationSpeed;
    [SerializeField]
    private float _BaseVectorSpeed;
    [SerializeField]
    private float _ImpactVectorSpeed;
    [SerializeField]
    private float _MaximumSpeed;

    private float nAngularSpeed;
    private SpeedController sSpeedController;
    private PathReceiver sPathReceiver;
    private Rigidbody sRigidbody;

    private void Awake()
    {
        this.sSpeedController = new SpeedController(this._MaximumSpeed);
        this.sPathReceiver = this.GetComponent<PathReceiver>();
        this.sRigidbody = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        this.sSpeedController.update();

        float nAdditionalSpeed = 0f;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                nAdditionalSpeed += this._BaseVectorSpeed * .5f;

            if (Input.GetKey(KeyCode.RightArrow))
                nAdditionalSpeed += this._BaseVectorSpeed * .5f;

            if (Input.GetKey(KeyCode.LeftArrow) ^ Input.GetKey(KeyCode.RightArrow))
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                    this.nAngularSpeed = -this._RotationSpeed;
                else
                    this.nAngularSpeed = this._RotationSpeed;
            }
        }

        this.transform.Rotate(Vector3.up, this.nAngularSpeed * Time.deltaTime, Space.Self);

        //Add base velocity.
        this.sRigidbody.AddForce(this.sPathReceiver.PathDirection * this._BaseVectorSpeed, ForceMode.Acceleration);

        //Add directional velocity.
        this.sRigidbody.AddForce(this.transform.forward * nAdditionalSpeed, ForceMode.Acceleration);

        //Add some drag.
        this.sRigidbody.AddForce(Mathf.Min(0f, this.sSpeedController.Speed - Vector3.Dot(this.sRigidbody.velocity, this.sPathReceiver.PathDirection)) * this.sPathReceiver.PathDirection * this._BaseVectorSpeed, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Wall"))
            return;

        var sContact = collision.GetContact(0);
        var sNormal = sContact.normal;

        this.sRigidbody.AddForce(this._ImpactVectorSpeed * sNormal - this.sRigidbody.velocity, ForceMode.VelocityChange);
        this.sSpeedController.restartAnimation();

        ImpactDirection sImpactDirection;

        if (Vector3.Dot(sContact.point - this.transform.position, this.transform.right) < 0f)
            sImpactDirection = ImpactDirection.Left;
        else
            sImpactDirection = ImpactDirection.Right;

        Debug.Log(sImpactDirection);
    }
}