// Written by Ákos Halmai, University of Pécs, Hungary.

namespace HyperionGeo
{
    public interface IProjection
    {
        public bool TryProjectForward(
            ref EllipsoidalCoordinate coordinateToProject,
            out ProjectedCoordinate projectedCoordinate);

        public EllipsoidalCoordinate ProjectInverse(ref ProjectedCoordinate coordinateToProject);
    }
}