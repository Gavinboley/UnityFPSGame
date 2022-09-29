using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    public CharacterController controller;
    public Transform shootOrigin;
    public float gravity = -9.81f;
    public float moveSpeed = 5f;
    public float jumpSpeed = 5f;
    public float sprintMultiplier = 1.8f;
    public float health;
    public float maxHealth = 100f;
    public float fireRate = 0.5f;
    private float lastShot = 0.0f;

    private bool[] inputs;
    private float yVelocity = 0;

    private void Start()
    {
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;
        jumpSpeed *= Time.fixedDeltaTime;
    }

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;

        inputs = new bool[6];
    }

    public void FixedUpdate()
    {
        if (health <= 0f)
        {
            return;
        }

        Vector2 _inputDirection = Vector2.zero;
        if (inputs[0])
        {
            _inputDirection.y += 1;
        }
        if (inputs[1])
        {
            _inputDirection.y -= 1;
        }
        if (inputs[2])
        {
            _inputDirection.x -= 1;
        }
        if (inputs[3])
        {
            _inputDirection.x += 1;
        }
        if (inputs[5])
        {
            _inputDirection.x *= sprintMultiplier;
            _inputDirection.y *= sprintMultiplier;
        }

        Move(_inputDirection);
    }

    private void Move(Vector2 _inputDirection)
    {
        Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        _moveDirection *= moveSpeed;

        if (controller.isGrounded)
        {
            yVelocity = 0f;
            if (inputs[4])
            {
                yVelocity = jumpSpeed;
            }
        }
        yVelocity += gravity;

        _moveDirection.y = yVelocity;
        controller.Move(_moveDirection);

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        inputs = _inputs;
        transform.rotation = _rotation;
    }

    public void Shoot(Vector3 _viewDirection)
    {
        if (health > 0 && Time.time > fireRate + lastShot)
        {
            if (Physics.Raycast(shootOrigin.position, _viewDirection, out RaycastHit _hit, 500f))
            {
                if (_hit.collider.CompareTag("Player"))
                {
                    _hit.collider.GetComponent<Player>().TakeDamage(50f);
                    lastShot = Time.time;
                }
            }
        }

    }

    public void TakeDamage(float _damage)
    {
        if (health <= 0f)
        {
            return;
        }

        health -= _damage;
        if (health <= 0f)
        {
            health = 0f;
            controller.enabled = false;
            transform.position = RandomSpawn();
            ServerSend.PlayerPosition(this);
            StartCoroutine(Respawn());
        }
        ServerSend.PlayerHealth(this);
    }

    public Vector3 RandomSpawn()
    {
        Random pos = new Random(); 
        int Pos = UnityEngine.Random.Range(1, 5);
        if (Pos == 1)
        {

            return new Vector3(48f, 3f, 48f);
        }
        if (Pos == 2)
        {
            return new Vector3(8f, 3f, 48f);
        }
        if (Pos == 3)
        {
            return new Vector3(8f, 3f, -48f);
        }
        if (Pos == 4)
        {
            return new Vector3(48f, 3f, -48f);
        }
        else
        {
            Debug.Log("Player is not going where he should");
            return new Vector3(48f, 3f, 48f);
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5f); 

        health = maxHealth;
        controller.enabled = true;
        ServerSend.PlayerRespawned(this);
    }
}
