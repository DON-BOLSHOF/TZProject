using UnityEngine;

namespace Boards.BoardCells
{
    public class BoardCellFactory
    {
        private BoardCell _prefab;
        
        public BoardCellFactory(BoardCell cell)
        {
            _prefab = cell;
        }

        public BoardCell Create(Vector2 cellPosition, Transform parent)
        {
            var cell = Object.Instantiate(_prefab, cellPosition, Quaternion.identity, parent);
            cell.DynamicInitialize(new Vector2(cellPosition.x,cellPosition.y));

            return cell;
        }
    }
}