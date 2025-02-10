using System;
using _Scripts.Abstractions;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TouchSystem : IInputHandler
{
    
    public event Action<Vector3> OnPointerDown;
    public event Action<Vector3> OnPointerUp;
    public event Action<Vector3> OnPointerMove;
    
    private bool isDetecting;
    private bool _isDetecting;

    bool IInputHandler.isDetecting
    {
        get => _isDetecting;
        set => _isDetecting = value;
    }

    public void StartDetection()
    {
        isDetecting = true;
        HandleTouchDetection().Forget();
    }

    public void StopDetection()
    {
        isDetecting = false;
    }

    public async UniTaskVoid HandleTouchDetection()
    {
        while (isDetecting)
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                var point = Camera.main.ScreenToWorldPoint(touch.position);
                var position = new Vector3(point.x, point.y);

                if (touch.phase == TouchPhase.Began)
                {
                    OnPointerDown?.Invoke(position);
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    OnPointerMove?.Invoke(position);
                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    OnPointerUp?.Invoke(position);
                }
            }

            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
        }
    }
}