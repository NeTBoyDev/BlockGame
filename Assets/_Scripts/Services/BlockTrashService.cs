using System;
using _Scripts.Abstractions;
using _Scripts.Abstractions.Interfaces;
using _Scripts.Entities.Block;
using DG.Tweening;
using DI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Scripts.Services
{
    public class BlockTrashService : IBlockTrashService
    {
        private BoxCollider2D _trashHoleCollider;
        [Inject] private IBoundsChecker _boundsChecker;

        public BlockTrashService(BoxCollider2D trashHoleCollider)
        {
            if (trashHoleCollider == null)
            {
                throw new ArgumentNullException(nameof(trashHoleCollider), "Trash hole collider cannot be null.");
            }

            _trashHoleCollider = trashHoleCollider;

            PositionTrashHole();
        }

        private void PositionTrashHole()
        {
            var screenPositionX = CalculateTrashHoleXPosition();
            var worldPositionY = _trashHoleCollider.gameObject.transform.position.y;

            _trashHoleCollider.gameObject.transform.position = new Vector3(screenPositionX, worldPositionY);
        }

        private float CalculateTrashHoleXPosition()
        {
            var screenToWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 4, 0));
            return screenToWorldPoint.x;
        }
        
        public void Trash(BlockPresenterBase block)
        {
            if (block == null || _trashHoleCollider == null || _boundsChecker == null)
            {
                Debug.LogWarning("Block or collider is null. Cannot trash the block.");
                return;
            }

            if (!MayTrash(block))
            {
                block.DestroyBlock();
                return;
            }

            MoveBlockToTrash(block);
        }
        
        private void MoveBlockToTrash(BlockPresenterBase block)
        {
            var targetPosition = _trashHoleCollider.bounds.center;
            var distance = Vector3.Distance(targetPosition, block.transform.position);
            
            var moveTween = block.transform.DOMove(targetPosition, distance / 3).SetEase(Ease.OutQuad);
            moveTween.onComplete += () => block.DestroyBlock();
            
            block.HideUnderMask();
        }

        public bool MayTrash(BlockPresenterBase block)
        {
            if (block == null || block.Collider == null || _trashHoleCollider == null)
            {
                return false;
            }

            var isAboveTrashHole = block.Collider.bounds.min.y > _trashHoleCollider.bounds.max.y ;

            var isWithinHorizontalBounds = _boundsChecker.IsWithinBounds(_trashHoleCollider.bounds, block.Collider.bounds);

            return isAboveTrashHole && isWithinHorizontalBounds;
        }
    }
}
