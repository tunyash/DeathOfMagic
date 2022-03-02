using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
    }
}
