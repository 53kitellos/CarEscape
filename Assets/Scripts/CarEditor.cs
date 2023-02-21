using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(PlayerCarControl))]
[System.Serializable]
public class CarEditor : Editor
{
  enum displayFieldType {DisplayAsAutomaticFields, DisplayAsCustomizableGUIFields}

  private PlayerCarControl _car;
  private SerializedObject _so;
  private SerializedProperty _maxSpeed;
  private SerializedProperty _maxReverseSpeed;
  private SerializedProperty _accelerationMultiplier;
  private SerializedProperty _maxSteeringAngle;
  private SerializedProperty _steeringSpeed;
  private SerializedProperty _brakeForce;
  private SerializedProperty _decelerationMultiplier;
  private SerializedProperty _handbrakeDriftMultiplier;
  private SerializedProperty _bodyMassCenter;
  private SerializedProperty _frontLeftMesh;
  private SerializedProperty _frontLeftCollider;
  private SerializedProperty _frontRightMesh;
  private SerializedProperty _frontRightCollider;
  private SerializedProperty _rearLeftMesh;
  private SerializedProperty _rearLeftCollider;
  private SerializedProperty _rearRightMesh;
  private SerializedProperty _rearRightCollider;
  private SerializedProperty _useEffects;
  private SerializedProperty _particleSystemRLW;
  private SerializedProperty _particleSystemRRW;
  private SerializedProperty _tireSkidRLW;
  private SerializedProperty _tireSkidRRW;
  private SerializedProperty _useUI;
  private SerializedProperty _carSpeedText;
  private SerializedProperty _nitroBar;
  private SerializedProperty _useSounds;
  private SerializedProperty _carEngineSound;
  private SerializedProperty _tireScreechSound;
  private SerializedProperty _nitroSound;
  private SerializedProperty _nitroCollectedSound;


    private void OnEnable()
  {
    _car = (PlayerCarControl)target;
    _so = new SerializedObject(target);

    _maxSpeed = _so.FindProperty("MaxSpeed");
    _maxReverseSpeed = _so.FindProperty("MaxReverseSpeed");
    _accelerationMultiplier = _so.FindProperty("AccelerationMultiplier");
    _maxSteeringAngle = _so.FindProperty("MaxSteeringAngle");
    _steeringSpeed = _so.FindProperty("SteeringSpeed");
    _brakeForce = _so.FindProperty("BrakeForce");
    _decelerationMultiplier = _so.FindProperty("DecelerationMultiplier");
    _handbrakeDriftMultiplier = _so.FindProperty("HandbrakeDriftMultiplier");
    _bodyMassCenter = _so.FindProperty("BodyMassCenter");
    _frontLeftMesh = _so.FindProperty("FrontLeftMesh");
    _frontLeftCollider = _so.FindProperty("FrontLeftCollider");
    _frontRightMesh = _so.FindProperty("FrontRightMesh");
    _frontRightCollider = _so.FindProperty("FrontRightCollider");
    _rearLeftMesh = _so.FindProperty("RearLeftMesh");
    _rearLeftCollider = _so.FindProperty("RearLeftCollider");
    _rearRightMesh = _so.FindProperty("RearRightMesh");
    _rearRightCollider = _so.FindProperty("RearRightCollider");
    _useEffects = _so.FindProperty("UseEffects");
    _particleSystemRLW = _so.FindProperty("RLWParticleSystem");
    _particleSystemRRW = _so.FindProperty("RRWParticleSystem");
    _tireSkidRLW = _so.FindProperty("RLWTireSkid");
    _tireSkidRRW = _so.FindProperty("RRWTireSkid");
    _useUI = _so.FindProperty("UseUI");
    _carSpeedText = _so.FindProperty("CarSpeedText");
    _nitroBar = _so.FindProperty("NitroBar");
    _useSounds = _so.FindProperty("UseSounds");
    _carEngineSound = _so.FindProperty("CarEngineSound");
    _tireScreechSound = _so.FindProperty("TireScreechSound");
    _nitroSound = _so.FindProperty("NitroSound");
    _nitroCollectedSound = _so.FindProperty("NitroCollectedSound");
    }

