using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace BBT
{
    abstract class AMindMap
    {
        protected void onAddNode(object sender, ANode node)
        {
            if (this.addNodeEvent != null)
                addNodeEvent(this, node);
        }

        protected void onRemoveNode(object sender, ANode node)
        {
            if (this.removeNodeEvent != null)
                removeNodeEvent(this, node);
        }

        public abstract void setMainNode(ANode node);
        public abstract void removeNode(ANode node, bool recursive = false);
        public abstract void addNode(ANode node);
        public delegate void addNodeEventHandler(object sender, ANode node);
        public delegate void removeNodeEventHandler(object sender, ANode node);

        public abstract string toJson();
        public abstract void fromJson(string Json);

        public abstract Tuple<Line, Grid> getDisplay(ANode element);
        public abstract ANode getMainNode();
        public abstract Point transformCoords(Point transformPoint, Point mainNodeCoords);

        public event addNodeEventHandler addNodeEvent;
        public event removeNodeEventHandler removeNodeEvent;
    }
}
