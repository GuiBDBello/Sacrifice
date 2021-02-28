using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
    public float JumpHeight = 2f;
    public float GroundDistance = 0.2f;
    public float DashDistance = 5f;
    public LayerMask Ground;
    public Transform GroundChecker;
    public GameObject Grenade;
    public float ThrowForce = 10f;

    private Rigidbody _body;
    private Vector3 _inputs = Vector3.zero;
    private bool _isGrounded = true;

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(GroundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        _inputs = Vector3.zero;
        _inputs.x = Input.GetAxis("Horizontal");
        //_inputs.z = Input.GetAxis("Vertical");
        if (_inputs != Vector3.zero)
            transform.forward = _inputs;
            
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            Jump();
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
            Dash();
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
            ThrowGrenade();
        
        MovePlayerToStart();
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

    private void MovePlayerToStart()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Vector3 startPosition = new Vector3(0, 1f, 0);
            _body.MovePosition(startPosition);
        }
    }

    private void ThrowGrenade()
    {
        GameObject grenade = Instantiate(Grenade, transform.position + (transform.forward), transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * ThrowForce, ForceMode.VelocityChange);
    }
}
