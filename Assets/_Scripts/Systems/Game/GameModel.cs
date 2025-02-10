using System.Collections.Generic;
using _Scripts.Abstractions;
using DI;
using UnityEngine;

namespace _Scripts.Systems.Game
{
    [System.Serializable]
    public class GameModel
    {
        [field: SerializeField] public List<BlockPresenterBase> Tower { get; private set; } = new();
        [Inject] public List<BlockModelBase> BlockModels { get; private set; } = new();

        public void AddBlock(BlockPresenterBase block)
        {
            if(!Tower.Contains(block))
                Tower.Add(block);
        }

        public void AddRange(List<BlockPresenterBase> blocks)
        {
            Tower.AddRange(blocks);
        }

        public void RemoveBlock(BlockPresenterBase block)
        {

            if (Tower.Contains(block))
            {
                Debug.Log("Remove");
                Tower.Remove(block);
            }
        }
        
        public void RemoveUpperBlockRange(BlockPresenterBase block)
        {
            int index = Tower.IndexOf(block);
            int count = Tower.Count - index;
            Tower.RemoveRange(index,count);
        }

        public bool Contains(BlockPresenterBase block) => Tower.Contains(block);
        
        public List<BlockPresenterBase> GetUpperBlockRange(BlockPresenterBase block)
        {
            int index = Tower.IndexOf(block)+1;
            int count = Tower.Count - index;
            return Tower.GetRange(index, count);
        }
        
        public BlockPresenterBase GetUpperBlock()
        {
            return Tower[^1];
        }
    }
}
