// Not ready. Do not use it!

using System;

namespace HyperionGeo
{

    public interface IGeometry
    {

    }

    public abstract record Geometry : IGeometry
    {

    }

    /// <summary>
    /// Not ready! Do not use it!
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public record Point<T> : Geometry, IGeometry where T : struct, ICoordinate
    {
        private readonly ICoordinate coordinate;
        private readonly Type type;
        public Datum Datum { get; set; }
        public IProjection Projection { get; set; }

        public Point(ICoordinate coordinate)
        {
            this.coordinate = coordinate ?? throw new ArgumentNullException(nameof(coordinate));
            type = typeof(T);
        }

        public double X
        {
            get
            {
                return coordinate switch
                {
                    EcefCoordinate ecefCoordinate => ecefCoordinate.X,
                    EllipsoidalCoordinate ellipsoidalCoordinate => ellipsoidalCoordinate.Lon_Radians,
                    ProjectedCoordinate projectedCoordinate => projectedCoordinate.X,
                    _ => throw new InvalidCastException("Wrong coordinate type."),
                };
            }

        }
    }

    //    public record LineStrig<T> : IGeometry, IList<T> where T : struct, ICoordinate
    //    {
    //        private readonly List<T> storage;
    //        T IList<T>.this[int index] { get => storage[index]; set => storage[index] = value; }

    //        int ICollection<T>.Count => storage.Count;

    //        bool ICollection<T>.IsReadOnly => false;

    //        void ICollection<T>.Add(T item)
    //        {
    //            throw new NotImplementedException();
    //        }

    //        void ICollection<T>.Clear() => storage.Clear();

    //        bool ICollection<T>.Contains(T item) => storage.Contains(item);

    //        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
    //        {
    //            throw new NotImplementedException();
    //        }

    //        IEnumerator<T> IEnumerable<T>.GetEnumerator()
    //        {
    //            throw new NotImplementedException();
    //        }

    //        IEnumerator IEnumerable.GetEnumerator()
    //        {
    //            throw new NotImplementedException();
    //        }

    //        int IList<T>.IndexOf(T item)
    //        {
    //            throw new NotImplementedException();
    //        }

    //        void IList<T>.Insert(int index, T item)
    //        {
    //            throw new NotImplementedException();
    //        }

    //        bool ICollection<T>.Remove(T item)
    //        {
    //            throw new NotImplementedException();
    //        }

    //        void IList<T>.RemoveAt(int index)
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }
}