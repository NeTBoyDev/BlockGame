using _Scripts.Abstractions.Interfaces;
using UnityEngine;

namespace _Scripts.Services
{
    public class BoundsCheckerService : IBoundsChecker
    {
        public bool IsWithinBounds(Bounds objectA, Bounds objectB, float percentage = 0.5f)
        {
            var boundsA = objectA;
            var sizeA = boundsA.size.x * percentage;
            var posB = objectB.center.x;
            var minBound = boundsA.center.x - sizeA;
            var maxBound = boundsA.center.x + sizeA;

            return posB >= minBound && posB <= maxBound;
        }
    }
}
