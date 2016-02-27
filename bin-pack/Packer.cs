using System;
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

        internal void Add(Item<T> item)
        {
            if (_item == null)
            {
                _item = item;
            }
            else
            {
                _right = new TreeNode<T>();
                _below = new TreeNode<T>();
                TreeNode<T> toAddTo;

                if (_item.Width > _item.Height)
                    toAddTo = _below;
                else
                    toAddTo = _right;

                toAddTo.Add(item);
            }
        }

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
    }
}
