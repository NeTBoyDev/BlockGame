using _Scripts.Abstractions;
using _Scripts.Entities.Block;
using UnityEngine;

namespace _Scripts.Factory
{
    public class BlockFactory : FactoryBase<BlockPresenter,BlockModelBase,Vector3>
    {
        public override BlockPresenter Produce(BlockModelBase model, Vector3 position)
        {
            var block = new GameObject($"{model.Color}Block");
                
            var presenter = block.AddComponent<BlockPresenter>();
            presenter.Initialize(model);
        
            presenter.gameObject.layer = LayerMask.NameToLayer(LayerEnum.Block.ToString());
                
            block.transform.position = position;

            return presenter;
        }
    }
}
