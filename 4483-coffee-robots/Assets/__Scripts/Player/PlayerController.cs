using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region public properties

    public CharacterController controller;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform defaultTransform;
    [SerializeField] private Transform inventory;

    [Header("Audio")]
    public AudioClip dashAudio;
    public AudioClip slowAudio;

    #endregion public properties

    #region private properties

    private float nextDashTime;
    private bool dash = false;
    private float wStrafe = 0f;
    private float sStrafe = 0f;
    private float dStrafe = 0f;
    private float aStrafe = 0f;
    
    AudioSource movementSource;
    Camera mainCamera;

    private Vector3 playerVelocity;
    private float unSlowTime;

    #endregion private properties

    void Awake()
    {
        movementSource = defaultTransform.gameObject.GetComponent<AudioSource>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Start()
    {
        movementSource.playOnAwake = false;
        movementSource.spatialBlend = 1f;
        movementSource.volume = 0.7f;
        movementSource.priority = 128;
    }

    void Update()
    {
        Vector2 cursorPos = Mouse.current.position.ReadValue(); // get cursor position
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(cursorPos.x, cursorPos.y, mainCamera.transform.position.y)); // map to world coordinates
        if (Physics.Raycast(mainCamera.gameObject.transform.position, (worldPos - mainCamera.gameObject.transform.position), out RaycastHit hit, 1000, LayerMask.GetMask("Ground"))) // draw line from camera through cursor to ground
        {
            defaultTransform.LookAt(new Vector3(hit.point.x, defaultTransform.position.y, hit.point.z));
        }

        if (Time.time >= unSlowTime)
        {
            Config.movementMod = 1f;
        }
    }

    void FixedUpdate()
    {
        Move((wStrafe + sStrafe) * Time.fixedDeltaTime, (aStrafe + dStrafe) * Time.fixedDeltaTime, dash);

        controller.Move(playerVelocity * Time.deltaTime);
    }

    void Move(float wsMove, float adMove, bool dashing)
    {
        Vector3 direction = Vector3.Normalize(new Vector3(adMove, 0, wsMove));

        playerVelocity = direction * Config.movementSpeed * Config.movementMod;

        if (dashing)
        {
            dash = false;
            playerVelocity += direction * Config.dashSpeed * Config.movementMod;

            movementSource.clip = dashAudio;
            movementSource.Play();
        }

        playerVelocity.y -= 9.8f; // apply gravity
    }

    public void SlowPlayer(float speedMultiplier, float duration)
    {
        unSlowTime = Time.time + duration;
        Config.movementMod = speedMultiplier;

        movementSource.clip = slowAudio;
        movementSource.Play();
    }

    #region  input functions

    public void StrafeLeft(InputAction.CallbackContext con)
	{
        aStrafe = con.ReadValue<float>() * -1;
	}

	public void StrafeRight(InputAction.CallbackContext con)
	{
        dStrafe = con.ReadValue<float>();
	}

	public void StrafeUp(InputAction.CallbackContext con)
	{
		wStrafe = con.ReadValue<float>();
	}

	public void StrafeDown(InputAction.CallbackContext con)
	{
		sStrafe = con.ReadValue<float>() * -1;
	}

    public void Dash(InputAction.CallbackContext con)
	{
		if (con.performed && Time.time >= nextDashTime)
		{
            dash = true;
            nextDashTime = Time.time + Config.dashCooldown;
		}
	}

    #endregion input functions
}
