using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipUtils
{
    public static class MatrixUtils
    {
        public static bool AreEqual(bool[,] matrix1, bool[,] matrix2)
        {
            if (matrix1.GetLength(0) != matrix2.GetLength(0) || matrix1.GetLength(1) != matrix2.GetLength(1))
            {
                return false;
            }

            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix1.GetLength(1); j++)
                {
                    if (matrix1[i, j] != matrix2[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool[,] ChangeSize(bool[,] matrix, int width, int height)
        {
            bool[,] newMatrix = new bool[width, height];
            for (int i = 0; i < Math.Min(width, matrix.GetLength(0)); i++)
            {
                for (int j = 0; j < Math.Min(height, matrix.GetLength(1)); j++)
                {
                    newMatrix[i, j] = matrix[i, j];
                }
            }
            return newMatrix;
        }

        public static bool[,] Copy(bool[,] matrix)
        {
            return ChangeSize(matrix, matrix.GetLength(0), matrix.GetLength(1));
        }

        public static bool[,] Transpose(bool[,] matrix)
        {
            bool[,] newMatrix = new bool[matrix.GetLength(1), matrix.GetLength(0)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    newMatrix[j, i] = matrix[i, j];
                }
            }
            return newMatrix;
        }

        public static bool[,] Rotate(bool[,] matrix)
        {
            bool[,] newMatrix = Transpose(matrix);
            for (int i = 0; i < newMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < newMatrix.GetLength(1) / 2; j++)
                {
                    (newMatrix[i, newMatrix.GetLength(1) - j - 1], newMatrix[i, j]) = (newMatrix[i, j], newMatrix[i, newMatrix.GetLength(1) - j - 1]);
                }
            }
            return newMatrix;
        }
    }
}
