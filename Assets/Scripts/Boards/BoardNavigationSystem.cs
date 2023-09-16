using System;
using System.Collections.Generic;
using System.Linq;
using Boards.BoardCells;
using UnityEngine;
using Zenject;

namespace Boards
{
    public class BoardNavigationSystem
    {
        [Inject] private Board _board;

        public List<Vector2> MakeDestination(BoardCell from, BoardCell to, int currentMovementPoint)
        {
            var result = new List<Vector2>();
             var delta = to.Position - from.Position;

            var step = new Vector2(from.Position.x, from.Position.y);

            while (Math.Abs(delta.y) > 0.01 && currentMovementPoint > 0)
            {
                var temp = delta.y > 0 ? new Vector2(0, +0.4875f) : new Vector2(0, -0.4875f);
                var movementValue = delta.x > 0 ? new Vector2(0.5f, 0) + temp : new Vector2(-0.5f, 0) + temp;
                delta -= movementValue;
                step += movementValue;

                currentMovementPoint--;
                result.Add(step);
            }

            while (Math.Abs(delta.x) > 0.01 && currentMovementPoint > 0)
            {
                var movementValue = delta.x > 0 ? new Vector2(+1, 0) : new Vector2(-1, 0);
                delta -= movementValue;
                step += movementValue;

                currentMovementPoint--;
                result.Add(step);
            }

            result = result.TakeWhile(vector2 =>
                _board.GetCellByGlobalPosition(vector2) != null &&
                _board.GetCellByGlobalPosition(vector2).AssignedUnitPack == null).ToList();

            return result;
        }
    }
}