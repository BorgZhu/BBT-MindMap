using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        static public Tuple<Line, Grid> getDisplay(ANode element)
        {
            Grid grid = element.getGrid();
            Line line = new Line();
            line.Stroke = new SolidColorBrush(Colors.Black);
            line.StrokeThickness = 1;
            if (element.getParent() == null)
            {
                line = null;
            }
            else
            {
                line.X1 = element.getParent().getRectangle().Left + ((element.getParent().getRectangle().Right - element.getParent().getRectangle().Left) * 0.5);
                line.Y1 = element.getParent().getRectangle().Top + ((element.getParent().getRectangle().Bottom - element.getParent().getRectangle().Top) * 0.5);
                line.X2 = element.getRectangle().Left + ((element.getRectangle().Right - element.getRectangle().Left) * 0.5);
                line.Y2 = element.getRectangle().Top + ((element.getRectangle().Bottom - element.getRectangle().Top) * 0.5);
            }
            return Tuple.Create(line, grid);
        }

        public abstract string toJson();
        public abstract void fromJson(string Json);

        public abstract ANode getMainNode();
        static public Point transformCoords(Point transformPoint, Point mainNodeCoords)
        {
            transformPoint.X += mainNodeCoords.X;
            transformPoint.Y += mainNodeCoords.Y;
            return transformPoint;
        }

        public event addNodeEventHandler addNodeEvent;
        public event removeNodeEventHandler removeNodeEvent;

        public delegate void changedSizeEventHandler(object sender, Size newSize);

        public event changedSizeEventHandler changeSizeEvent;

        /// <summary>
        /// ein kleiner Wrapper um die Events!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="node"></param>
        protected void changeDrawSize(object sender, Size newSize)
        {
            if ((this.changeSizeEvent != null))
                changeSizeEvent(this, newSize);
        }

        public abstract void setDrawSize(Size newSize);
        public abstract Size getDrawSize();
    }
}
