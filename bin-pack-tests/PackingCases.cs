using NUnit.Framework;

namespace Mercury.BinPack.Tests
{
    public sealed class PackingCases : MercurySuite
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

            public override string ToString()
            {
                return $"{Width}x{Height}";
            }
        }

        protected override void Specifications()
        {
            Specs += "When packing one item (#w x #h), "
                .Arrange(() => new Packer<TypeToPack>(t => t.Width, t => t.Height))
                .With(new {w = 1, h = 1})
                .With(new {w = 40, h = 30})
                .With(new {w = 20, h = 10})
                .Act((p, d) => p.Pack(new TypeToPack(d.w, d.h)))
                .Assert("Total width is #w", (r, d) => Assert.AreEqual(d.w, r.TotalWidth))
                .Assert("Total height is #h", (r, d) => Assert.AreEqual(d.h, r.TotalHeight));

            Specs += "When packing two items (#i1) (#i2), "
                .Arrange(() => new Packer<TypeToPack>(t => t.Width, t => t.Height))
                .With(new {i1 = new {w = 1, h = 1}, i2 = new {w = 1, h = 1}, expected = new {w = 2, h = 1}})
                .With(new {i1 = new {w = 1, h = 2}, i2 = new {w = 1, h = 2}, expected = new {w = 2, h = 2}})
                .With(new {i1 = new {w = 2, h = 1}, i2 = new {w = 2, h = 1}, expected = new {w = 2, h = 2}})
                .With(new {i1 = new {w = 3, h = 1}, i2 = new {w = 2, h = 1}, expected = new {w = 3, h = 2}})
                .With(new {i1 = new {w = 2, h = 1}, i2 = new {w = 3, h = 1}, expected = new {w = 3, h = 2}})
                .Act((p, d) => p.Pack(new TypeToPack(d.i1.w, d.i1.h), new TypeToPack(d.i2.w, d.i2.h)))
                .Assert("Total width is #expected", (r, d) => Assert.AreEqual(d.expected.w, r.TotalWidth))
                .Assert("Total height is #expected", (r, d) => Assert.AreEqual(d.expected.h, r.TotalHeight));


            Specs += "When packing three items #1, #2, #3, "
                .Arrange(() => new Packer<TypeToPack>(t => t.Width, t => t.Height))
                .With(new TypeToPack(1, 1), new TypeToPack(1, 1), new TypeToPack(1, 1), new Size(2, 2))
                .Act((sut, a, b, c, expect) => sut.Pack(a, b, c))
                .Assert("total width should be #4.Width",(r, expect) => Assert.AreEqual(expect.Width, r.TotalWidth))
                .Assert("total height should be #4.Height", (r, expect) => Assert.AreEqual(expect.Height, r.TotalHeight));
        }
    }
}
