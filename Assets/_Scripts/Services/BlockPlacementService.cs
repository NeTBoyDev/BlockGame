using System.Collections.Generic;
using _Scripts.Abstractions;
using _Scripts.Abstractions.Interfaces;
using DI;
using UnityEngine;

namespace _Scripts.Services
{
    public class BlockPlacementService : IBlockPlacementService
    {
        
        [Inject("GroundCollider")] private BoxCollider2D GroundCollider { get; set; }
        [Inject] private IBoundsChecker _boundsChecker;
        private const float MinPlacementOffset = 0.1f;
        public bool CanPlace(BlockPresenterBase block, List<BlockPresenterBase> tower)
        {
            if (!block)
                return false;

            if (IsTowerFull(tower))
                return false;
            
            var blockBounds = block.Collider.bounds;
            var topBounds = tower.Count == 0 ? GroundCollider : tower[^1].Collider;

            return blockBounds.min.y + MinPlacementOffset > topBounds.bounds.max.y &&
                   _boundsChecker.IsWithinBounds(topBounds.bounds, block.Collider.bounds);
        }

        public void Place(BlockPresenterBase block, List<BlockPresenterBase> tower)
        {
            var bounds = block.Collider.bounds;
            var position = tower.Count > 0
                ? new Vector3(block.transform.position.x, GroundCollider.bounds.max.y + bounds.extents.y + bounds.size.y * tower.Count)
                : new Vector3(block.transform.position.x, GroundCollider.bounds.max.y + bounds.extents.y);

            block.PlaceOn(position);
            tower.Add(block);
        }

        public bool IsTowerFull(List<BlockPresenterBase> tower)
        {
            if (tower.Count == 0)
                return false;   
            
            var topScreenBound = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
            
            var block = tower[0];
            var bounds = block.Collider.bounds;
            var height = GroundCollider.bounds.max.y + bounds.extents.y + bounds.size.y * (tower.Count);

            return height > topScreenBound.y;
        }

        
    }
}