    public override void OnInspectorGUI()
  {
    _so.Update();
    GUILayout.Space(25);

    GUILayout.Label("CAR SETUP", EditorStyles.boldLabel);
    GUILayout.Space(10);

    _maxSpeed.intValue = EditorGUILayout.IntSlider("Max Speed:", _maxSpeed.intValue, 20, 190);
    _maxReverseSpeed.intValue = EditorGUILayout.IntSlider("Max Reverse Speed:", _maxReverseSpeed.intValue, 10, 120);
    _accelerationMultiplier.intValue = EditorGUILayout.IntSlider("Acceleration Multiplier:", _accelerationMultiplier.intValue, 1, 10);
    _maxSteeringAngle.intValue = EditorGUILayout.IntSlider("Max Steering Angle:", _maxSteeringAngle.intValue, 10, 45);
    _steeringSpeed.floatValue = EditorGUILayout.Slider("Steering Speed:", _steeringSpeed.floatValue, 0.1f, 1f);
    _brakeForce.intValue = EditorGUILayout.IntSlider("Brake Force:", _brakeForce.intValue, 100, 600);
    _decelerationMultiplier.intValue = EditorGUILayout.IntSlider("Deceleration Multiplier:", _decelerationMultiplier.intValue, 1, 10);
    _handbrakeDriftMultiplier.intValue = EditorGUILayout.IntSlider("Drift Multiplier:", _handbrakeDriftMultiplier.intValue, 1, 10);
    EditorGUILayout.PropertyField(_bodyMassCenter, new GUIContent("Mass Center of Car: "));
    GUILayout.Space(25);

    GUILayout.Label("WHEELS", EditorStyles.boldLabel);
    GUILayout.Space(10);

    EditorGUILayout.PropertyField(_frontLeftMesh, new GUIContent("Front Left Mesh: "));
    EditorGUILayout.PropertyField(_frontLeftCollider, new GUIContent("Front Left Collider: "));
    EditorGUILayout.PropertyField(_frontRightMesh, new GUIContent("Front Right Mesh: "));
    EditorGUILayout.PropertyField(_frontRightCollider, new GUIContent("Front Right Collider: "));
    EditorGUILayout.PropertyField(_rearLeftMesh, new GUIContent("Rear Left Mesh: "));
    EditorGUILayout.PropertyField(_rearLeftCollider, new GUIContent("Rear Left Collider: "));
    EditorGUILayout.PropertyField(_rearRightMesh, new GUIContent("Rear Right Mesh: "));
    EditorGUILayout.PropertyField(_rearRightCollider, new GUIContent("Rear Right Collider: "));
    GUILayout.Space(25);

    GUILayout.Label("EFFECTS", EditorStyles.boldLabel);
    GUILayout.Space(10);

    _useEffects.boolValue = EditorGUILayout.BeginToggleGroup("Use effects (particle systems)?", _useEffects.boolValue);
    GUILayout.Space(10);

    EditorGUILayout.PropertyField(_particleSystemRLW, new GUIContent("Rear Left Particle System: "));
    EditorGUILayout.PropertyField(_particleSystemRRW, new GUIContent("Rear Right Particle System: "));
    EditorGUILayout.PropertyField(_tireSkidRLW, new GUIContent("Rear Left Trail Renderer: "));
    EditorGUILayout.PropertyField(_tireSkidRRW, new GUIContent("Rear Right Trail Renderer: "));
    EditorGUILayout.EndToggleGroup();
    GUILayout.Space(25);

    GUILayout.Label("UI", EditorStyles.boldLabel);
    GUILayout.Space(10);

    _useUI.boolValue = EditorGUILayout.BeginToggleGroup("Use UI (Speed text)?", _useUI.boolValue);
    GUILayout.Space(10);

    EditorGUILayout.PropertyField(_carSpeedText, new GUIContent("Speed Text (UI): "));
    EditorGUILayout.PropertyField(_nitroBar, new GUIContent("Nitro (UI): ")); 
    EditorGUILayout.EndToggleGroup();
    GUILayout.Space(25);

    GUILayout.Label("SOUNDS", EditorStyles.boldLabel);
    GUILayout.Space(10);

    _useSounds.boolValue = EditorGUILayout.BeginToggleGroup("Use Sounds (car sounds)?", _useSounds.boolValue);
    GUILayout.Space(10);

    EditorGUILayout.PropertyField(_carEngineSound, new GUIContent("Car Engine Sound: "));
    EditorGUILayout.PropertyField(_tireScreechSound, new GUIContent("Tire Screech Sound: "));
    EditorGUILayout.PropertyField(_nitroSound, new GUIContent("Nitro Sound: "));
    EditorGUILayout.PropertyField(_nitroCollectedSound, new GUIContent("Nitro Collected Sound: "));


    EditorGUILayout.EndToggleGroup();
    GUILayout.Space(10);

    _so.ApplyModifiedProperties();
  }
}