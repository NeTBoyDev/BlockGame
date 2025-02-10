using System;
using UnityEngine;

namespace _Scripts.Abstractions
{
    [System.Serializable]
    public class BlockModelBase : ICloneable
    {
        [field:SerializeField] public Color Color { get; set; }
        [field:SerializeField] public Sprite Sprite { get; set; }
        
        [HideInInspector]
        public AudioClip DropClip { get; set; }
        [HideInInspector]
        public AudioClip PickUpClip { get; set; }
        [HideInInspector]
        public AudioClip DestroyClip { get; set; }

        public object Clone()
        {
            var clone = new BlockModelBase();
            clone.Color = Color;
            clone.Sprite = Sprite;
            clone.DropClip = DropClip;
            clone.PickUpClip = PickUpClip;
            clone.DestroyClip = DestroyClip;
            return clone;
        }
    }

    [Serializable]
    public struct SerializedBlockModel
    {
        public SerializableColor Color { get; set; }
        public SerializableVector3 Position { get; set; }
    }
    
    [Serializable]
    public struct SerializableColor
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public SerializableColor(Color color)
        {
            r = color.r;
            g = color.g;
            b = color.b;
            a = color.a;
        }

        public Color ToColor()
        {
            return new Color(r, g, b, a);
        }
    }
    
    [Serializable]
    public struct SerializableVector3
    {
        public float x;
        public float y;
        public float z;

        public SerializableVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
            
        }

        public Vector3 ToVector()
        {
            return new Vector3(x,y,z);
        }
    }
}
