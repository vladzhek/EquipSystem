using Cinemachine;
using UnityEngine;

public class DollyCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineDollyCart _dollyCart;
    [SerializeField] private float _sensitivity = 1f;

    private bool _isDragging = false;
    private float _startMouseX;
    private float _startDollyPosition;

    private void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
            _startMouseX = Input.mousePosition.x;
            _startDollyPosition = _dollyCart.m_Position;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }
        
        if (_isDragging)
        {
            var currentMouseX = Input.mousePosition.x;
            var deltaX = currentMouseX - _startMouseX;
            var newPosition = _startDollyPosition + (deltaX * _sensitivity * Time.deltaTime);
            
            _dollyCart.m_Position = Mathf.Clamp01(newPosition);
        }
    }
}