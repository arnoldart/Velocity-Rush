using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    // public WheelCollider[] wheel = new WheelCollider[2];
    // public float torque = 200f;
    // private void Start()
    // {
    //     
    // }
    //
    // private void FixedUpdate()
    // {
    //     if (Input.GetKey(KeyCode.W))
    //     {
    //         for (int i = 0; i < wheel.Length; i++)
    //         {
    //             wheel[i].motorTorque = torque;
    //         }
    //     }
    //     else
    //     {
    //         for (int i = 0; i < wheel.Length; i++)
    //         {
    //             wheel[i].motorTorque = 0;
    //         }
    //     }
    // }
    //
    
     public float acceleration = 20f;
    public float maxSteeringAngle = 45f;
    public float[] gearRatios = { 3.5f, 2.5f, 1.8f, 1.4f, 1.1f, 0.9f, 0.7f, 0.5f };
    public Text gearText;
    public Text speedometerText;
    public float maxSpeedMPH = 200f;

    public AudioClip engineSound;
    public AudioClip handbrakeSound;
    private AudioSource engineAudio;
    private AudioSource handbrakeAudio;

    private Vector2 movementInput;
    private Rigidbody kartRigidbody;
    private int currentGear = 0;
    private bool handbrakeActivated = false;

    public float handbrakeReduction = 0.9f; 

    void Start()
    {
        kartRigidbody = GetComponent<Rigidbody>();

        
        engineAudio = gameObject.AddComponent<AudioSource>();
        engineAudio.clip = engineSound;
        engineAudio.loop = true;
        engineAudio.Play();

        
        handbrakeAudio = gameObject.AddComponent<AudioSource>();
        handbrakeAudio.clip = handbrakeSound;
        handbrakeAudio.loop = true;
    }

    void Update()
    {
        MoveKart();
        UpdateUI();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }

    public void OnHandbrake(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            ActivateHandbrake();
        }
        else if (ctx.canceled)
        {
            DeactivateHandbrake();
        }
    }

    void MoveKart()
    {
        float horizontalInput = movementInput.x;
        float steering = Mathf.Clamp(horizontalInput, -1f, 1f) * maxSteeringAngle;
        float throttle = Mathf.Clamp(movementInput.y, 0f, 1f);
        float speed = kartRigidbody.velocity.magnitude * 2.237f;

        ManualShiftGear();
        float cappedSpeed = Mathf.Min(speed, maxSpeedMPH);

        float currentGearRatio = gearRatios[currentGear];
        float engineForce = throttle * currentGearRatio * acceleration;

        if (handbrakeActivated)
        {
            
            float reverseForce = -50f; 
            kartRigidbody.AddForce(transform.forward * reverseForce, ForceMode.Acceleration);
        }
        else
        {
            float cappedEngineForce = Mathf.Min(engineForce, maxSpeedMPH);
            kartRigidbody.AddForce(transform.forward * cappedEngineForce, ForceMode.Acceleration);
        }

        Quaternion deltaRotation = Quaternion.Euler(0, steering * Time.deltaTime, 0);
        kartRigidbody.MoveRotation(kartRigidbody.rotation * deltaRotation);

        
        if (speedometerText != null)
        {
            speedometerText.text = "Speed: " + Mathf.Round(cappedSpeed) + " mph";
        }

       
        engineAudio.pitch = 0.5f + 0.5f * (speed / maxSpeedMPH);
    }

    void UpdateUI()
    {
        if (gearText != null)
        {
            gearText.text = "Gear: " + (currentGear + 1);
        }
    }

    void ManualShiftGear()
    {
        if (Keyboard.current[Key.UpArrow].wasPressedThisFrame && currentGear < gearRatios.Length - 1)
        {
            currentGear++;
            // Reset waktu audio ke 0 saat menaikkan gigi
            engineAudio.time = 0f;
        }
        else if (Keyboard.current[Key.DownArrow].wasPressedThisFrame && currentGear > 0)
        {
            currentGear--;
            // Reset waktu audio ke 0 saat menurunkan gigi
            engineAudio.time = 0f;
        }
    }

    void ActivateHandbrake()
    {
        handbrakeActivated = true;
        // Menyalakan suara handbrake
        handbrakeAudio.Play();
    }

    void DeactivateHandbrake()
    {
        handbrakeActivated = false;
        // Mematikan suara handbrake
        handbrakeAudio.Stop();
    }
    
    // public float acceleration = 10f;
    // public float maxSteeringAngle = 30f;
    // public float[] gearRatios = { 3.5f, 2.5f, 1.8f, 1.4f, 1.1f, 0.9f, 0.7f, 0.5f };
    // public Text gearText;
    // public Text speedometerText;
    // public float maxSpeedMPH = 150f;
    // public float downshiftThreshold = 30f;
    //
    // private Vector2 movementInput;
    // private Rigidbody kartRigidbody;
    // private int currentGear = 0;
    //
    // void Start()
    // {
    //     kartRigidbody = GetComponent<Rigidbody>();
    // }
    //
    // void Update()
    // {
    //     MoveKart();
    //     UpdateUI();
    // }
    //
    // public void OnMove(InputAction.CallbackContext ctx)
    // {
    //     movementInput = ctx.ReadValue<Vector2>();
    // }
    //
    // void MoveKart()
    // {
    //     float horizontalInput = movementInput.x;
    //
    //     float steering = Mathf.Clamp(horizontalInput, -1f, 1f) * maxSteeringAngle;
    //
    //     float throttle = Mathf.Clamp(movementInput.y, 0f, 1f);
    //     float speed = kartRigidbody.velocity.magnitude * 2.237f;
    //
    //     ManualShiftGear();
    //
    //     float cappedSpeed = Mathf.Min(speed, maxSpeedMPH);
    //
    //     float currentGearRatio = gearRatios[currentGear];
    //
    //     float engineForce = throttle * currentGearRatio * acceleration;
    //
    //     float cappedEngineForce = Mathf.Min(engineForce, maxSpeedMPH);
    //
    //     kartRigidbody.AddForce(transform.forward * cappedEngineForce, ForceMode.Acceleration);
    //
    //     Quaternion deltaRotation = Quaternion.Euler(0, steering * Time.deltaTime, 0);
    //     kartRigidbody.MoveRotation(kartRigidbody.rotation * deltaRotation);
    //
    //     // Otomatis downshift hanya jika mobil bergerak lurus atau belokannya lemah
    //     if (speed < downshiftThreshold && currentGear > 0 && Mathf.Abs(horizontalInput) < 0.2f)
    //     {
    //         currentGear--;
    //     }
    //
    //     if (speed > downshiftThreshold * 1.5f && currentGear < gearRatios.Length - 1)
    //     {
    //         currentGear++;
    //     }
    //
    //     if (speed < 5f && currentGear > 0)
    //     {
    //         currentGear--;
    //     }
    //
    //     if (speed > 5f && currentGear < gearRatios.Length - 1)
    //     {
    //         currentGear++;
    //     }
    //
    //     if (speed < 1f)
    //     {
    //         currentGear = 0;
    //     }
    //
    //     if (speed > maxSpeedMPH)
    //     {
    //         currentGear = gearRatios.Length - 1;
    //     }
    //
    //     if (speed < 0.1f)
    //     {
    //         currentGear = 0;
    //     }
    //
    //     if (speed < 0.01f)
    //     {
    //         kartRigidbody.velocity = Vector3.zero;
    //         kartRigidbody.angularVelocity = Vector3.zero;
    //     }
    //
    //     if (kartRigidbody.velocity.magnitude < 0.1f)
    //     {
    //         kartRigidbody.velocity = Vector3.zero;
    //         kartRigidbody.angularVelocity = Vector3.zero;
    //     }
    //
    //     if (kartRigidbody.angularVelocity.magnitude < 0.1f)
    //     {
    //         kartRigidbody.velocity = Vector3.zero;
    //         kartRigidbody.angularVelocity = Vector3.zero;
    //     }
    //
    //     if (kartRigidbody.velocity.sqrMagnitude < 0.01f)
    //     {
    //         kartRigidbody.velocity = Vector3.zero;
    //         kartRigidbody.angularVelocity = Vector3.zero;
    //     }
    //
    //     if (kartRigidbody.angularVelocity.sqrMagnitude < 0.01f)
    //     {
    //         kartRigidbody.velocity = Vector3.zero;
    //         kartRigidbody.angularVelocity = Vector3.zero;
    //     }
    //
    //     if (kartRigidbody.angularVelocity.magnitude < 0.01f && kartRigidbody.velocity.magnitude < 0.01f)
    //     {
    //         kartRigidbody.velocity = Vector3.zero;
    //         kartRigidbody.angularVelocity = Vector3.zero;
    //     }
    // }
    //
    // void UpdateUI()
    // {
    //     if (gearText != null)
    //     {
    //         gearText.text = "Gear: " + (currentGear + 1);
    //     }
    // }
    //
    // void ManualShiftGear()
    // {
    //     if (Keyboard.current[Key.UpArrow].wasPressedThisFrame && currentGear < gearRatios.Length - 1)
    //     {
    //         currentGear++;
    //     }
    //     else if (Keyboard.current[Key.DownArrow].wasPressedThisFrame && currentGear > 0)
    //     {
    //         currentGear--;
    //     }
    // }


}
