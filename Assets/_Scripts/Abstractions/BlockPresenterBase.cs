using System;
using System.Threading;
using _Scripts.Entities.Block;
using _Scripts.Systems.Game;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace _Scripts.Abstractions
{
    [RequireComponent(typeof(BlockView),typeof(AudioSource))]
    public class BlockPresenterBase : MonoBehaviour
    {
        public BlockModelBase Model { get; set; }

        public bool IsDrag { get; private set; }
        
        public BoxCollider2D Collider { get; private set; }
        
        protected TweenerCore<Vector3, Vector3, VectorOptions> moveTween;

        protected BlockView View { get; set; }
        private AudioSource _source { get; set; }
        
        
        public virtual void DestroyBlock()
        {
            OnDestroy?.Invoke();
            
            Destroy(gameObject);
        }

        public virtual void PlaceOn(Vector3 place)
        {
            var distance = Vector3.Distance(transform.position, place);
            var placeTime = distance/3;
            moveTween = transform.DOMove(place, placeTime).SetEase(Ease.OutBounce);
            moveTween.onComplete += () => OnFall?.Invoke();
        }
        
        public event Action OnStartDrag;
        public event Action OnStopDrag;
        public event Action OnFall;
        public event Action OnDestroy;

        public virtual void Initialize(BlockModelBase model)
        {
            if (View == null)
                View = GetComponent<BlockView>();
            if (_source == null)
                _source = GetComponent<AudioSource>();
            
            Model = model;
            
            View.SetSprite(Model.Sprite);
            View.SetColor(Model.Color);
            
            Collider = GetComponent<BoxCollider2D>();
            if (Collider == null)
                Collider = gameObject.AddComponent<BoxCollider2D>();

            OnStartDrag += () => Play(Model.PickUpClip);
            OnStartDrag += () => View.OnStartDrag();
            
            OnStopDrag += () => Play(Model.DropClip);
            OnStopDrag += () => View.OnStopDrag();
            
            //OnDestroy += () => Play(Model.DestroyClip);
        }

        public virtual void StartDrag()
        {
            IsDrag = true;
            moveTween.Kill();
            
            
            OnStartDrag?.Invoke();
        }

        public virtual void StopDrag() 
        {
            IsDrag = false; 
            
            OnStopDrag?.Invoke();
        }
        public void HandleDrag(Vector3 position)
        {
            if (!IsDrag)
                return;
            transform.position = position;

        }

        public void HideUnderMask()
        {
            View.HideUnderMask();
            gameObject.layer = 0;
        }
        
        private void OnEnable()
        {
            if (View == null)
                View = GetComponent<BlockView>();
        }

        protected void Play(AudioClip clip)
        {
            _source.Stop();
            _source.PlayOneShot(clip);
        }
    }
}
