using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
    public float JumpHeight = 2.5f;
    public float GroundDistance = 0.2f;
    public float DashDistance = 5f;
    public LayerMask Ground;
    public GameObject Grenade;
    public float ThrowForce = 10f;
    //public List<Collider> RagdollParts = new List<Collider>();
    public GameObject Root;
    
    private Rigidbody _body;
    private Transform _groundChecker;
    private Vector3 _inputs = Vector3.zero;
    private bool _isGrounded = true;
    private int _grenadeQuantity = 0;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        SetRagdoll(false);
        //SetRagdollParts();
        //SetColliderSpheres();
    }

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
        _groundChecker = Root.transform;
    }

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        _inputs = Vector3.zero;
        _inputs.x = Input.GetAxis("Horizontal");
        //_inputs.z = Input.GetAxis("Vertical");
        if (_inputs != Vector3.zero)
        {
            transform.forward = _inputs;
        }
        _animator.SetFloat("movement", _inputs.magnitude);
        _animator.SetBool("isJumping", !_isGrounded);

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            Jump();
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
            Dash();
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
            ThrowGrenade();
        
        if (Input.GetKeyDown(KeyCode.R)) {
            SetRagdoll(true);
        }
    }

    private void FixedUpdate()
    {
        _body.MovePosition(_body.position + _inputs * Speed * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        Debug.Log("Jump");
        _body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
    }

    private void Dash()
    {
        Debug.Log("Dash");
        Vector3 dashVelocity = Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime)));
        _body.AddForce(dashVelocity, ForceMode.VelocityChange);
    }

    public void PickUpGrenade()
    {
        _grenadeQuantity++;
    }

    private void ThrowGrenade()
    {
        if (_grenadeQuantity > 0)
        {
            GameObject grenade = Instantiate(Grenade, transform.position + (transform.forward), transform.rotation);
            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * ThrowForce, ForceMode.VelocityChange);
            _grenadeQuantity--;
        }
    }

    private bool isRagdoll()
    {
        return !_animator.enabled;
    }

    private void SetRagdoll(bool enabled)
    {
        Root.SetActive(enabled);

        _animator.enabled = !enabled;
        GetComponent<BoxCollider>().enabled = !enabled;
        GetComponent<CapsuleCollider>().enabled = !enabled;
    }
}
