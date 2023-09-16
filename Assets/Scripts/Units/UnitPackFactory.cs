using UnityEngine;
using Zenject;

namespace Units
{
    public class UnitPackFactory : PlaceholderFactory<UnitPack>
    {
        public UnitPack Create(UnitModel unitModel, ArmySide armySide, bool isInversed = false)
        {
            var prefabInstance = Create();
            prefabInstance.Initialize(unitModel,armySide);

            if (isInversed) prefabInstance.InverseDirection();

            return prefabInstance;
        }
    }
}