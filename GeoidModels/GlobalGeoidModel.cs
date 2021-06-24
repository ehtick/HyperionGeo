namespace HyperionGeo
{

    public class GlobalGeoidModel : GeoidModel
    {
        public const double maxX = 180.125;
        public const double minX = -maxX;

        public const double maxY = 90.125;
        public const double minY = -maxY;

        public const double cellsizeX = .25;
        public const double cellsizeY = cellsizeX;

        public GlobalGeoidModel(
            string name,
            string fileName,
            int numberOfRows,
            int numberOfColumns,
            double cellsizeX,
            double cellsizeY) :
            base(name, fileName, numberOfRows, numberOfColumns, cellsizeX, cellsizeY)
        { }

        public double GetUndulationValue(in EllipsoidalCoordinate ellipsoidalCoordinate) =>
            GetUndulation((ellipsoidalCoordinate.Lon_Degrees - minX) / cellsizeX, (-ellipsoidalCoordinate.Lat_Degrees - minY) / cellsizeY, true, false);
    }
}