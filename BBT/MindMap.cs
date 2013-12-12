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
            public TreeElement(AMindMap mindmap)
            {
                this._mindmap = mindmap;
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
                TreeElement element = new TreeElement(this._mindmap);
                element.node = node;
                element.parent = this;
                this.children.Add(element);
            }

            internal void delete(TreeElement knoten, bool recursive)
            {
                if ((knoten.children.Count > 0) && !recursive)
                    throw new ETreeDeleteNotAllowed("Der Knoten, den du löschen wolltest hat noch Kinderknoten.");

                if (recursive)
                {
                    for (int count = knoten.children.Count-1; count >= 0; count-- )
                    {
                        TreeElement child = knoten.children[count];
                        this._mindmap.removeNode(child.node, true);
                    }
                }
                this.children.Remove(knoten);
            }

            internal void invalidateChilds()
            {
                foreach (TreeElement child in children)
                    child.node.invalidate();
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
            node.changeNodeEvent += node_changeNodeEvent;
        }

        private Size _drawSize;

        public override void setDrawSize(Size newSize)
        {
            this._drawSize.Width = Math.Max(newSize.Width, this._drawSize.Width);
            this._drawSize.Height = Math.Max(newSize.Height, this._drawSize.Height);
            this.changeDrawSize(this, this._drawSize);
        }

        private void node_changeNodeEvent(object sender, ANode node)
        {
            if (this._nodeRegistry.node == node)
                this._nodeRegistry.invalidateChilds();
            else
            {
                TreeElement element = this._nodeRegistry.getAnyChild(node);
                if (element != null)
                    element.invalidateChilds();
            }

            this.setDrawSize(new Size(Math.Max(this._drawSize.Width, node.getRectangle().Right), 
                Math.Max(this._drawSize.Height, node.getRectangle().Bottom)));
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
            element.changeNodeEvent += node_changeNodeEvent;

            this.onAddNode(this, element);
        }

        public override void removeNode(ANode element, bool recursive = false)
        {
            if (element == null)
                throw new ENodeIsNull("der übergebene Knoten ist null!");
            TreeElement knoten = this._nodeRegistry.getAnyChild(element);
            if ((knoten == null) || (this._nodeRegistry.node == element))
                throw new ENodeNotExist("Den Knoten, den du löschen wolltes, gibt es nicht oder ist der Hauptknoten.");

            TreeElement parent = knoten.parent;
            parent.delete(knoten, recursive);

            this.onRemoveNode(this, element);
        }

        public override Size getDrawSize()
        {
            return this._drawSize;
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
