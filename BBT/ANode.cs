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
        /// <summary>
        /// ein kleiner Wrapper um die Events!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="node"></param>
        protected void changeNode(object sender, ANode node)
        {
            if (this.changeNodeEvent != null)
                changeNodeEvent(this, node);
        }

        public abstract Rect getRectangle();
        public abstract void setRectangle(Rect rectangle);
        public abstract ANode getParent();
        public abstract List<ANode> getChildren();
        public abstract string getText();
        public abstract IStyle getStyle();
        public delegate void changedNodeEventHandler(object sender, ANode node);

        public event changedNodeEventHandler changeNodeEvent;
    }
}
