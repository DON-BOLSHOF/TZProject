using Units;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class UnitPackFactoryInstaller : MonoInstaller
    {
        [SerializeField] private UnitPack _unitPack;
        
        public override void InstallBindings()
        {
            Container.BindFactory<UnitPack, UnitPackFactory>().FromComponentInNewPrefab(_unitPack);
        }
    }
}