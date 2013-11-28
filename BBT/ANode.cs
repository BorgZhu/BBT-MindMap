using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BBT
{
    abstract class ANode
    {
        protected void changedNodeEventHandler(object sender, ANode node)
        {
            if (this.changeNodeEvent != null)
                changeNodeEvent(this, node);
        }

        public virtual Rect getRectangle();
        public virtual void setRectangle(Rect rectangle);
        public virtual ANode getParent();
        public virtual List<ANode> getChildren();
        public virtual string getText();
        public virtual IStyle getStyle();
        public delegate void changedNodeEventHandler(object sender, ANode node);

        public event changedNodeEventHandler changeNodeEvent;
    }
}
