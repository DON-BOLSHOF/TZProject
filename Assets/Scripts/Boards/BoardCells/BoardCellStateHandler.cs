namespace Boards.BoardCells
{
    public class BoardCellStateHandler
    {
        private BoardCellState _boardCellState;

        private BoardCellVisualState _cellVisual;

        public BoardCellStateHandler(BoardCellState boardCellState)
        {
            _boardCellState = boardCellState;
        }

        public void MouseVisit()
        {
            _cellVisual = _boardCellState.BoardCellVisualState.Value;
            
            _boardCellState.BoardCellVisualState.Value = BoardCellVisualState.MouseVisited;
        }

        public void MouseExit()
        {
            _boardCellState.BoardCellVisualState.Value = _cellVisual;
        }

        public void SetAllyHighlight()
        {
            _boardCellState.BoardCellVisualState.Value = BoardCellVisualState.AllyCell;
        }

        public void SetEnemyHighlight()
        {
            _boardCellState.BoardCellVisualState.Value = BoardCellVisualState.PotentialAttack;
        }

        public void SetActiveUnitCell()
        {
            _boardCellState.BoardCellVisualState.Value = BoardCellVisualState.ActiveUnitCell;
        }

        public void SetPotentialMove()
        {
            _boardCellState.BoardCellVisualState.Value = BoardCellVisualState.PotentialMove;
        }

        public void SetCommonHighlight()
        {
            _boardCellState.BoardCellVisualState.Value = BoardCellVisualState.None;
        }
    }
}