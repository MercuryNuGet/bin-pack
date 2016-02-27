using NUnit.Framework;
using System;

namespace Mercury.BinPack.Tests
{
    public sealed class PackingCases : SpecificationByMethod
    {
        public class TypeToPack
        {
            public TypeToPack(int w, int h)
            {
                Width = w;
                Height = h;
            }

            public int Width { get; set; }
            public int Height { get; set; }
        }

        private ISpecification Spec { get { throw new InvalidOperationException(); } set { Spec(value); } }

        protected override void Cases()
        {
            Spec = "When packing one item (#w x #h), "
                .Arrange(() => new Packer<TypeToPack>(t => t.Width, t => t.Height))
                .With(new { w = 1, h = 1 })
                .With(new { w = 40, h = 30 })
                .With(new { w = 20, h = 10 })
                .Act((p, d) => p.Pack(new TypeToPack(d.w, d.h)))
                .Assert("Total width is #w", (r, d) => Assert.AreEqual(d.w, r.TotalWidth))
                .Assert("Total height is #h", (r, d) => Assert.AreEqual(d.h, r.TotalHeight));

            Spec = "When packing two items (#i1) (#i2), "
              .Arrange(() => new Packer<TypeToPack>(t => t.Width, t => t.Height))
              .With(new { i1 = new { w = 1, h = 1 }, i2 = new { w = 1, h = 1 }, expected = new { w = 2, h = 1 } })
              .With(new { i1 = new { w = 1, h = 2 }, i2 = new { w = 1, h = 2 }, expected = new { w = 2, h = 2 } })
              .With(new { i1 = new { w = 2, h = 1 }, i2 = new { w = 2, h = 1 }, expected = new { w = 2, h = 2 } })
              .Act((p, d) => p.Pack(new TypeToPack(d.i1.w, d.i1.h), new TypeToPack(d.i2.w, d.i2.h)))
              .Assert("Total width is #expected", (r, d) => Assert.AreEqual(d.expected.w, r.TotalWidth))
              .Assert("Total height is #expected", (r, d) => Assert.AreEqual(d.expected.h, r.TotalHeight));

        }
    }
}
