using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerCarControl : MonoBehaviour
{
    //WHEELS
    [SerializeField] private GameObject _frontLeftMesh;
    [SerializeField] private WheelCollider _frontLeftCollider;
    [SerializeField] private GameObject _frontRightMesh;
    [SerializeField] private WheelCollider _frontRightCollider;
    [SerializeField] private GameObject _rearLeftMesh;
    [SerializeField] private WheelCollider _rearLeftCollider;
    [SerializeField] private GameObject _rearRightMesh;
    [SerializeField] private WheelCollider _rearRightCollider;

    //PARTICLE SYSTEMS
    [SerializeField] private ParticleSystem _smokeSystemRLW;
    [SerializeField] private ParticleSystem _smokeSystemRRW;
    [SerializeField] private TrailRenderer _tireSkidRLW;
    [SerializeField] private TrailRenderer _tireSkidRRW;
     private bool _useEffects = true;

    //SPEED TEXT (UI)
    [SerializeField] private Text _carSpeedText;
    [SerializeField] private NitroBar _nitroBar;
     private bool _useUI = true;

    //SOUNDS
    [SerializeField] private AudioSource _carEngineSound;
    [SerializeField] private AudioSource _tireScreechSound;
    [SerializeField] private AudioSource _nitroSound;
    [SerializeField] private AudioSource _nitroCollectedSound;
     private bool _useSounds = true;
     private float _initialCarEngineSoundPitch;

    [SerializeField] private GameObject _throttleButton;
    [SerializeField] private GameObject _reverseButton;
    [SerializeField] private GameObject _turnRightButton;
    [SerializeField] private GameObject _turnLeftButton;
    [SerializeField] private GameObject _handbrakeButton;
    [SerializeField] private GameObject _nitroButton;
    private TouchInput _throttlePTI;
    private TouchInput _reversePTI;
    private TouchInput _turnRightPTI;
    private TouchInput _turnLeftPTI;
    private TouchInput _handbrakePTI;
    private TouchInput _nitroPTI;

    //CAR STATS
    private int _maxSpeed = 140;
    private int _maxAccelerationSpeed = 230;
    private int _maxReverseSpeed = 50;
    private int _accelerationMultiplier = 12;
    private int _maxSteeringAngle = 35;
    private int _brakeForce = 450;
    private int _decelerationMultiplier = 2;
    private int _handbrakeDriftMultiplier = 10;
    private int _maxNitroValue = 100;
    private int _nitroAcceleration = 150;
    private float _hardnessWASD = 10f;
    private float _hardnessTouchInput = 5f;
    private float _carSpeed;
    private float _steeringSpeed = 0.7f;
    private float _spendNitroValue = -25;
    private float _refillNirtoValue = 2;
    private float _steeringAxis;
    private float _throttleAxis;
    private float _driftingAxis;
    private float _localVelocityZ;
    private float _localVelocityX;
    private bool _isDrifting;
    private bool _isTractionLocked;
    private bool _deceleratingCar;
    private Vector3 _bodyMassCenter;
    private Rigidbody _carRigidbody;

    private WheelFrictionCurve _wheelFrictionFL;
    private float _extremumSlipFLW;
    private WheelFrictionCurve _wheelFrictionFR;
    private float _extremumSlipFRW;
    private WheelFrictionCurve _wheelFrictionRL;
    private float _extremumSlipRLW;
    private WheelFrictionCurve _wheelFrictionRR;
    private float _extremumSlipRRW;

    public event UnityAction<float, float> NitroValueChanged;

    private void Start()
    {
      _carRigidbody = gameObject.GetComponent<Rigidbody>();
      _carRigidbody.centerOfMass = _bodyMassCenter;

      _wheelFrictionFL = new WheelFrictionCurve ();
        _wheelFrictionFL.extremumSlip = _frontLeftCollider.sidewaysFriction.extremumSlip;
        _extremumSlipFLW = _frontLeftCollider.sidewaysFriction.extremumSlip;
        _wheelFrictionFL.extremumValue = _frontLeftCollider.sidewaysFriction.extremumValue;
        _wheelFrictionFL.asymptoteSlip = _frontLeftCollider.sidewaysFriction.asymptoteSlip;
        _wheelFrictionFL.asymptoteValue = _frontLeftCollider.sidewaysFriction.asymptoteValue;
        _wheelFrictionFL.stiffness = _frontLeftCollider.sidewaysFriction.stiffness;
      _wheelFrictionFR = new WheelFrictionCurve ();
        _wheelFrictionFR.extremumSlip = _frontRightCollider.sidewaysFriction.extremumSlip;
        _extremumSlipFRW = _frontRightCollider.sidewaysFriction.extremumSlip;
        _wheelFrictionFR.extremumValue = _frontRightCollider.sidewaysFriction.extremumValue;
        _wheelFrictionFR.asymptoteSlip = _frontRightCollider.sidewaysFriction.asymptoteSlip;
        _wheelFrictionFR.asymptoteValue = _frontRightCollider.sidewaysFriction.asymptoteValue;
        _wheelFrictionFR.stiffness = _frontRightCollider.sidewaysFriction.stiffness;
      _wheelFrictionRL = new WheelFrictionCurve ();
        _wheelFrictionRL.extremumSlip = _rearLeftCollider.sidewaysFriction.extremumSlip;
        _extremumSlipRLW = _rearLeftCollider.sidewaysFriction.extremumSlip;
        _wheelFrictionRL.extremumValue = _rearLeftCollider.sidewaysFriction.extremumValue;
        _wheelFrictionRL.asymptoteSlip = _rearLeftCollider.sidewaysFriction.asymptoteSlip;
        _wheelFrictionRL.asymptoteValue = _rearLeftCollider.sidewaysFriction.asymptoteValue;
        _wheelFrictionRL.stiffness = _rearLeftCollider.sidewaysFriction.stiffness;
      _wheelFrictionRR = new WheelFrictionCurve ();
        _wheelFrictionRR.extremumSlip = _rearRightCollider.sidewaysFriction.extremumSlip;
        _extremumSlipRRW = _rearRightCollider.sidewaysFriction.extremumSlip;
        _wheelFrictionRR.extremumValue = _rearRightCollider.sidewaysFriction.extremumValue;
        _wheelFrictionRR.asymptoteSlip = _rearRightCollider.sidewaysFriction.asymptoteSlip;
        _wheelFrictionRR.asymptoteValue = _rearRightCollider.sidewaysFriction.asymptoteValue;
        _wheelFrictionRR.stiffness = _rearRightCollider.sidewaysFriction.stiffness;

        if(_carEngineSound != null)
        {
          _initialCarEngineSoundPitch = _carEngineSound.pitch;
        }

        if(_useUI)
        {
          InvokeRepeating("CarSpeedUI", 0f, 0.1f);
        }
        else if(!_useUI)
        {
          if(_carSpeedText != null)
          {
            _carSpeedText.text = "0";
          }
        }

        if(_useSounds)
        {
          InvokeRepeating("CarSounds", 0f, 0.1f);
        }
        else if(!_useSounds)
        {
          if(_carEngineSound != null)
          {
            _carEngineSound.Stop();
          }

          if(_tireScreechSound != null)
          {
            _tireScreechSound.Stop();
          }
        }

        if(!_useEffects)
        {
          if(_smokeSystemRLW != null)
          {
            _smokeSystemRLW.Stop();
          }

          if(_smokeSystemRRW != null)
          {
            _smokeSystemRRW.Stop();
          }

          if(_tireSkidRLW != null)
          {
            _tireSkidRLW.emitting = false;
          }

          if(_tireSkidRRW != null)
          {
            _tireSkidRRW.emitting = false;
          }
        }

        if (PlayerPrefs.GetInt("touchpadOn")==1)
        {
            if (_throttleButton != null && _reverseButton != null &&
            _turnRightButton != null && _turnLeftButton != null
            && _handbrakeButton != null && _nitroButton != null)
            {
                _throttlePTI = _throttleButton.GetComponent<TouchInput>();
                _reversePTI = _reverseButton.GetComponent<TouchInput>();
                _turnLeftPTI = _turnLeftButton.GetComponent<TouchInput>();
                _turnRightPTI = _turnRightButton.GetComponent<TouchInput>();
                _handbrakePTI = _handbrakeButton.GetComponent<TouchInput>();
                _nitroPTI = _nitroButton.GetComponent<TouchInput>();
            }
            else
            {
                String ex = "Touch controls are not completely set up. You must drag and drop your scene buttons in the" +
                " PrometeoCarController component.";
                Debug.LogWarning(ex);
            }
        }
    }

    private void Update()
    {
      _carSpeed = (2 * Mathf.PI * _frontLeftCollider.radius * _frontLeftCollider.rpm * 60) / 1000;
      _localVelocityX = transform.InverseTransformDirection(_carRigidbody.velocity).x;
      _localVelocityZ = transform.InverseTransformDirection(_carRigidbody.velocity).z;

        if (PlayerPrefs.GetInt("touchpadOn") == 1)
        {
            if (_nitroPTI.buttonPressed && _throttlePTI.buttonPressed)
            {
                if (_nitroBar.TryGetNitro())
                {
                    _nitroSound.Play();
                    CancelInvoke("DecelerateCar");
                    _deceleratingCar = false;
                    GoForward(_nitroAcceleration, _maxAccelerationSpeed);
                    NitroValueChanged?.Invoke(_spendNitroValue * Time.deltaTime, _maxNitroValue);
                }
                else
                {
                    CancelInvoke("DecelerateCar");
                    _deceleratingCar = false;
                    GoForward(_accelerationMultiplier, _maxSpeed);
                }
            }
            else if (_throttlePTI.buttonPressed)
            {
                CancelInvoke("DecelerateCar");
                _deceleratingCar = false;
                GoForward(_accelerationMultiplier, _maxSpeed);
            }
            

            if (_reversePTI.buttonPressed)
            {
                CancelInvoke("DecelerateCar");
                _deceleratingCar = false;
                GoReverse();
            }

            if (_turnLeftPTI.buttonPressed)
            {
                TurnLeft(_hardnessTouchInput);
            }

            if (_turnRightPTI.buttonPressed)
            {
                TurnRight(_hardnessTouchInput);
            }

            if (_handbrakePTI.buttonPressed)
            {
                CancelInvoke("DecelerateCar");
                _deceleratingCar = false;
                Handbrake();
            }

            if (!_handbrakePTI.buttonPressed)
            {
                RecoverTraction();
            }

            if ((!_throttlePTI.buttonPressed && !_reversePTI.buttonPressed))
            {
                ThrottleOff();
            }

            if ((!_reversePTI.buttonPressed && !_throttlePTI.buttonPressed) && !_handbrakePTI.buttonPressed && !_deceleratingCar)
            {
                InvokeRepeating("DecelerateCar", 0f, 0.1f);
                _deceleratingCar = true;
            }

            if (!_turnLeftPTI.buttonPressed && !_turnRightPTI.buttonPressed && _steeringAxis != 0f)
            {
                ResetSteeringAngle();
            }

        }
        else
        {


            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
            {
                if (_nitroBar.TryGetNitro())
                {
                    CancelInvoke("DecelerateCar");
                    _deceleratingCar = false;
                    GoForward(_nitroAcceleration, _maxAccelerationSpeed);
                    NitroValueChanged?.Invoke(_spendNitroValue * Time.deltaTime, _maxNitroValue);
                }
            }
            else if (Input.GetKey(KeyCode.W))
            {
                CancelInvoke("DecelerateCar");
                _deceleratingCar = false;
                GoForward(_accelerationMultiplier, _maxSpeed);
            }


            if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (_nitroBar.TryGetNitro())
                {
                    _nitroSound.Play();
                    _smokeSystemRLW.Play();
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
                TurnLeft(_hardnessWASD);
            }

            if (Input.GetKey(KeyCode.D))
            {
                TurnRight(_hardnessWASD);
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

        }

        AnimateWheelMeshes();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<NitroSupply>(out NitroSupply nitroSupply))
        {
            _nitroCollectedSound.Play();
            NitroValueChanged?.Invoke(_refillNirtoValue, _maxNitroValue);
            nitroSupply.SelfDetroy();
        }
    }

    private void CarSpeedUI()
    {
      if(_useUI){
          try{
            float absoluteCarSpeed = Mathf.Abs(_carSpeed);
            _carSpeedText.text = Mathf.RoundToInt(absoluteCarSpeed).ToString();
          }catch(Exception ex){
            Debug.LogWarning(ex);
          }
      }

    }

    private void CarSounds()
    {
      if(_useSounds)
      {
        try
        {
          if(_carEngineSound != null)
          {
            float engineSoundPitch = _initialCarEngineSoundPitch + (Mathf.Abs(_carRigidbody.velocity.magnitude) / 25f);
            _carEngineSound.pitch = engineSoundPitch;
          }

          if((_isDrifting) || (_isTractionLocked && Mathf.Abs(_carSpeed) > 12f))
          {
            if(!_tireScreechSound.isPlaying)
            {
              _tireScreechSound.Play();
            }
          }
          else if((!_isDrifting) && (!_isTractionLocked || Mathf.Abs(_carSpeed) < 12f))
          {
            _tireScreechSound.Stop();
          }
        }
        catch(Exception ex)
        {
          Debug.LogWarning(ex);
        }
      }
      else if(!_useSounds)
      {
        if(_carEngineSound != null && _carEngineSound.isPlaying)
        {
          _carEngineSound.Stop();
        }

        if(_tireScreechSound != null && _tireScreechSound.isPlaying)
        {
          _tireScreechSound.Stop();
        }
      }
    }

    private void TurnLeft(float hardnessForce)
    {
      _steeringAxis = _steeringAxis - (Time.deltaTime * hardnessForce * _steeringSpeed);

      if(_steeringAxis < -1f)
      {
        _steeringAxis = -1f;
      }

      var steeringAngle = _steeringAxis * _maxSteeringAngle;
      _frontLeftCollider.steerAngle = Mathf.Lerp(_frontLeftCollider.steerAngle, steeringAngle, _steeringSpeed);
      _frontRightCollider.steerAngle = Mathf.Lerp(_frontRightCollider.steerAngle, steeringAngle, _steeringSpeed);
    }

    public void TurnRight(float hardnessForce)
    {
      _steeringAxis = _steeringAxis + (Time.deltaTime * hardnessForce * _steeringSpeed);

      if(_steeringAxis > 1f)
      {
        _steeringAxis = 1f;
      }

      var steeringAngle = _steeringAxis * _maxSteeringAngle;
      _frontLeftCollider.steerAngle = Mathf.Lerp(_frontLeftCollider.steerAngle, steeringAngle, _steeringSpeed);
      _frontRightCollider.steerAngle = Mathf.Lerp(_frontRightCollider.steerAngle, steeringAngle, _steeringSpeed);
    }

    public void ResetSteeringAngle()
    {
      if(_steeringAxis < 0f)
      {
        _steeringAxis = _steeringAxis + (Time.deltaTime * 20f * _steeringSpeed);
      }
      else if(_steeringAxis > 0f)
      {
        _steeringAxis = _steeringAxis - (Time.deltaTime * 20f * _steeringSpeed);
      }

      if(Mathf.Abs(_frontLeftCollider.steerAngle) < 1f)
      {
        _steeringAxis = 0f;
      }

      var steeringAngle = _steeringAxis * _maxSteeringAngle;
      _frontLeftCollider.steerAngle = Mathf.Lerp(_frontLeftCollider.steerAngle, steeringAngle, _steeringSpeed);
      _frontRightCollider.steerAngle = Mathf.Lerp(_frontRightCollider.steerAngle, steeringAngle, _steeringSpeed);
    }

    private void AnimateWheelMeshes()
    {
      try
      {
        Quaternion FLWRotation;
        Vector3 FLWPosition;
        _frontLeftCollider.GetWorldPose(out FLWPosition, out FLWRotation);
        _frontLeftMesh.transform.position = FLWPosition;
        _frontLeftMesh.transform.rotation = FLWRotation;

        Quaternion FRWRotation;
        Vector3 FRWPosition;
        _frontRightCollider.GetWorldPose(out FRWPosition, out FRWRotation);
        _frontRightMesh.transform.position = FRWPosition;
        _frontRightMesh.transform.rotation = FRWRotation;

        Quaternion RLWRotation;
        Vector3 RLWPosition;
        _rearLeftCollider.GetWorldPose(out RLWPosition, out RLWRotation);
        _rearLeftMesh.transform.position = RLWPosition;
        _rearLeftMesh.transform.rotation = RLWRotation;

        Quaternion RRWRotation;
        Vector3 RRWPosition;
        _rearRightCollider.GetWorldPose(out RRWPosition, out RRWRotation);
        _rearRightMesh.transform.position = RRWPosition;
        _rearRightMesh.transform.rotation = RRWRotation;
      }
      catch(Exception ex)
      {
        Debug.LogWarning(ex);
      }
    }

    private void GoForward(int accelerationValue, int maxSpeed)
    {
      if (Mathf.Abs(_localVelocityX) > 2.5f)
      {
            _isDrifting = true;
            DriftCarPS();
      }
      else
      {
            _isDrifting = false;
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
        if(Mathf.RoundToInt(_carSpeed) < maxSpeed)
        {
          _frontLeftCollider.brakeTorque = 0;
          _frontLeftCollider.motorTorque = (accelerationValue * 50f) * _throttleAxis;
          _frontRightCollider.brakeTorque = 0;
          _frontRightCollider.motorTorque = (accelerationValue * 50f) * _throttleAxis;
          _rearLeftCollider.brakeTorque = 0;
          _rearLeftCollider.motorTorque = (accelerationValue * 50f) * _throttleAxis;
          _rearRightCollider.brakeTorque = 0;
          _rearRightCollider.motorTorque = (accelerationValue * 50f) * _throttleAxis;
        }
        else
        {
    	_frontLeftCollider.motorTorque = 0;
    	_frontRightCollider.motorTorque = 0;
        _rearLeftCollider.motorTorque = 0;
    	_rearRightCollider.motorTorque = 0;
    	}
      }
    }

    private void GoReverse()
    {
      if(Mathf.Abs(_localVelocityX) > 2.5f)
      {
        _isDrifting = true;
        DriftCarPS();
      }
      else
      {
        _isDrifting = false;
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
        if(Mathf.Abs(Mathf.RoundToInt(_carSpeed)) < _maxReverseSpeed)
        {
          _frontLeftCollider.brakeTorque = 0;
          _frontLeftCollider.motorTorque = (_accelerationMultiplier * 50f) * _throttleAxis;
          _frontRightCollider.brakeTorque = 0;
          _frontRightCollider.motorTorque = (_accelerationMultiplier * 50f) * _throttleAxis;
          _rearLeftCollider.brakeTorque = 0;
          _rearLeftCollider.motorTorque = (_accelerationMultiplier * 50f) * _throttleAxis;
          _rearRightCollider.brakeTorque = 0;
          _rearRightCollider.motorTorque = (_accelerationMultiplier * 50f) * _throttleAxis;
        }
        else 
        {
    	 _frontLeftCollider.motorTorque = 0;
    	 _frontRightCollider.motorTorque = 0;
         _rearLeftCollider.motorTorque = 0;
    	 _rearRightCollider.motorTorque = 0;
    	}
      }
    }

    private void ThrottleOff()
    {
      _frontLeftCollider.motorTorque = 0;
      _frontRightCollider.motorTorque = 0;
      _rearLeftCollider.motorTorque = 0;
      _rearRightCollider.motorTorque = 0;
    }

    private void DecelerateCar()
    {
      if(Mathf.Abs(_localVelocityX) > 2.5f)
      {
        _isDrifting = true;
        DriftCarPS();
      }
      else
      {
        _isDrifting = false;
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

      _carRigidbody.velocity = _carRigidbody.velocity * (1f / (1f + (0.025f * _decelerationMultiplier)));
      _frontLeftCollider.motorTorque = 0;
      _frontRightCollider.motorTorque = 0;
      _rearLeftCollider.motorTorque = 0;
      _rearRightCollider.motorTorque = 0;

      if(_carRigidbody.velocity.magnitude < 0.25f)
      {
        _carRigidbody.velocity = Vector3.zero;
        CancelInvoke("DecelerateCar");
      }
    }

    private void Brakes()
    {
      _frontLeftCollider.brakeTorque = _brakeForce;
      _frontRightCollider.brakeTorque = _brakeForce;
      _rearLeftCollider.brakeTorque = _brakeForce;
      _rearRightCollider.brakeTorque = _brakeForce;
    }

    private void Handbrake()
    {
      CancelInvoke("RecoverTraction");
      _driftingAxis = _driftingAxis + (Time.deltaTime);
      float secureStartingPoint = _driftingAxis * _extremumSlipFLW * _handbrakeDriftMultiplier;

      if(secureStartingPoint < _extremumSlipFLW)
      {
        _driftingAxis = _extremumSlipFLW / (_extremumSlipFLW * _handbrakeDriftMultiplier);
      }

      if(_driftingAxis > 1f)
      {
        _driftingAxis = 1f;
      }

      if(Mathf.Abs(_localVelocityX) > 2.5f)
      {
        _isDrifting = true;
      }
      else
      {
        _isDrifting = false;
      }

      if(_driftingAxis < 1f)
      {
        _wheelFrictionFL.extremumSlip = _extremumSlipFLW * _handbrakeDriftMultiplier * _driftingAxis;
        _frontLeftCollider.sidewaysFriction = _wheelFrictionFL;

        _wheelFrictionFR.extremumSlip = _extremumSlipFRW * _handbrakeDriftMultiplier * _driftingAxis;
        _frontRightCollider.sidewaysFriction = _wheelFrictionFR;

        _wheelFrictionRL.extremumSlip = _extremumSlipRLW * _handbrakeDriftMultiplier * _driftingAxis;
        _rearLeftCollider.sidewaysFriction = _wheelFrictionRL;

        _wheelFrictionRR.extremumSlip = _extremumSlipRRW * _handbrakeDriftMultiplier * _driftingAxis;
        _rearRightCollider.sidewaysFriction = _wheelFrictionRR;
      }

      _isTractionLocked = true;
      DriftCarPS();
    }

    private void DriftCarPS()
    {
      if(_useEffects)
      {
        try
        {
          if(_isDrifting)
          {
            _smokeSystemRLW.Play();
            _smokeSystemRRW.Play();
          }
          else if(!_isDrifting)
          {
            _smokeSystemRLW.Stop();
            _smokeSystemRRW.Stop();
          }
        }
        catch(Exception ex)
        {
          Debug.LogWarning(ex);
        }

        try
        {
          if((_isTractionLocked || Mathf.Abs(_localVelocityX) > 5f) && Mathf.Abs(_carSpeed) > 12f)
          {
            _tireSkidRLW.emitting = true;
            _tireSkidRRW.emitting = true;
          }
          else 
          {
            _tireSkidRLW.emitting = false;
            _tireSkidRRW.emitting = false;
          }
        }
        catch(Exception ex)
        {
          Debug.LogWarning(ex);
        }
      }
      else if(!_useEffects)
      {
        if(_smokeSystemRLW != null)
        {
          _smokeSystemRLW.Stop();
        }

        if(_smokeSystemRRW != null)
        {
          _smokeSystemRRW.Stop();
        }

        if(_tireSkidRLW != null)
        {
          _tireSkidRLW.emitting = false;
        }

        if(_tireSkidRRW != null) 
        {
          _tireSkidRRW.emitting = false;
        }
      }

    }

    private void RecoverTraction()
    {
      _isTractionLocked = false;
      _driftingAxis = _driftingAxis - (Time.deltaTime / 1.5f);

      if(_driftingAxis < 0f)
      {
        _driftingAxis = 0f;
      }

      if(_wheelFrictionFL.extremumSlip > _extremumSlipFLW)
      {
        _wheelFrictionFL.extremumSlip = _extremumSlipFLW * _handbrakeDriftMultiplier * _driftingAxis;
        _frontLeftCollider.sidewaysFriction = _wheelFrictionFL;

        _wheelFrictionFR.extremumSlip = _extremumSlipFRW * _handbrakeDriftMultiplier * _driftingAxis;
        _frontRightCollider.sidewaysFriction = _wheelFrictionFR;

        _wheelFrictionRL.extremumSlip = _extremumSlipRLW * _handbrakeDriftMultiplier * _driftingAxis;
        _rearLeftCollider.sidewaysFriction = _wheelFrictionRL;

        _wheelFrictionRR.extremumSlip = _extremumSlipRRW * _handbrakeDriftMultiplier * _driftingAxis;
        _rearRightCollider.sidewaysFriction = _wheelFrictionRR;

        Invoke("RecoverTraction", Time.deltaTime);

      }
      else if (_wheelFrictionFL.extremumSlip < _extremumSlipFLW)
      {
        _wheelFrictionFL.extremumSlip = _extremumSlipFLW;
        _frontLeftCollider.sidewaysFriction = _wheelFrictionFL;

        _wheelFrictionFR.extremumSlip = _extremumSlipFRW;
        _frontRightCollider.sidewaysFriction = _wheelFrictionFR;

        _wheelFrictionRL.extremumSlip = _extremumSlipRLW;
        _rearLeftCollider.sidewaysFriction = _wheelFrictionRL;

        _wheelFrictionRR.extremumSlip = _extremumSlipRRW;
        _rearRightCollider.sidewaysFriction = _wheelFrictionRR;

        _driftingAxis = 0f;
      }
    }
}