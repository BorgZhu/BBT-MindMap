using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BBT
{
    /// <summary>
    /// abstrakte Klasse für alle Nodes
    /// </summary>
    public abstract class ANode
    {
        /// <summary>
        /// true wenn Node geändert wird
        /// </summary>
        private bool _onUpdateing = false;

        /// <summary>
        /// start vieler Änderungen
        /// </summary>
        public void beginUpdate()
        {
            this._onUpdateing = true;
        }

        /// <summary>
        /// ende vieler Änderungen
        /// </summary>
        public void endUpdate()
        {
            this._onUpdateing = false;
            this.changeNode(this, this);
        }

        /// <summary>
        /// Getter für das Rectangle, dass den Zeichenbereich angibt.
        /// </summary>
        /// <returns>der Zeichenbereich</returns>
        public abstract Rect getRectangle();
        /// <summary>
        /// Setter für das Rectangle, dass den Zeichenbereich angibt.
        /// </summary>
        /// <param name="rectangle">der neue Zeichenbereich</param>
        public abstract void setRectangle(Rect rectangle);

        /// <summary>
        /// gibt den Elternknoten zurück
        /// </summary>
        /// <returns>der Elternknoten</returns>
        public abstract ANode getParent();
        /// <summary>
        /// setzt den Elternknoten
        /// </summary>
        /// <param name="parent">Elternknoten</param>
        public abstract void setParent(ANode parent);

        /// <summary>
        /// Getter für den Text des Knotens
        /// </summary>
        /// <returns>Text des Knotens</returns>
        public abstract string getText();
        /// <summary>
        /// Setter für den Text des Knotens
        /// </summary>
        /// <param name="text">neuer Text des Knotens</param>
        public abstract void setText(string text);

        /// <summary>
        /// gibt den Style, der vom Knoten verwendet wird, zurück
        /// </summary>
        /// <returns>Style, der verwendet wird</returns>
        public abstract IStyle getStyle();
        /// <summary>
        /// setzt den Style, der verwendet werden soll
        /// </summary>
        /// <param name="style">der neue Style, der vom Knoten verwendet werden soll</param>
        public virtual void setStyle(AStyle style)
        {
            style.changeStyleEvent += this.onChangedStyle;
        }
        
        /// <summary>
        /// reicht das ändern Event vom Style durch an den Knoten
        /// </summary>
        /// <param name="sender">die Instanz, die den Style geändert hat</param>
        /// <param name="node">das Style, dass geändert wurde</param>
        protected virtual void onChangedStyle(object sender, IStyle node)
        {
            this.changeNode(sender, this);
        }

        /// <summary>
        /// setzt die Form mit der der Knoten angezeigt wird
        /// </summary>
        /// <param name="form">die neue Form mit der der Knoten angezeigt wird</param>
        public abstract void setForm(IForm form);
        /// <summary>
        /// gubt die Form mit der der Knoten aktuell angezeigt wird zurück
        /// </summary>
        /// <returns>die aktuelle Form</returns>
        public abstract IForm getForm();

        /// <summary>
        /// ein Wrapper um die Methode getStrokeFromNode vom aktuellen Style
        /// </summary>
        /// <returns>das Grid vom aktuellen Knoten</returns>
        public Grid getGrid()
        {
            return this.getForm().getStrokeFromNode(this);
        }

        /// <summary>
        /// gibt die XML-Repräsentation vom aktuellen Knoten zurück
        /// </summary>
        /// <returns>die XML-Repräsentation vom aktuellen Knoten</returns>
        public abstract string toJson();
        /// <summary>
        /// erstellt einen Knoten von einer XML-Repräsentation
        /// </summary>
        /// <param name="XML">die XML-Repräsentation vom Knoten</param>
        public abstract void fromJson(string Json);

        public delegate void changedNodeEventHandler(object sender, ANode node);

        public event changedNodeEventHandler changeNodeEvent;

        /// <summary>
        /// ein kleiner Wrapper um die Events!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="node"></param>
        protected void changeNode(object sender, ANode node)
        {
            if ((this.changeNodeEvent != null) && (!this._onUpdateing))
                changeNodeEvent(this, node);
        }

        public void invalidate()
        {
            this.changeNode(this, this);
        }
    }
}
