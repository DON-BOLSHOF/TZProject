using UniRx;

namespace Boards.BoardCells
{
    public class BoardCellState
    {
        private ReactiveProperty<BoardCellVisualState> _boardCellVisualState;

        public ReactiveProperty<BoardCellVisualState> BoardCellVisualState => _boardCellVisualState;

        public BoardCellState()
        {
            _boardCellVisualState = new ReactiveProperty<BoardCellVisualState>();
        }
    }
    
    
    public enum BoardCellVisualState
    {
        None,
        MouseVisited,
        ActiveUnitCell,
        PotentialAttack,
        PotentialMove,
        AllyCell
    }
}