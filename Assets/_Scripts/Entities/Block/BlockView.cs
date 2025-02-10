using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace _Scripts.Entities.Block
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BlockView : MonoBehaviour
    {
        public SpriteRenderer _spriteRenderer { get; private set; }

        public void SetSprite(Sprite sprite)
        {
            if(_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();
            
            _spriteRenderer.sprite = sprite;
        }

        public void SetColor(Color color)
        {
            if(_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.color = color;
        }

        public TweenerCore<Vector3,Vector3,VectorOptions> DestroyBlock()
        {
            var doScale = transform.DOScale(transform.localScale * 1.5f,.45f).SetEase(Ease.OutQuart); //Вынести в отдельный класс анимки
            var doRotate = transform.DORotate(new Vector3(0,0,90),.45f).SetEase(Ease.OutQuart);
            var doAlpha = DOTween.To(() => _spriteRenderer.color, x => _spriteRenderer.color = x,
                new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0), .45f).SetEase(Ease.OutQuart);
            
            return doScale;
        }

        public void OnStartDrag()
        {
            _spriteRenderer.sortingOrder = 1;
        }
        
        public void OnStopDrag()
        {
            _spriteRenderer.sortingOrder = 0;
        }

        public void HideUnderMask()
        {
            _spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }
        
        
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}
