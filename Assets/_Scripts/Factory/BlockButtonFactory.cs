using _Scripts.Abstractions;
using DI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Factory
{
    public class BlockButtonFactory : FactoryBase<ClickableButton,BlockModelBase,ScrollRect>
    {
        [Inject] private BlockFactory _blockFactory;
    
        [Inject] private CompositeDisposable _disposables = new();
        public override ClickableButton Produce(BlockModelBase block, ScrollRect scrollRect)
        {
            var button = CreateBlockButton(block);
            SetupButtonInteractions(button, block, scrollRect);
            SubscribeToScrollRectReactivation(scrollRect);
            return button;
        }
        
        private ClickableButton CreateBlockButton(BlockModelBase block)
        {
            var blockButton = new GameObject($"{block.Color}BlockButton");
            var image = blockButton.AddComponent<Image>();
            image.color = block.Color;
            image.sprite = block.Sprite;
            return blockButton.AddComponent<ClickableButton>();
        }

        private void SetupButtonInteractions(ClickableButton button, BlockModelBase block, ScrollRect scrollRect)
        {
            button.OnPointerDown += eventData =>
            {
                scrollRect.enabled = false;
                var position = Camera.main.ScreenToWorldPoint(eventData.position);
                var producedBlock = _blockFactory.Produce(block, new Vector3(position.x, position.y));
                GamePresenter.Instance.SetBlock(producedBlock);
            };
        }

        private void SubscribeToScrollRectReactivation(ScrollRect scrollRect)
        {
            Observable.EveryUpdate()
                .Where(_ => !RectTransformUtility.RectangleContainsScreenPoint(scrollRect.transform as RectTransform, Input.mousePosition))
                .Subscribe(_ => scrollRect.enabled = true)
                .AddTo(_disposables);
        }
    }
}
