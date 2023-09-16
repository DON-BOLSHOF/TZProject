using Boards;
using GamePlay;
using Signals;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Board _board;
        [SerializeField] private GamePlayController _gamePlayController;
        [SerializeField] private PlayerHandler _playerHandler;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<FulfillBoardNewUnitPackSignal>();
            Container.DeclareSignal<PickSignal>();
            Container.DeclareSignal<CellClickedSignal>();
            Container.DeclareSignal<DestinationSignal>();
            Container.DeclareSignal<NextTurnSignal>();

            Container.BindInterfacesTo<PlayerHandler>().FromInstance(_playerHandler).AsSingle();
            Container.BindInterfacesTo<GamePlayController>().FromInstance(_gamePlayController).AsSingle();
            Container.BindInterfacesAndSelfTo<Board>().FromInstance(_board).AsSingle();

            Container.BindInterfacesAndSelfTo<BoardNavigationSystem>().AsSingle();
        }
    }
}