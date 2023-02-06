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

    [Header("GameObjects")]
    [Header("Audio")]

    #endregion public properties

    #region private properties

    private float nextDashTime;
    private bool dash = false;
    private float wStrafe = 0f;
    private float sStrafe = 0f;
    private float dStrafe = 0f;
    private float aStrafe = 0f;
    
    AudioSource movementSource;

    private Vector3 playerVelocity;

    #endregion private properties

    void Awake()
    {
        movementSource = defaultTransform.gameObject.GetComponent<AudioSource>();
    }

    void Start()
    {
        movementSource.playOnAwake = false;
        movementSource.spatialBlend = 1f;
        movementSource.volume = 0.7f;
        movementSource.priority = 128;

        // set up dash ui
    }

    void Update()
    {
        // update dash ui
    }

    void FixedUpdate()
    {
        Move((wStrafe + sStrafe) * Time.fixedDeltaTime, (aStrafe + dStrafe) * Time.fixedDeltaTime, dash);

        controller.Move(playerVelocity * Time.deltaTime);
    }

    void Move(float wsMove, float adMove, bool dashing)
    {
        Vector3 direction = Vector3.Normalize(new Vector3(adMove, 0, wsMove));

        playerVelocity = direction * Config.movementSpeed;

        if (dashing)
        {
            dash = false;
            playerVelocity += direction * Config.dashSpeed;
        }
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
