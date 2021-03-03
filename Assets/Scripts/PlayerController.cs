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
    public GameObject PlayerPrefab;
    
    private Rigidbody _body;
    private Transform _groundChecker;
    private Vector3 _inputs = Vector3.zero;
    private bool _isGrounded = true;
    private int _grenadeQuantity = 0;
    private Animator _animator;

    private Vector3 _playerStartPosition;
    private Vector3 _cameraStartPosition;
    private GameObject _canvas;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _body = GetComponent<Rigidbody>();
        //SetRagdollParts();
        //SetColliderSpheres();
    }

    private void Start()
    {
        _groundChecker = Root.transform;
        
        _playerStartPosition = this.gameObject.transform.position;
        _cameraStartPosition = Camera.main.transform.position;

        _canvas = GameObject.FindWithTag("Canvas UI");
        
        Root.SetActive(false);

        _animator.enabled = true;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
    }

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        _inputs = Vector3.zero;
        if (!isRagdoll())
        {
            _inputs.x = Input.GetAxis("Horizontal");
        }
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
            Die();
        }
    }

    private void FixedUpdate()
    {
        _body.MovePosition(_body.position + _inputs * Speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            Debug.Log("PickUp");
            _grenadeQuantity++;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Failure")
        {
            Die();
        }
        if (other.gameObject.tag == "Finish")
        {
            Finish();
        }
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

    public void Die()
    {
        Debug.Log("Morel");
        SetRagdoll(true);
        StartCoroutine(WaitAndRespawn(3.0f));
    }

    private bool isRagdoll()
    {
        return !_animator.enabled;
    }

    public void SetRagdoll(bool isEnabled)
    {
        Root.SetActive(isEnabled);

        //_body.isKinematic = isEnabled;
        _animator.enabled = !isEnabled;
        GetComponent<BoxCollider>().enabled = !isEnabled;
        GetComponent<CapsuleCollider>().enabled = !isEnabled;
    }

    private IEnumerator WaitAndRespawn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Voutol");
        //SetRagdoll(false);
        GameObject player = Instantiate(PlayerPrefab, _playerStartPosition, PlayerPrefab.transform.rotation);
        Camera camera = Camera.main;
        camera.GetComponent<CameraController>().SetPlayer(player);
        camera.transform.position = _cameraStartPosition;
        //_body.MovePosition(_playerStartPosition);
        Destroy(this);
    }

    private void Finish()
    {
        _canvas.GetComponent<PauseMenu>().EndgameMenuObject.SetActive(true);
    }
}
