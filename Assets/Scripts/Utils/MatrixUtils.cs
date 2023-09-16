using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class MatrixUtils<T> where T : class
    {
        public static T Find(List<List<T>> matrix, Predicate<T> predicate)
        {
            var query = from row in matrix
                from cell in row
                where predicate.Invoke(cell)
                select cell;

            if (!query.Any()) return null;
            var boardCell = query.ToArray()[0];

            return boardCell;
        }
    }
}