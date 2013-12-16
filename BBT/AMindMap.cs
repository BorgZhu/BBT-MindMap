using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace BBT
{
    abstract class AMindMap
    {
        /// <summary>
        /// Event Wrapper sendet das Event, dass ein Knoten hinzugefügt wurde an alle Listener.
        /// </summary>
        /// <param name="sender">die Instanz, in die der Knoten hinzugefügt wurde</param>
        /// <param name="node">der Knoten, der hinzugefügt wurde</param>
        protected void onAddNode(object sender, ANode node)
        {
            if (this.addNodeEvent != null)
                addNodeEvent(this, node);
        }
        /// <summary>
        /// Event Wrapper sendet das Event, dass ein Knoten entfernt wurde an alle Listener.
        /// </summary>
        /// <param name="sender">die Instanz, in aus der der Knoten entfernt wurde</param>
        /// <param name="node">der Knoten, der entfernt wurde</param>
        protected void onRemoveNode(object sender, ANode node)
        {
            if (this.removeNodeEvent != null)
                removeNodeEvent(this, node);
        }
        /// <summary>
        /// setter für das MainElement. Das MainElement hat ein paar Sonderrollen: kann nicht gelöscht werden
        /// </summary>
        /// <param name="node">Der Knoten, der als MainElement festgelegt werden soll</param>
        public abstract void setMainNode(ANode node);

        /// <summary>
        /// mit dieser Methode kann man einen Knoten aus der Mindmap entfernen
        /// </summary>
        /// <param name="node">der Knoten, der entfernt werden soll</param>
        /// <param name="recursive">gibt an, ob alle  Subknoten entfernt werden sollen</param>
        public abstract void removeNode(ANode node, bool recursive = false);

        /// <summary>
        /// fügt einen Knoten zur MindMap hinzu
        /// </summary>
        /// <param name="node">der Knoten, der hinzugefügt werden soll</param>
        public abstract void addNode(ANode node);

        /// <summary>
        /// Dieser Delegate legt die Struktur vom EventHandler fest, der das hinzufügen von Knoten signalisiert.
        /// </summary>
        /// <param name="sender">Die Instanz von Mindmap zu der ein Knoten hinzugefügt wurde</param>
        /// <param name="node">Der Knoten, der hinzugefügt wurde</param>
        public delegate void addNodeEventHandler(object sender, ANode node);

        /// <summary>
        /// Dieser Delegate legt die Struktur vom EventHandler fest, der das entfernen von Knoten signalisiert.
        /// </summary>
        /// <param name="sender">DIe Instanz von Mindmap aus der ein Knoten entfernt wurde</param>
        /// <param name="node">Der Knoten, der entfernt wurde</param>
        public delegate void removeNodeEventHandler(object sender, ANode node);

        /// <summary>
        /// Diese Statische Methode gibt die Verbindende Linie zwischen Parent- und Childnode zurück, 
        /// sowie das Anzeige Element(Grid) vom Knoten, der gezeichnet werden soll.
        /// </summary>
        /// <param name="element">Der Knoten, der gezeichnet werden soll</param>
        /// <returns>Die Linie zum Parentnode und das Anzeigeelement des Knotens.</returns>
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

        /// <summary>
        /// Diese Methode wird verwendet um die Mindmap in XML zu exportieren.
        /// </summary>
        /// <returns>Die Stringrepräsentation vom XML der der Mindmap</returns>
        public abstract XElement toXML();
        public abstract void fromXML(XElement XML);
        /// <summary>
        /// Diese Methode wird verwendet um aus dem XML der Mindmap wieder eine Mindmap zu erstellen
        /// </summary>
        /// <param name="Json">die Stringrepräsentation von dem XML der Mindmap.</param>

        /// <summary>
        /// diese Methode kann verwendet werden um den Hauptknoten zu bekommen.
        /// </summary>
        /// <returns>der Hauptknoten, wenn einer existiert.</returns>
        public abstract ANode getMainNode();

        /// <summary>
        /// Das Event, dass geworfen wird, wenn ein Knoten hinzugefügt wurde
        /// </summary>
        public event addNodeEventHandler addNodeEvent;

        /// <summary>
        /// Das Event, dass geworfen wird, wenn ein Knoten entfernt wurde
        /// </summary>
        public event removeNodeEventHandler removeNodeEvent;

        /// <summary>
        /// die Struktur des Eventhandler, der Aufgerufen wird, wenn die Größe sich ändert
        /// </summary>
        /// <param name="sender">Die Instanz, die die Größe geändert hat.</param>
        /// <param name="newSize">Die neue größe von einem Knoten</param>
        public delegate void changedSizeEventHandler(object sender, Size newSize);

        /// <summary>
        /// Das Event, dass geworfen wird, wenn sich die Größe des Knotens ändert.
        /// </summary>
        public event changedSizeEventHandler changeSizeEvent;

        /// <summary>
        /// ein kleiner Wrapper um das DrawSize Change Event
        /// </summary>
        /// <param name="sender">die Instanz, die die Größe geändert hat</param>
        /// <param name="newSize">Die neue größe der MindMap</param>
        protected void changeDrawSize(object sender, Size newSize)
        {
            if ((this.changeSizeEvent != null))
                changeSizeEvent(this, newSize);
        }

        /// <summary>
        /// ein Setter für die größe der MindMap
        /// </summary>
        /// <param name="newSize">die neue Göße der MindMap</param>
        public abstract void setDrawSize(Size newSize);

        /// <summary>
        /// ein Getter für die größe der MindMap
        /// </summary>
        /// <returns>die größe der MindMap</returns>
        public abstract Size getDrawSize();
    }
}
