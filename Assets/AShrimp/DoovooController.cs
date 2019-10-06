
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PathReceiver), typeof(KeyStrokeInputController))]
public class DoovooController : MonoBehaviour
{
    [SerializeField]
    private float _MoveSpeed;
    [SerializeField]
    private float _RotationSpeed;
    [SerializeField]
    private float _ImpactVectorSpeed;
    [SerializeField]
    private float _MaximumSpeed;

    private float nAngularSpeed;
    private Rigidbody sRigidbody;
    private PathReceiver sPathReceiver;
    private KeyStrokeInputController sKeyStrokeInputController;
    private SpeedController sSpeedController;

    private void Awake()
    {
        this.sRigidbody = this.GetComponent<Rigidbody>();
        this.sPathReceiver = this.GetComponent<PathReceiver>();
        this.sKeyStrokeInputController = this.GetComponent<KeyStrokeInputController>();
        this.sSpeedController = new SpeedController(this._MaximumSpeed);
#if UNITY_ANDROID
        _RotationSpeed = 80f;
#endif

    }

    private void FixedUpdate()
    {
        this.sSpeedController.update();

        var nAdditionalSpeed = this._MoveSpeed * (this.sKeyStrokeInputController.LeftSpeed + this.sKeyStrokeInputController.RightSpeed);

        this.nAngularSpeed = this._RotationSpeed * (this.sKeyStrokeInputController.LeftSpeed - this.sKeyStrokeInputController.RightSpeed);

        this.transform.Rotate(Vector3.up, this.nAngularSpeed * Time.deltaTime, Space.Self);

        //Add base velocity.
        this.sRigidbody.AddForce(this.sPathReceiver.PathDirection * this.sPathReceiver.PathSpeed, ForceMode.Acceleration);

        //Add directional velocity.
        this.sRigidbody.AddForce(this.transform.forward * nAdditionalSpeed, ForceMode.Acceleration);

        //Add some drag.
        this.sRigidbody.AddForce(Mathf.Min(0f, this.sSpeedController.Speed - this.sRigidbody.velocity.magnitude) * this.sRigidbody.velocity, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Wall"))
            return;

        var sContact = collision.GetContact(0);
        var sNormal = sContact.normal;

        this.sRigidbody.AddForce(this._ImpactVectorSpeed * sNormal - this.sRigidbody.velocity, ForceMode.VelocityChange);
        this.sSpeedController.restartAnimation();

        DoovooCrachEffect.crachCall(Vector3.Dot(sContact.point - this.transform.position, this.transform.right) < 0f);
    }
}