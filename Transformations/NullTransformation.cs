using System.Runtime.CompilerServices;

namespace HyperionGeo
{
    public class NullTransformation : ITransformation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        EcefCoordinate ITransformation.Transform(
                                    ref EcefCoordinate coordinateToTransform,
                                    bool _)
            => coordinateToTransform;
    }
}