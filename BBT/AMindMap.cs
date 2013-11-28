using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public abstract void removeNode(ANode node);
        public abstract void addNode(ANode node);
        public delegate void addNodeEventHandler(object sender, ANode node);
        public delegate void removeNodeEventHandler(object sender, ANode node);

        public event addNodeEventHandler addNodeEvent;
        public event removeNodeEventHandler removeNodeEvent;
    }
}
