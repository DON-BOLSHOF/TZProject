using Signals;
using UniRx;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(PlayerInputSystem))]
public class PlayerHandler : MonoBehaviour, IInitializable
{
    private PlayerInputSystem _playerInputSystem;

    [Inject] private readonly SignalBus _signalBus;
    
    public void Initialize()
    {
        _playerInputSystem = GetComponent<PlayerInputSystem>();
        _playerInputSystem.OnMouseMoved.Subscribe(MouseMove).AddTo(this);
        _playerInputSystem.OnMouseClicked.Subscribe(MouseClick).AddTo(this);
        _playerInputSystem.OnSpaceClicked.Subscribe(_ => SpaceClick()).AddTo(this);
    }

    private void MouseMove(Vector2 value)
    {
        _signalBus.Fire(new PickSignal{PickPosition = Camera.main!.ScreenToWorldPoint(value)});
    }   
    
    private void MouseClick(Vector2 value)
    {
        _signalBus.Fire(new CellClickedSignal{CellClickedPosition = Camera.main!.ScreenToWorldPoint(value)});
    }

    private void SpaceClick()
    {
        _signalBus.Fire(new NextTurnSignal());
    }
}