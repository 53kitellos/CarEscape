using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerCarControl : MonoBehaviour
{
      [Space(20)]
      [Space(10)]
      [Range(20, 190)]
      public int MaxSpeed = 90; 
      [Range(10, 120)]
      public int MaxReverseSpeed = 45; 
      [Range(1, 10)]
      public int AccelerationMultiplier;
      [Space(10)]
      [Range(10, 45)]
      public int MaxSteeringAngle = 27; 
      [Range(0.1f, 1f)]
      public float SteeringSpeed = 0.5f;
      [Space(10)]
      [Range(100, 600)]
      public int BrakeForce = 350; 
      [Range(1, 10)]
      public int DecelerationMultiplier = 2; 
      [Range(1, 10)]
      public int HandbrakeDriftMultiplier = 5;
      [Space(10)]
      public Vector3 BodyMassCenter;

    //WHEELS

      public GameObject FrontLeftMesh;
      public WheelCollider FrontLeftCollider;
      [Space(10)]
      public GameObject FrontRightMesh;
      public WheelCollider FrontRightCollider;
      [Space(10)]
      public GameObject RearLeftMesh;
      public WheelCollider RearLeftCollider;
      [Space(10)]
      public GameObject RearRightMesh;
      public WheelCollider RearRightCollider;

    //PARTICLE SYSTEMS

      [Space(20)]
      [Space(10)]
      public bool UseEffects = false;
      public ParticleSystem RLWParticleSystem;
      public ParticleSystem RRWParticleSystem;
      [Space(10)]
      public TrailRenderer RLWTireSkid;
      public TrailRenderer RRWTireSkid;

    //SPEED TEXT (UI)

      [Space(20)]
      [Space(10)]
      public bool UseUI = false;
      public Text CarSpeedText; 
      public NitroBar NitroBar;

    //SOUNDS

      [Space(20)]
      [Space(10)]
      public bool UseSounds = false;
      public AudioSource CarEngineSound; 
      public AudioSource TireScreechSound;
      public AudioSource NitroSound;
      public AudioSource NitroCollectedSound;
      private float _initialCarEngineSoundPitch; 

    //CAR DATA

      [HideInInspector]
      public float CarSpeed; 
      [HideInInspector]
      public bool IsDrifting; 
      [HideInInspector]
      public bool IsTractionLocked; 

    //PRIVATE VARIABLES

      private Rigidbody _carRigidbody;
      private int _maxNitroValue = 100;
      private int _nitroAcceleration = 150;
      private float _spendNitroValue = -20;
      private float _refillNirtoValue = 4;
      private float _steeringAxis;
      private float _throttleAxis;
      private float _driftingAxis;
      private float _localVelocityZ;
      private float _localVelocityX;
      private bool _deceleratingCar;
      WheelFrictionCurve _wheelFrictionFL;
      float _extremumSlipFLW;
      WheelFrictionCurve _wheelFrictionFR;
      float _extremumSlipFRW;
      WheelFrictionCurve _wheelFrictionRL;
      float _extremumSlipRLW;
      WheelFrictionCurve _wheelFrictionRR;
      float _extremumSlipRRW;

      public event UnityAction<float, float> NitroValueChanged;

    private void Start()
    {
      _carRigidbody = gameObject.GetComponent<Rigidbody>();
      _carRigidbody.centerOfMass = BodyMassCenter;

      _wheelFrictionFL = new WheelFrictionCurve ();
        _wheelFrictionFL.extremumSlip = FrontLeftCollider.sidewaysFriction.extremumSlip;
        _extremumSlipFLW = FrontLeftCollider.sidewaysFriction.extremumSlip;
        _wheelFrictionFL.extremumValue = FrontLeftCollider.sidewaysFriction.extremumValue;
        _wheelFrictionFL.asymptoteSlip = FrontLeftCollider.sidewaysFriction.asymptoteSlip;
        _wheelFrictionFL.asymptoteValue = FrontLeftCollider.sidewaysFriction.asymptoteValue;
        _wheelFrictionFL.stiffness = FrontLeftCollider.sidewaysFriction.stiffness;
      _wheelFrictionFR = new WheelFrictionCurve ();
        _wheelFrictionFR.extremumSlip = FrontRightCollider.sidewaysFriction.extremumSlip;
        _extremumSlipFRW = FrontRightCollider.sidewaysFriction.extremumSlip;
        _wheelFrictionFR.extremumValue = FrontRightCollider.sidewaysFriction.extremumValue;
        _wheelFrictionFR.asymptoteSlip = FrontRightCollider.sidewaysFriction.asymptoteSlip;
        _wheelFrictionFR.asymptoteValue = FrontRightCollider.sidewaysFriction.asymptoteValue;
        _wheelFrictionFR.stiffness = FrontRightCollider.sidewaysFriction.stiffness;
      _wheelFrictionRL = new WheelFrictionCurve ();
        _wheelFrictionRL.extremumSlip = RearLeftCollider.sidewaysFriction.extremumSlip;
        _extremumSlipRLW = RearLeftCollider.sidewaysFriction.extremumSlip;
        _wheelFrictionRL.extremumValue = RearLeftCollider.sidewaysFriction.extremumValue;
        _wheelFrictionRL.asymptoteSlip = RearLeftCollider.sidewaysFriction.asymptoteSlip;
        _wheelFrictionRL.asymptoteValue = RearLeftCollider.sidewaysFriction.asymptoteValue;
        _wheelFrictionRL.stiffness = RearLeftCollider.sidewaysFriction.stiffness;
      _wheelFrictionRR = new WheelFrictionCurve ();
        _wheelFrictionRR.extremumSlip = RearRightCollider.sidewaysFriction.extremumSlip;
        _extremumSlipRRW = RearRightCollider.sidewaysFriction.extremumSlip;
        _wheelFrictionRR.extremumValue = RearRightCollider.sidewaysFriction.extremumValue;
        _wheelFrictionRR.asymptoteSlip = RearRightCollider.sidewaysFriction.asymptoteSlip;
        _wheelFrictionRR.asymptoteValue = RearRightCollider.sidewaysFriction.asymptoteValue;
        _wheelFrictionRR.stiffness = RearRightCollider.sidewaysFriction.stiffness;

        if(CarEngineSound != null)
        {
          _initialCarEngineSoundPitch = CarEngineSound.pitch;
        }

        if(UseUI)
        {
          InvokeRepeating("CarSpeedUI", 0f, 0.1f);
        }
        else if(!UseUI)
        {
          if(CarSpeedText != null)
          {
            CarSpeedText.text = "0";
          }
        }

        if(UseSounds)
        {
          InvokeRepeating("CarSounds", 0f, 0.1f);
        }
        else if(!UseSounds)
        {
          if(CarEngineSound != null)
          {
            CarEngineSound.Stop();
          }

          if(TireScreechSound != null)
          {
            TireScreechSound.Stop();
          }
        }

        if(!UseEffects)
        {
          if(RLWParticleSystem != null)
          {
            RLWParticleSystem.Stop();
          }

          if(RRWParticleSystem != null)
          {
            RRWParticleSystem.Stop();
          }

          if(RLWTireSkid != null)
          {
            RLWTireSkid.emitting = false;
          }

          if(RRWTireSkid != null){
            RRWTireSkid.emitting = false;
          }
        }
    }

    private void Update()
    {
      CarSpeed = (2 * Mathf.PI * FrontLeftCollider.radius * FrontLeftCollider.rpm * 60) / 1000;
      _localVelocityX = transform.InverseTransformDirection(_carRigidbody.velocity).x;
      _localVelocityZ = transform.InverseTransformDirection(_carRigidbody.velocity).z;
        
        if (Input.GetKey(KeyCode.W))
        {
            CancelInvoke("DecelerateCar");
            _deceleratingCar = false;
            GoForward(AccelerationMultiplier);
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            if (NitroBar.TryGetNitro())
            {
                CancelInvoke("DecelerateCar");
                _deceleratingCar = false;
                GoForward(_nitroAcceleration);
                NitroValueChanged?.Invoke(_spendNitroValue * Time.deltaTime, _maxNitroValue);
            }
            else 
            {
                CancelInvoke("DecelerateCar");
                _deceleratingCar = false;
                GoForward(AccelerationMultiplier);
            }
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (NitroBar.TryGetNitro())
            {
                NitroSound.Play();
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            CancelInvoke("DecelerateCar");
            _deceleratingCar = false;
            GoReverse();
        }

        if (Input.GetKey(KeyCode.A))
        {
            TurnLeft();
        }

        if (Input.GetKey(KeyCode.D))
        {
            TurnRight();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            CancelInvoke("DecelerateCar");
            _deceleratingCar = false;
            Handbrake();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            RecoverTraction();
        }

        if ((!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)))
        {
            ThrottleOff();
        }

        if ((!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)) && !Input.GetKey(KeyCode.Space) && !_deceleratingCar)
        {
            InvokeRepeating("DecelerateCar", 0f, 0.1f);
            _deceleratingCar = true;
        }

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && _steeringAxis != 0f)
        {
            ResetSteeringAngle();
        }

        AnimateWheelMeshes();
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<NitroSupply>(out NitroSupply nitroSupply))
        {
            NitroCollectedSound.Play();
            NitroValueChanged?.Invoke(_refillNirtoValue, _maxNitroValue);
            nitroSupply.SelfDetroy();
        }
    }

    public void CarSpeedUI(){

      if(UseUI){
          try{
            float absoluteCarSpeed = Mathf.Abs(CarSpeed);
            CarSpeedText.text = Mathf.RoundToInt(absoluteCarSpeed).ToString();
          }catch(Exception ex){
            Debug.LogWarning(ex);
          }
      }

    }

    public void CarSounds(){

      if(UseSounds)
      {
        try
        {
          if(CarEngineSound != null)
          {
            float engineSoundPitch = _initialCarEngineSoundPitch + (Mathf.Abs(_carRigidbody.velocity.magnitude) / 25f);
            CarEngineSound.pitch = engineSoundPitch;
          }

          if((IsDrifting) || (IsTractionLocked && Mathf.Abs(CarSpeed) > 12f))
          {
            if(!TireScreechSound.isPlaying)
            {
              TireScreechSound.Play();
            }
          }
          else if((!IsDrifting) && (!IsTractionLocked || Mathf.Abs(CarSpeed) < 12f))
          {
            TireScreechSound.Stop();
          }
        }
        catch(Exception ex)
        {
          Debug.LogWarning(ex);
        }
      }
      else if(!UseSounds)
      {
        if(CarEngineSound != null && CarEngineSound.isPlaying)
        {
          CarEngineSound.Stop();
        }

        if(TireScreechSound != null && TireScreechSound.isPlaying)
        {
          TireScreechSound.Stop();
        }
      }
    }

    public void TurnLeft(){
      _steeringAxis = _steeringAxis - (Time.deltaTime * 10f * SteeringSpeed);

      if(_steeringAxis < -1f)
      {
        _steeringAxis = -1f;
      }

      var steeringAngle = _steeringAxis * MaxSteeringAngle;
      FrontLeftCollider.steerAngle = Mathf.Lerp(FrontLeftCollider.steerAngle, steeringAngle, SteeringSpeed);
      FrontRightCollider.steerAngle = Mathf.Lerp(FrontRightCollider.steerAngle, steeringAngle, SteeringSpeed);
    }

    public void TurnRight()
    {
      _steeringAxis = _steeringAxis + (Time.deltaTime * 10f * SteeringSpeed);

      if(_steeringAxis > 1f)
      {
        _steeringAxis = 1f;
      }

      var steeringAngle = _steeringAxis * MaxSteeringAngle;
      FrontLeftCollider.steerAngle = Mathf.Lerp(FrontLeftCollider.steerAngle, steeringAngle, SteeringSpeed);
      FrontRightCollider.steerAngle = Mathf.Lerp(FrontRightCollider.steerAngle, steeringAngle, SteeringSpeed);
    }

    public void ResetSteeringAngle()
    {
      if(_steeringAxis < 0f)
      {
        _steeringAxis = _steeringAxis + (Time.deltaTime * 10f * SteeringSpeed);
      }
      else if(_steeringAxis > 0f)
      {
        _steeringAxis = _steeringAxis - (Time.deltaTime * 10f * SteeringSpeed);
      }

      if(Mathf.Abs(FrontLeftCollider.steerAngle) < 1f)
      {
        _steeringAxis = 0f;
      }

      var steeringAngle = _steeringAxis * MaxSteeringAngle;
      FrontLeftCollider.steerAngle = Mathf.Lerp(FrontLeftCollider.steerAngle, steeringAngle, SteeringSpeed);
      FrontRightCollider.steerAngle = Mathf.Lerp(FrontRightCollider.steerAngle, steeringAngle, SteeringSpeed);
    }

    void AnimateWheelMeshes(){
      try
      {
        Quaternion FLWRotation;
        Vector3 FLWPosition;
        FrontLeftCollider.GetWorldPose(out FLWPosition, out FLWRotation);
        FrontLeftMesh.transform.position = FLWPosition;
        FrontLeftMesh.transform.rotation = FLWRotation;

        Quaternion FRWRotation;
        Vector3 FRWPosition;
        FrontRightCollider.GetWorldPose(out FRWPosition, out FRWRotation);
        FrontRightMesh.transform.position = FRWPosition;
        FrontRightMesh.transform.rotation = FRWRotation;

        Quaternion RLWRotation;
        Vector3 RLWPosition;
        RearLeftCollider.GetWorldPose(out RLWPosition, out RLWRotation);
        RearLeftMesh.transform.position = RLWPosition;
        RearLeftMesh.transform.rotation = RLWRotation;

        Quaternion RRWRotation;
        Vector3 RRWPosition;
        RearRightCollider.GetWorldPose(out RRWPosition, out RRWRotation);
        RearRightMesh.transform.position = RRWPosition;
        RearRightMesh.transform.rotation = RRWRotation;
      }
      catch(Exception ex)
      {
        Debug.LogWarning(ex);
      }
    }

    public void GoForward(int accelerationValue)
    {
      if(Mathf.Abs(_localVelocityX) > 2.5f)
      {
        IsDrifting = true;
        DriftCarPS();
      }
      else
      {
        IsDrifting = false;
        DriftCarPS();
      }

      _throttleAxis = _throttleAxis + (Time.deltaTime * 3f);

      if(_throttleAxis > 1f)
      {
        _throttleAxis = 1f;
      }

      if(_localVelocityZ < -1f)
      {
        Brakes();
      }
      else
      {
        if(Mathf.RoundToInt(CarSpeed) < MaxSpeed)
        {
          FrontLeftCollider.brakeTorque = 0;
          FrontLeftCollider.motorTorque = (accelerationValue * 50f) * _throttleAxis;
          FrontRightCollider.brakeTorque = 0;
          FrontRightCollider.motorTorque = (accelerationValue * 50f) * _throttleAxis;
          RearLeftCollider.brakeTorque = 0;
          RearLeftCollider.motorTorque = (accelerationValue * 50f) * _throttleAxis;
          RearRightCollider.brakeTorque = 0;
          RearRightCollider.motorTorque = (accelerationValue * 50f) * _throttleAxis;
        }
        else
        {
    	FrontLeftCollider.motorTorque = 0;
    	FrontRightCollider.motorTorque = 0;
        RearLeftCollider.motorTorque = 0;
    	RearRightCollider.motorTorque = 0;
    	}
      }
    }

    public void GoReverse(){
      if(Mathf.Abs(_localVelocityX) > 2.5f)
      {
        IsDrifting = true;
        DriftCarPS();
      }
      else
      {
        IsDrifting = false;
        DriftCarPS();
      }

      _throttleAxis = _throttleAxis - (Time.deltaTime * 3f);

      if(_throttleAxis < -1f)
      {
        _throttleAxis = -1f;
      }

      if(_localVelocityZ > 1f)
      {
        Brakes();
      }
      else
      {
        if(Mathf.Abs(Mathf.RoundToInt(CarSpeed)) < MaxReverseSpeed)
        {
          FrontLeftCollider.brakeTorque = 0;
          FrontLeftCollider.motorTorque = (AccelerationMultiplier * 50f) * _throttleAxis;
          FrontRightCollider.brakeTorque = 0;
          FrontRightCollider.motorTorque = (AccelerationMultiplier * 50f) * _throttleAxis;
          RearLeftCollider.brakeTorque = 0;
          RearLeftCollider.motorTorque = (AccelerationMultiplier * 50f) * _throttleAxis;
          RearRightCollider.brakeTorque = 0;
          RearRightCollider.motorTorque = (AccelerationMultiplier * 50f) * _throttleAxis;
        }
        else 
        {
    	 FrontLeftCollider.motorTorque = 0;
    	 FrontRightCollider.motorTorque = 0;
         RearLeftCollider.motorTorque = 0;
    	 RearRightCollider.motorTorque = 0;
    	}
      }
    }

    public void ThrottleOff(){
      FrontLeftCollider.motorTorque = 0;
      FrontRightCollider.motorTorque = 0;
      RearLeftCollider.motorTorque = 0;
      RearRightCollider.motorTorque = 0;
    }

    public void DecelerateCar(){
      if(Mathf.Abs(_localVelocityX) > 2.5f)
      {
        IsDrifting = true;
        DriftCarPS();
      }
      else
      {
        IsDrifting = false;
        DriftCarPS();
      }

      if(_throttleAxis != 0f)
      {
        if(_throttleAxis > 0f)
        {
          _throttleAxis = _throttleAxis - (Time.deltaTime * 10f);
        }
        else if(_throttleAxis < 0f)
        {
            _throttleAxis = _throttleAxis + (Time.deltaTime * 10f);
        }

        if(Mathf.Abs(_throttleAxis) < 0.15f)
        {
          _throttleAxis = 0f;
        }
      }

      _carRigidbody.velocity = _carRigidbody.velocity * (1f / (1f + (0.025f * DecelerationMultiplier)));
      FrontLeftCollider.motorTorque = 0;
      FrontRightCollider.motorTorque = 0;
      RearLeftCollider.motorTorque = 0;
      RearRightCollider.motorTorque = 0;

      if(_carRigidbody.velocity.magnitude < 0.25f)
      {
        _carRigidbody.velocity = Vector3.zero;
        CancelInvoke("DecelerateCar");
      }
    }

    public void Brakes(){
      FrontLeftCollider.brakeTorque = BrakeForce;
      FrontRightCollider.brakeTorque = BrakeForce;
      RearLeftCollider.brakeTorque = BrakeForce;
      RearRightCollider.brakeTorque = BrakeForce;
    }

    public void Handbrake()
    {
      CancelInvoke("RecoverTraction");
      _driftingAxis = _driftingAxis + (Time.deltaTime);
      float secureStartingPoint = _driftingAxis * _extremumSlipFLW * HandbrakeDriftMultiplier;

      if(secureStartingPoint < _extremumSlipFLW)
      {
        _driftingAxis = _extremumSlipFLW / (_extremumSlipFLW * HandbrakeDriftMultiplier);
      }

      if(_driftingAxis > 1f)
      {
        _driftingAxis = 1f;
      }

      if(Mathf.Abs(_localVelocityX) > 2.5f)
      {
        IsDrifting = true;
      }
      else
      {
        IsDrifting = false;
      }

      if(_driftingAxis < 1f)
      {
        _wheelFrictionFL.extremumSlip = _extremumSlipFLW * HandbrakeDriftMultiplier * _driftingAxis;
        FrontLeftCollider.sidewaysFriction = _wheelFrictionFL;

        _wheelFrictionFR.extremumSlip = _extremumSlipFRW * HandbrakeDriftMultiplier * _driftingAxis;
        FrontRightCollider.sidewaysFriction = _wheelFrictionFR;

        _wheelFrictionRL.extremumSlip = _extremumSlipRLW * HandbrakeDriftMultiplier * _driftingAxis;
        RearLeftCollider.sidewaysFriction = _wheelFrictionRL;

        _wheelFrictionRR.extremumSlip = _extremumSlipRRW * HandbrakeDriftMultiplier * _driftingAxis;
        RearRightCollider.sidewaysFriction = _wheelFrictionRR;
      }

      IsTractionLocked = true;
      DriftCarPS();
    }

    public void DriftCarPS(){

      if(UseEffects)
      {
        try
        {
          if(IsDrifting)
          {
            RLWParticleSystem.Play();
            RRWParticleSystem.Play();
          }
          else if(!IsDrifting)
          {
            RLWParticleSystem.Stop();
            RRWParticleSystem.Stop();
          }
        }
        catch(Exception ex)
        {
          Debug.LogWarning(ex);
        }

        try
        {
          if((IsTractionLocked || Mathf.Abs(_localVelocityX) > 5f) && Mathf.Abs(CarSpeed) > 12f)
          {
            RLWTireSkid.emitting = true;
            RRWTireSkid.emitting = true;
          }
          else 
          {
            RLWTireSkid.emitting = false;
            RRWTireSkid.emitting = false;
          }
        }
        catch(Exception ex)
        {
          Debug.LogWarning(ex);
        }
      }
      else if(!UseEffects)
      {
        if(RLWParticleSystem != null)
        {
          RLWParticleSystem.Stop();
        }

        if(RRWParticleSystem != null)
        {
          RRWParticleSystem.Stop();
        }

        if(RLWTireSkid != null)
        {
          RLWTireSkid.emitting = false;
        }

        if(RRWTireSkid != null) 
        {
          RRWTireSkid.emitting = false;
        }
      }

    }

    public void RecoverTraction(){
      IsTractionLocked = false;
      _driftingAxis = _driftingAxis - (Time.deltaTime / 1.5f);

      if(_driftingAxis < 0f)
      {
        _driftingAxis = 0f;
      }

      if(_wheelFrictionFL.extremumSlip > _extremumSlipFLW)
      {
        _wheelFrictionFL.extremumSlip = _extremumSlipFLW * HandbrakeDriftMultiplier * _driftingAxis;
        FrontLeftCollider.sidewaysFriction = _wheelFrictionFL;

        _wheelFrictionFR.extremumSlip = _extremumSlipFRW * HandbrakeDriftMultiplier * _driftingAxis;
        FrontRightCollider.sidewaysFriction = _wheelFrictionFR;

        _wheelFrictionRL.extremumSlip = _extremumSlipRLW * HandbrakeDriftMultiplier * _driftingAxis;
        RearLeftCollider.sidewaysFriction = _wheelFrictionRL;

        _wheelFrictionRR.extremumSlip = _extremumSlipRRW * HandbrakeDriftMultiplier * _driftingAxis;
        RearRightCollider.sidewaysFriction = _wheelFrictionRR;

        Invoke("RecoverTraction", Time.deltaTime);

      }
      else if (_wheelFrictionFL.extremumSlip < _extremumSlipFLW)
      {
        _wheelFrictionFL.extremumSlip = _extremumSlipFLW;
        FrontLeftCollider.sidewaysFriction = _wheelFrictionFL;

        _wheelFrictionFR.extremumSlip = _extremumSlipFRW;
        FrontRightCollider.sidewaysFriction = _wheelFrictionFR;

        _wheelFrictionRL.extremumSlip = _extremumSlipRLW;
        RearLeftCollider.sidewaysFriction = _wheelFrictionRL;

        _wheelFrictionRR.extremumSlip = _extremumSlipRRW;
        RearRightCollider.sidewaysFriction = _wheelFrictionRR;

        _driftingAxis = 0f;
      }
    }
}