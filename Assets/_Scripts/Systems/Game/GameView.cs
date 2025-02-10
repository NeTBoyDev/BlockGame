using System.Collections.Generic;
using _Scripts.Abstractions;
using _Scripts.Factory;
using DI;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Systems.Game
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private GameObject _blockListContent;
        [SerializeField] private ScrollRect _scrollRect;
        private CanvasGroup _scrollGroup;
        private Button[] _blockButtons;
        
        [Inject] private BackgroundScrollSystem _backgroundScrollSystem;
        [Inject] private BlockButtonFactory _blockButtonFactory;
        
        public void Initialize(List<BlockModelBase> blocks)
        {
            InitializeBlockList(blocks);
            InitializeScrollBackGround();

            _scrollGroup = _scrollRect.GetComponent<CanvasGroup>();
        }

        public void DisableBlockList()
        {
            _scrollGroup.blocksRaycasts = false;
            _scrollGroup.alpha = .6f;
        }

        public void EnableBlockList()
        {
            _scrollGroup.blocksRaycasts = true;
            _scrollGroup.alpha = 1f;
        }
        
        private void InitializeBlockList(List<BlockModelBase> blocks)
        {
            foreach (var block in blocks)
            {
                var button = _blockButtonFactory.Produce(block,_scrollRect);
                
                button.transform.SetParent(_blockListContent.transform);
                button.transform.localScale = Vector3.one;
            }
        }

        private void InitializeScrollBackGround()
        {
            _backgroundScrollSystem.StartScroll();
        }
    }
}
