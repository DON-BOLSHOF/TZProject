using System.Collections.Generic;
using Boards.BoardCells;
using Units;
using UnityEngine;

namespace Signals
{
    public class DestinationSignal
    {
        public Destination Destination;
    }

    public class Destination
    {
        public DestinationType DestinationType;
        
        public List<Vector2> Path;
        public UnitPack Target;
    }

    public enum DestinationType
    {
        Walk,
        Attack
    }
}