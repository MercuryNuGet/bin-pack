using System;
using System.Collections.Generic;
using System.Linq;

namespace Mercury.BinPack
{
    public sealed class Packer<T>
    {
        private readonly Func<T, int> _getWidth;
        private readonly Func<T, int> _getHeight;

        public Packer(Func<T, int> getWidth, Func<T, int> getHeight)
        {
            _getWidth = getWidth;
            _getHeight = getHeight;
        }

        public PackResult Pack(params T[] items)
        {
            var root = new TreeNode<T>();

            foreach (Item<T> item in items
                .Select(i => new Item<T>(i, new Size(_getWidth(i), _getHeight(i))))
                .OrderByDescending(i => i.Volume))
            {
                root.Add(item);
            }

            return new PackResult
            {
                TotalWidth = root.Width,
                TotalHeight = root.Height
            };
        }
    }

    internal sealed class Item<T>
    {
        private readonly T _item;
        private readonly Size _size;

        public Item(T item, Size size)
        {
            _item = item;
            _size = size;
        }

        public int Width { get { return _size.Width; } }
        public int Height { get { return _size.Height; } }
        public int Volume { get { return _size.Volume; } }
    }

    internal sealed class TreeNode<T>
    {
        private Item<T> _item;
        private TreeNode<T> _right;
        private TreeNode<T> _below;
        public int X { get; private set; }
        public int Y { get; private set; }

        internal IEnumerable<TreeNode<T>> GetAllLeafs()
        {
            if (_item == null)
            {
                yield return this;
                yield break;
            }
            foreach (TreeNode<T> leaf in _right.GetAllLeafs()) yield return leaf;
            foreach (TreeNode<T> leaf in _below.GetAllLeafs()) yield return leaf;
        }

        internal void Add(Item<T> item)
        {
            if (_item == null)
            {
                _item = item;
                _right = new TreeNode<T> {Depth = Depth + 1, X = X + _item.Width, Y = Y};
                _below = new TreeNode<T> {Depth = Depth + 1, X = X, Y = Y + _item.Height};
            }
            else
            {
                TreeNode<T> toAddTo = GetAllLeafs().OrderBy(l => l.D2).First();
                toAddTo.Add(item);
            }
        }

        public int D2 {get { return X*X + Y*Y; }}

        public int Width
        {
            get
            {
                if (_item == null) return 0;
                int result = _item.Width;
                if (_right != null) result += _right.Width;
                return result;
            }
        }
        public int Height
        {
            get
            {
                if (_item == null) return 0;
                int result = _item.Height;
                if (_below != null) result += _below.Height;
                return result;
            }
        }

        public int Depth { get; private set; }
    }
}
