using System.Collections.Generic;

namespace _Scripts.Abstractions.Interfaces
{
    public interface IBlockPlacementService
    {
        bool CanPlace(BlockPresenterBase block, List<BlockPresenterBase> tower);
        void Place(BlockPresenterBase block, List<BlockPresenterBase> tower);

        bool IsTowerFull(List<BlockPresenterBase> tower);
    }
}