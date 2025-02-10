using UnityEngine;

namespace _Scripts.Abstractions.Interfaces
{
    public interface IBoundsChecker
    {
        public bool IsWithinBounds(Bounds objectA, Bounds objectB, float percentage = 0.5f);
    }
}
