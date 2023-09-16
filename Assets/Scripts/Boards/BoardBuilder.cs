using System;
using System.Collections.Generic;
using Boards.BoardCells;
using UnityEngine;

namespace Boards
{
    public class BoardBuilder : MonoBehaviour
    {
        [SerializeField] private BoardCell _prefab;
        [SerializeField] private Transform _startPoint;

        [SerializeField] private Grid _grid;

        private BoardCellFactory _factory;

        private void Awake()
        {
            _factory = new BoardCellFactory(_prefab);
        }

        public List<List<BoardCell>> GenerateBoard(int width, int height)
        {
            var result = new List<List<BoardCell>>();

            var startGridPosition = _grid.WorldToCell(_startPoint.position);

            for (var y = startGridPosition.y; y > startGridPosition.y - height; y--)
            {
                var x = (Math.Abs(startGridPosition.y - height) - y) % 2 == 1
                    ? startGridPosition.x
                    : startGridPosition.x - 1; //Гексогены требуют изворотливости)

                List<BoardCell> row = new();

                for (; x < startGridPosition.x + width; x++)
                {
                    var cellPosition = _grid.CellToWorld(new Vector3Int(x, y, startGridPosition.z));
                    var cell = _factory.Create(cellPosition, transform);
                    row.Add(cell);
                }

                result.Add(row);
            }

            return result;
        }
    }
}