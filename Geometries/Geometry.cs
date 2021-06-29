//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace HyperionGeo
//{

//    public interface IGeometry
//    {

//    }
//    public abstract record Geometry : IGeometry
//    {
    
//    }

//    public record Point<T> : Geometry, IGeometry where T : struct, ICoordinate
//    {
//        private readonly ICoordinate coordinate;
        
//        public Point(ICoordinate coordinate)
//        {
//            this.coordinate = coordinate ?? throw new ArgumentNullException(nameof(coordinate));
//        }
//    }

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
//}
