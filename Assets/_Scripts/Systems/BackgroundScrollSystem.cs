using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DI;
using UnityEngine;

namespace _Scripts.Systems
{
    public class BackgroundScrollSystem
    {
        #region Inject

        [Inject] private List<Material> _scrollMaterials;

        #endregion

        #region Fields

        private CancellationTokenSource _cancellationTokenSource = new();

        #endregion
        

        public void StartScroll()
        {
            HandleScroll()
                .Forget();
        }

        private async UniTaskVoid HandleScroll()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                foreach (var material in _scrollMaterials)
                {
                    var vector = new Vector2(Time.fixedTime, Time.fixedTime);
                    material.SetVector("_MainTex_ST",new Vector4(1,1,Mathf.Cos(Time.fixedTime/35),Mathf.Sin(Time.fixedTime/35)));
                }
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
            }
        }

        public void StopScroll()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
