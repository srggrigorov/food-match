using UnityEngine;
using UnityEngine.Serialization;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class CameraScreenResolution : MonoBehaviour
{
    [FormerlySerializedAs("maintainWidth")]
    [SerializeField]
    private bool _maintainWidth = true;
    [FormerlySerializedAs("adaptPosition")]
    [Range(-1, 1)]
    [SerializeField]
    private int _adaptPosition;
    private float _defaultWidth, _defaultHeight;
    private Camera _targetCamera;

    private Vector3 _cameraPos;

    private void OnEnable()
    {
        _targetCamera ??= GetComponent<Camera>();
        _cameraPos = _targetCamera.transform.position;
        _defaultHeight = _targetCamera.orthographicSize;
        _defaultWidth = _targetCamera.orthographicSize * _targetCamera.aspect;
    }

    private void Update()
    {
        if (_maintainWidth)
        {
            _targetCamera.orthographicSize = _defaultWidth / _targetCamera.aspect;
            _targetCamera.transform.position = new Vector3(_cameraPos.x,
                _adaptPosition * (_defaultHeight - _targetCamera.orthographicSize), _cameraPos.z);
        }
        else
        {
            _targetCamera.transform.position =
                new Vector3(_adaptPosition * (_defaultWidth - _targetCamera.orthographicSize * _targetCamera.aspect),
                    _cameraPos.y, _cameraPos.z);
        }
    }
}