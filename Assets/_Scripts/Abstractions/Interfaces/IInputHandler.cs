using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Abstractions
{
    public interface IInputHandler
    {
        public bool isDetecting { get; protected set; }

        public void StartDetection();

        public void StopDetection();

        public UniTaskVoid HandleTouchDetection();

        public event Action<Vector3> OnPointerDown; 
        public event Action<Vector3> OnPointerUp; 
        public event Action<Vector3> OnPointerMove; 
        
    }
}
