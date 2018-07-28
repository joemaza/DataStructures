//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using System;
using System.Collections;
using System.Collections.Generic;

namespace Collections.Custom
{
    /// <summary>
    /// Heap.
    /// </summary>
    public class Heap<T> : IEnumerable<T>, ICollection, IReadOnlyCollection<T>
    {
        #region Fields
        /// <summary>
        /// The comparer.
        /// </summary>
        private readonly IComparer<T> _comparer = Comparer<T>.Default;

        /// <summary>
        /// The items.
        /// </summary>
        private readonly List<T> _items = new List<T>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Collections.Custom.Heap`1"/> class.
        /// </summary>
        /// <param name="isMinHeap">If set to <c>true</c> is minimum heap.</param>
        /// <param name="items">Items.</param>
        public Heap(bool isMinHeap = false, IEnumerable<T> items = null)
        {
            IsMinHeap = isMinHeap;

            if(items != null)
            {
                foreach(var item in items)
                {
                    Push(item);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Collections.Custom.Heap`1"/> class.
        /// </summary>
        /// <param name="comparer">Comparer.</param>
        public Heap(IComparer<T> comparer)
        {
            _comparer = comparer;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count => ((ICollection) _items).Count;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Collections.Custom.Heap`1"/> is
        /// minimum heap.
        /// </summary>
        /// <value><c>true</c> if is minimum heap; otherwise, <c>false</c>.</value>
		public bool IsMinHeap { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Collections.Custom.Heap`1"/> is synchronized.
        /// </summary>
        /// <value><c>true</c> if is synchronized; otherwise, <c>false</c>.</value>
        public bool IsSynchronized => ((ICollection) _items).IsSynchronized;

        /// <summary>
        /// Gets the sync root.
        /// </summary>
        /// <value>The sync root.</value>
        public object SyncRoot => ((ICollection) _items).SyncRoot;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Clear this instance.
        /// </summary>
        public void Clear()
        {
            _items.Clear();
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">Array.</param>
        /// <param name="index">Index.</param>
        public void CopyTo(Array array, int index)
        {
            ((ICollection) _items).CopyTo(array, index);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>) _items).GetEnumerator();
        }

        /// <summary>
        /// System.s the collections. IE numerable. get enumerator.
        /// </summary>
        /// <returns>The collections. IE numerable. get enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>) _items).GetEnumerator();
        }

        /// <summary>
        /// Peek this instance.
        /// </summary>
        /// <returns>The peek.</returns>
        public T Peek()
        {
            if(_items.Count == 0) throw new InvalidOperationException($"Heap contains no items.");

            return _items[0];
        }

        /// <summary>
        /// Pop this instance.
        /// </summary>
        /// <returns>The pop.</returns>
        public T Pop()
        {
            if(_items.Count == 0) throw new InvalidOperationException($"Heap contains no items.");

            var item = _items[0];

            _items.RemoveAt(0);

            UpHeap();

            return item;
        }

        /// <summary>
        /// Push the specified item.
        /// </summary>
        /// <returns>The push.</returns>
        /// <param name="item">Item.</param>
        public T Push(T item)
        {
            _items.Add(item);

            DownHeap();

            return item;
        }

        /// <summary>
        /// Pushes the items.
        /// </summary>
        /// <param name="items">Items.</param>
        public void PushItems(IEnumerable<T> items)
        {
            foreach(var item in items)
            {
                Push(item);
            }
        }

        /// <summary>
        /// Compare the specified left and right items.
        /// </summary>
        /// <remarks>Changes the semantics depending on the value of _isMax</remarks>
        /// <returns>The compare result.</returns>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        protected int Compare(T left, T right)
        {
            int result = _comparer.Compare(left, right);

            if(result == 0)
            {
                return 0;
            }

            if(IsMinHeap)
            {
                return result;
            }
            else
            {
                return result * -1;
            }
        }

        /// <summary>
        /// Downs the heap.
        /// </summary>
        private void DownHeap()
        {
            int index = 0;

            while(HasLeftChild(index))
            {
                int smalledChildIndex = LeftChildIndex(index);

                if(HasRightChild(index) && (Compare(RightChild(index), LeftChild(index)) < 0))
                {
                    smalledChildIndex = RightChildIndex(index);
                }

                if(Compare(_items[index], _items[smalledChildIndex]) < 0)
                {
                    break;
                }
                else
                {
                    Swap(index, smalledChildIndex);
                }

                index = smalledChildIndex;
            }
        }
        /// <summary>
        /// Hases the left child.
        /// </summary>
        /// <returns><c>true</c>, if left child was hased, <c>false</c> otherwise.</returns>
        /// <param name="index">Index.</param>
        private bool HasLeftChild(int index) => LeftChildIndex(index) < _items.Count;
        /// <summary>
        /// Hases the parent.
        /// </summary>
        /// <returns><c>true</c>, if parent was hased, <c>false</c> otherwise.</returns>
        /// <param name="index">Index.</param>
        private bool HasParent(int index) => ParentIndex(index) >= 0;
        /// <summary>
        /// Hases the right child.
        /// </summary>
        /// <returns><c>true</c>, if right child was hased, <c>false</c> otherwise.</returns>
        /// <param name="index">Index.</param>
        private bool HasRightChild(int index) => RightChildIndex(index) < _items.Count;
        /// <summary>
        /// Lefts the child.
        /// </summary>
        /// <returns>The child.</returns>
        /// <param name="index">Index.</param>
        private T LeftChild(int index) => _items[LeftChildIndex(index)];
        /// <summary>
        /// Lefts the index of the child.
        /// </summary>
        /// <returns>The child index.</returns>
        /// <param name="index">Index.</param>
        private int LeftChildIndex(int index) => 2 * index + 1;
        /// <summary>
        /// Parent the specified index.
        /// </summary>
        /// <returns>The parent.</returns>
        /// <param name="index">Index.</param>
        private T Parent(int index) => _items[ParentIndex(index)];
        /// <summary>
        /// Parents the index.
        /// </summary>
        /// <returns>The index.</returns>
        /// <param name="index">Index.</param>
        private int ParentIndex(int index) => (index - 1) / 2;
        /// <summary>
        /// Rights the child.
        /// </summary>
        /// <returns>The child.</returns>
        /// <param name="index">Index.</param>
        private T RightChild(int index) => _items[RightChildIndex(index)];
        /// <summary>
        /// Rights the index of the child.
        /// </summary>
        /// <returns>The child index.</returns>
        /// <param name="index">Index.</param>
        private int RightChildIndex(int index) => 2 * index + 2;

        /// <summary>
        /// Swap the specified leftIndex and rightIndex.
        /// </summary>
        /// <param name="leftIndex">Left index.</param>
        /// <param name="rightIndex">Right index.</param>
        private void Swap(int leftIndex, int rightIndex)
        {
            var temp = _items[leftIndex];
            _items[leftIndex] = _items[rightIndex];
            _items[rightIndex] = temp;
        }

        /// <summary>
        /// Ups the heap.
        /// </summary>
        private void UpHeap()
        {
            int index = _items.Count - 1;

            while(HasParent(index) && (Compare(Parent(index), _items[index]) > 0))
            {
                Swap(ParentIndex(index), index);
                index = ParentIndex(index);
            }
        }

        #endregion Methods
    }
}