using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBT
{
    using EDuplicateNode = Exception;
    using ENodeNotExist = Exception;
    using ENodeNotDeleted = Exception;
    class MindMap : AMindMap
    {
        private List<ANode> _nodes;
        public MindMap()
        {
            this._nodes = new List<ANode>();
        }

        public override void addNode(ANode element)
        {
            if (this._nodes.Contains(element))
                throw new EDuplicateNode("Den Knoten, den du hinzufügen wolltest gibt es schon!");
            this._nodes.Add(element);
            this.onAddNode(this, element);
        }

        public override void removeNode(ANode element)
        {
            if (!this._nodes.Contains(element))
                throw new ENodeNotExist("Den Knoten, den du löschen wolltes, gibt es nicht.");
            if (!this._nodes.Remove(element))
                throw new ENodeNotDeleted("Der Knoten konnte nicht gelöscht werden!");
            this.onRemoveNode(this, element);
        }
    }
}
