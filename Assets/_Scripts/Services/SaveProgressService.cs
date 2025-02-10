using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using _Scripts.Abstractions;
using _Scripts.Abstractions.Interfaces;
using _Scripts.Entities.Block;
using _Scripts.Factory;
using DI;
using UnityEngine;

namespace _Scripts.Services
{
    public class SaveProgressService : ISaveService<List<BlockPresenterBase>,BlockModelBase>
    {
        [Inject] private BlockFactory _blockFactory;
        private const string SaveFileName = "save.dat";
        
        public void Save(List<BlockPresenterBase> data)
        {
            List<SerializedBlockModel> serializedBlocks = SerializeBlocks(data);
            string savePath = GetSavePath();
            SerializeToFile(serializedBlocks, savePath);
        }

        public List<BlockPresenterBase> Load(BlockModelBase modelPreset)
        {
            if (modelPreset == null)
                return null;
            
            string savePath = GetSavePath();
            if (!File.Exists(savePath)) return null;

            List<SerializedBlockModel> serializedBlocks = DeserializeFromFile(savePath);
            if (serializedBlocks == null) return null;

            return DeserializeBlocks(serializedBlocks, modelPreset);
        }

        private List<SerializedBlockModel> SerializeBlocks(List<BlockPresenterBase> data)
        {
            List<SerializedBlockModel> result = new();
            foreach (var block in data)
            {
                result.Add(new SerializedBlockModel
                {
                    Color = new SerializableColor(block.Model.Color),
                    Position = new SerializableVector3(block.transform.position)
                });
            }
            return result;
        }

        private List<BlockPresenterBase> DeserializeBlocks(List<SerializedBlockModel> data, BlockModelBase modelPreset)
        {
            List<BlockPresenterBase> result = new();
            foreach (var serializedBlock in data)
            {
                var model = (BlockModelBase)modelPreset.Clone();
                model.Color = serializedBlock.Color.ToColor();

                var block = _blockFactory.Produce(model, serializedBlock.Position.ToVector());
                result.Add(block);
            }
            return result;
        }
        
        private string GetSavePath()
        {
            return Path.Combine(Application.persistentDataPath, SaveFileName);
        }

        private void SerializeToFile(List<SerializedBlockModel> data, string filePath)
        {
            try
            {
                using (FileStream file = File.Create(filePath))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(file, data);
                }
            }
            catch (IOException e)
            {
                Debug.LogError($"Error serializing data: {e.Message}");
            }
        }

        private List<SerializedBlockModel> DeserializeFromFile(string filePath)
        {
            try
            {
                using (FileStream file = File.Open(filePath, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    return (List<SerializedBlockModel>)bf.Deserialize(file);
                }
            }
            catch (IOException e)
            {
                Debug.LogError($"Error deserializing data: {e.Message}");
                return null;
            }
        }
    }
}
