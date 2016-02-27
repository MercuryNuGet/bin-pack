namespace Mercury.BinPack
{
    public sealed class Size
    {
        private readonly int _itemWidth;
        private readonly int _itemHeight;
        private readonly int _volume;

        public Size(int itemWidth, int itemHeight)
        {
            _itemWidth = itemWidth;
            _itemHeight = itemHeight;
            _volume = _itemWidth * _itemHeight;
        }

        public int Width { get { return _itemWidth; } }
        public int Height { get { return _itemHeight; } }

        public int Volume { get { return _volume; } }

        public override string ToString()
        {
            return _itemWidth + "x" + _itemHeight;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Size);
        }

        public bool Equals(Size size)
        {
            if (size == null) return false;

            return size._itemWidth == _itemWidth &&
                size._itemHeight == _itemHeight;
        }

        public override int GetHashCode()
        {
            return _itemHeight * 31 + _itemWidth;
        }
    }
}
