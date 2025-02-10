using System.Collections.Generic;
using _Scripts.Abstractions;
using UnityEngine;

namespace _Scripts.Configuration
{
    [CreateAssetMenu(menuName = "Game/Configuration")]
    public class GameConfiguration : ScriptableObject
    {
        public List<BlockModelBase> Blocks
        {
            get
            {
                InitializeModels();
                return blocks;
            }
        }

        [field:SerializeField] private List<BlockModelBase> blocks { get; set; }
        [field:SerializeField] public List<Material> ScrollMaterials { get; set; }
        [field:SerializeField] public AudioClip DropClip { get; set; }
        [field:SerializeField] public AudioClip PickClip { get; set; }
        [field:SerializeField] public AudioClip DestroyClip { get; set; }


        private void InitializeModels()
        {
            foreach (var block in blocks)
            {
                block.DropClip = DropClip;
                block.PickUpClip = PickClip;
                block.DestroyClip = DestroyClip;
            }
        }

        
    }
}