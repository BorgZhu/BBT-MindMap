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
    using EInvalidTreeElement = Exception;
    using EInvalidMainNode = Exception;
    using EMindMapNotEmpty = Exception;
    using ENodeIsNull = Exception;
    using ETreeDeleteNotAllowed = Exception;
    using System.Windows.Controls;
using System.Windows.Shapes;
    using System.Windows;
    class MindMap : AMindMap
    {
        protected class TreeElement
        {
            private AMindMap _mindmap = null;
            public TreeElement parent = null;
            public ANode node = null;
            public List<TreeElement> children = null;
            public TreeElement(AMindMap mind)
            {
                this._mindmap = mind;
                this.children = new List<TreeElement>();
            }

            public TreeElement getAnyChild(ANode node)
            {
                if (this.node == null)
                    throw new EInvalidTreeElement("der Knoten wurde in diesem Baumelement nicht gesetzt!");

                foreach (TreeElement child in children)
                {
                    TreeElement result = null;
                    if (child.node == node)
                    {
                        return child;
                    }
                    else if ((result = child.getAnyChild(node)) != null)
                    {
                        return result;
                    }
                }
                return null;
            }

            public void addChild(ANode node)
            {
                TreeElement element = new TreeElement(this.parent._mindmap);
                element.node = node;
                element.parent = this;
                this.children.Add(element);
            }

            internal void delete(TreeElement knoten, bool recursive)
            {
                if ((this.children.Count > 0) && !recursive)
                    throw new ETreeDeleteNotAllowed("Der Knoten, den du löschen wolltest hat noch Kinderknoten.");

                if (knoten == null)
                {
                    foreach (TreeElement child in this.children)
                    {
                        this._mindmap.removeNode(child.node, recursive);
                    }
                }
                else
                    this.children.Remove(knoten);
            }
        }

        private TreeElement _nodeRegistry;

        protected TreeElement getNode(ANode node)
        {
            if (node == null)
                throw new ENodeIsNull("der übergebene Knoten ist null!");
            return this._nodeRegistry.getAnyChild(node);
        }

        public MindMap()
        {
            this._nodeRegistry = new TreeElement(this);
        }

        public override void setMainNode(ANode node)
        {
            if (node == null)
                throw new ENodeIsNull("der übergebene Knoten ist null!");
            if (node.getParent() != null)
                throw new EInvalidMainNode("Es ist ein Parent gesetzt, dass ist im MainNode nicht erlaubt.");
            if (this._nodeRegistry.node != null)
                throw new EMindMapNotEmpty("Die MindMap ist nicht leer!");
            this._nodeRegistry.node = node;
        }
        public override void addNode(ANode element)
        {
            if (element == null)
                throw new ENodeIsNull("der übergebene Knoten ist null!");
            if ((this._nodeRegistry.getAnyChild(element) != null) && (this._nodeRegistry.node != element))
                throw new EDuplicateNode("Den Knoten, den du hinzufügen wolltest gibt es schon!");
            TreeElement parent = this._nodeRegistry.getAnyChild(element.getParent());
            if ((parent == null) && (this._nodeRegistry.node != element.getParent()))
                throw new ENodeNotExist("Der Elternknoten existiert im Baum nicht.");

            if (parent == null)
                parent = this._nodeRegistry;

            parent.addChild(element);

            this.onAddNode(this, element);
        }

        public override void removeNode(ANode element, bool recursive = false)
        {
            if (element == null)
                throw new ENodeIsNull("der übergebene Knoten ist null!");
            TreeElement knoten = this._nodeRegistry.getAnyChild(element);
            if (knoten == null)
                throw new ENodeNotExist("Den Knoten, den du löschen wolltes, gibt es nicht oder ist der Hauptknoten.");

            TreeElement parent = knoten.parent;
            parent.delete(knoten, recursive);

            this.onRemoveNode(this, element);
        }

        public override ANode getMainNode()
        {
            return this._nodeRegistry.node;
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
