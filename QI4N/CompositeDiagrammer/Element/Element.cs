﻿namespace CompositeDiagrammer
{
    using System.Drawing.Drawing2D;

    using QI4N.Framework;

    public interface ElementComposite : Element, Identity, TransientComposite
    {
    }


    [Mixins(typeof(ElementMixin))]
    public interface Element
    {
        void Render(RenderInfo renderInfo);
    }

    public class ElementMixin : Element
    {
        [This]
        private object self;

        [This]
        private Shape shape;

        public void Render(RenderInfo renderInfo)
        {
            var bordered = this.self as Bordered;
            var filled = this.self as Filled;
            var container = this.self as Container;

            using (GraphicsPath path = this.shape.GetPath())
            {
                if (filled != null)
                {
                    filled.RenderFilling(renderInfo, path);
                }

                if (container != null)
                {
                    container.RenderChildren(renderInfo);
                }

                if (bordered != null)
                {
                    bordered.RenderBorder(renderInfo, path);
                }
            }
        }
    }
}