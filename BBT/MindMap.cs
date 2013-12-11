﻿using System;
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
            public TreeElement parent = null;
            public ANode node = null;
            public List<TreeElement> children = null;
            public TreeElement()
            {
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
                TreeElement element = new TreeElement();
                element.node = node;
                element.parent = this;
                this.children.Add(element);
            }

            internal void delete(TreeElement knoten, bool recursive)
            {
                if ((this.children.Count > 0) && !recursive)
                    throw new ETreeDeleteNotAllowed("Der Knoten, den du löschen wolltest hat noch Kinderknoten.");

                if (knoten == null)
                    this.children = null;
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
            this._nodeRegistry = new TreeElement();
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

        public override Tuple<Line, Grid> getDisplay(ANode element)
        {
            Grid grid = element.getGrid();
            Line line = new Line();
            if (element.getParent() == null)
            {
                line.X1 = 0;
                line.Y1 = 0;
            }
            else
            {
                line.X1 = element.getParent().getRectangle().Left + ((element.getParent().getRectangle().Right-element.getParent().getRectangle().Left) * 0.5);
                line.Y1 = element.getParent().getRectangle().Top + ((element.getParent().getRectangle().Bottom-element.getParent().getRectangle().Top) * 0.5);
            }
            line.X2 = element.getRectangle().Left + ((element.getRectangle().Right - element.getRectangle().Left) * 0.5);
            line.Y2 = element.getRectangle().Top + ((element.getRectangle().Bottom - element.getRectangle().Top) * 0.5);
            return Tuple.Create(line, grid);
        }

        public override ANode getMainNode()
        {
            return this._nodeRegistry.node;
        }

        public override Point transformCoords(Point transformPoint, Point mainNodeCoords)
        {
            transformPoint.X += mainNodeCoords.X;
            transformPoint.Y += mainNodeCoords.Y;
            return transformPoint;
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
