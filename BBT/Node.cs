using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;

namespace BBT
{
    class Node : ANode
    {
        Rect grundform;
        ANode parent;
        String text="";
        IStyle stil;
        IForm shape = new Rechteck();

        public override Rect getRectangle()
        {
            if (grundform != null)
                return grundform;
            else
                return new Rect(0, 0, 0, 0);
        }
        public override void setRectangle(Rect rectangle)
        {
            this.grundform = rectangle;
            changeNode(this, this);
        }

        public override ANode getParent()
        {
            return parent;
        }
        public override void setParent(ANode parent)
        {
            this.parent = parent;
        }

        public override string getText()
        {
            return text;
        }
        public override void setText(string text)
        {
            this.text = text;
            changeNode(this, this);
        }

        public override IStyle getStyle()
        {
            if (stil != null)
                return stil;
            else
                return null;
        }

        public override void setStyle(AStyle style)
        {
            this.stil = style;
            base.setStyle(style);
        }

        public override void setForm(IForm form)
        {
            this.shape = form;
            changeNode(this, this);
        }
        public override IForm getForm()
        {
            if (shape != null)
                return shape;
            else
                return null;
        }

        public override string toJson()
        {
            throw new NotImplementedException();
        }


        public override void fromJson(string Json)
        {
            throw new NotImplementedException();
        }
    }
}
