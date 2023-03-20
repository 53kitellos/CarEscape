using UnityEngine;

public class ScrolledLevels : MonoBehaviour
{
    [SerializeField] private ScriptableObject[] _scriptableObjects;
    [SerializeField] private LevelView _levelView;

    private int _currentLevelIndex;

    private void Awake()
    {
        ChangeScriptableObject(0);
    }

    public void ChangeScriptableObject(int change)
    {
        _currentLevelIndex += change;

        if (_currentLevelIndex < 0)
        {
            _currentLevelIndex = _scriptableObjects.Length - 1;
        }
        else if (_currentLevelIndex > _scriptableObjects.Length-1) 
        {
            _currentLevelIndex = 0;
        }

        if (_levelView != null)
        {
            _levelView.ShowLevel((Level)_scriptableObjects[_currentLevelIndex]);
        }
    }
}