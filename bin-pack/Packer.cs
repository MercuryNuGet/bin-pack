using System;

namespace Mercury.BinPack
{
    public sealed class Packer<T>
    {
        private readonly Func<T,int> _getWidth;
        private readonly Func<T, int> _getHeight;

        public Packer(Func<T,int> getWidth, Func<T,int> getHeight)
        {
            _getWidth = getWidth;
            _getHeight = getHeight;
        }

        public PackResult Pack(T item)
        {
            return new PackResult { 
                TotalWidth = _getWidth(item),
                TotalHeight = _getHeight(item)
            };
        }
    }
}
