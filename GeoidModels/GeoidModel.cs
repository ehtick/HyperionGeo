using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using static System.Math;

namespace HyperionGeo
{
    public abstract class GeoidModel
    {
        private readonly float[,] GeoidImage;

        protected GeoidModel(string name, string fileName, int numberOfRows, int numberOfColumns, double cellsizeX, double cellsizeY)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            NumberOfRows = numberOfRows;
            NumberOfColumns = numberOfColumns;
            GeoidImage = ReadFloatFile(fileName, numberOfColumns, numberOfColumns);

            CellsizeX = cellsizeX;
            CellsizeY = cellsizeY;
        }

        public string Name { get; init; }
        public int NumberOfRows { get; init; }
        public int NumberOfColumns { get; init; }
        public double CellsizeX { get; init; }
        public double CellsizeY { get; init; }
        
        private static double LinearInterpolation(double left, double right, double pos)
            => FusedMultiplyAdd(1.0 - pos, left, pos * right);
        
        protected static double BilinearInterolation(double a, double b, double c, double d, double x, double y)
            => LinearInterpolation(LinearInterpolation(a, b, x), LinearInterpolation(c, d, x), y);

        private static void WarpAround(ref double x, ref int left_x, ref int right_x, int x_max)
        {
            if (left_x == -1)
            {
                right_x = x_max;
                left_x = 0;
                x = -x;
            }
            if (left_x == x_max)
            {
                right_x = 0;
                left_x = x_max;
                x -= x_max;
            }
        }

        private static unsafe float[,] ReadFloatFile(string fileName, int rows, int columns)
        {
            FileInfo fileInfo = new(fileName);
            long len = fileInfo.Exists ? fileInfo.Length : throw new FileNotFoundException("File not found!", fileInfo.FullName);

            using MemoryMappedFile file = MemoryMappedFile.CreateFromFile(fileInfo.FullName, FileMode.Open, null, len, MemoryMappedFileAccess.Read);
            using MemoryMappedViewAccessor fileAccessor = file.CreateViewAccessor(0, len, MemoryMappedFileAccess.Read);
            float[,] geoidImage = new float[rows, columns];
            fixed (float* fltPointer = &geoidImage[0, 0])
                Buffer.MemoryCopy(fileAccessor.SafeMemoryMappedViewHandle.DangerousGetHandle().ToPointer(), fltPointer, len, len);
            return geoidImage;
        }
        protected double GetUndulation(double x,
                                       double y,
                                       bool centered = true,
                                       bool eastWestWarp = true)
        {
            if (centered) { x -= .5; y -= .5; }

            int left_x = (int)Floor(x), right_x = 1 + left_x;

            if (eastWestWarp)
                WarpAround(ref x, ref left_x, ref right_x, NumberOfColumns - 1);

            int upper_y = (int)Floor(y), lower_y = 1 + upper_y;

            float[,] geoidImage = GeoidImage;
            return BilinearInterolation(
                a: geoidImage[upper_y, left_x],
                b: geoidImage[upper_y, right_x],
                c: geoidImage[lower_y, left_x],
                d: geoidImage[lower_y, right_x],
                x: x - Truncate(x),
                y: y - Truncate(y));
        }
    }
}