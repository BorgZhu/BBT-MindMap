using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
using System.Web;
using System.Xml.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace BBT
{
    /// <summary>
    /// Datenklasse für einen Node
    /// </summary>
    
    class Node : ANode
    {
        Rect grundform =  new Rect(0,0,0,0);
        ANode parent;
        String text="";
        IStyle stil;
        IForm shape = new Rechteck();

        
        /// <summary>
        /// gibt Rectangle für die Größe des Grid zurück
        /// </summary>
        /// <returns></returns>
        public override Rect getRectangle()
        {
            if (grundform != null)
                return grundform;
            else
                return new Rect(0, 0, 0, 0);

            
        }
        /// <summary>
        /// setzt die größe des Rectangle der die Größe des Grids bestimmt
        /// </summary>
        /// <param name="rectangle"></param>
        public override void setRectangle(Rect rectangle)
        {
            rectangle.X = Math.Max(0, rectangle.X);
            rectangle.Y = Math.Max(0, rectangle.Y);
            this.grundform = rectangle;
            changeNode(this, this);
        }
        /// <summary>
        /// gibt den Elternknoten zurück
        /// </summary>
        /// <returns></returns>
        public override ANode getParent()
        {
            
            return parent;
        }
        /// <summary>
        /// setzt den Elternknoten
        /// </summary>
        /// <param name="parent"></param>
        public override void setParent(ANode parent)
        {
            this.parent = parent;
        }
        /// <summary>
        /// gibt den Text des Knoten zurück
        /// </summary>
        /// <returns></returns>
        public override string getText()
        {
            return text + "";
        }
        /// <summary>
        /// setzt den Text des Knoten
        /// </summary>
        /// <param name="text"></param>
        public override void setText(string text)
        {
            this.text = text;
            changeNode(this, this);
        }
        /// <summary>
        /// gibt das Styleobjekt eines Nodes zurück
        /// </summary>
        /// <returns></returns>
        public override IStyle getStyle()
        {
           
            return stil;            
        }
        /// <summary>
        /// setzt den Style eines Knoten
        /// </summary>
        /// <param name="style"></param>
        public override void setStyle(AStyle style)
        {
            this.stil = style;
            base.setStyle(style);
        }
        /// <summary>
        /// setzt die Form eines Knoten
        /// </summary>
        /// <param name="form"></param>
        public override void setForm(IForm form)
        {
            this.shape = form;
            changeNode(this, this);
        }
        /// <summary>
        /// gibt die Form des Knoten zurück
        /// </summary>
        /// <returns></returns>
        public override IForm getForm()
        {
            return shape;
        }
        /// <summary>
        /// wandelt die Daten des Nodes zur Speicherung um
        /// </summary>
        /// <returns></returns>
        public override XElement toXML()
        {
            try
            {
                XElement blub = new XElement("node",

                                    new XElement("X", getRectangle().X),
                                    new XElement("Y", getRectangle().Y),
                                    new XElement("width", getRectangle().Width),
                                    new XElement("height", getRectangle().Height),
                                    new XElement("stil", getStyle().ToString()),
                                    new XElement("text", getText()),
                                    new XElement("form", getForm().ToString())
                                    );
                if (this.getStyle() != null)
                {
                    blub.Add(new XElement("style",
                                    new XElement("farbe", (getStyle().getColor() == null ? System.Windows.Media.Colors.Black : getStyle().getColor().Item1)),
                                    new XElement("fill", (getStyle().getColor() == null ? false : getStyle().getColor().Item2)),
                                    new XElement("font", getStyle().getFontsize()),
                                    //new XElement("aktiv", getStyle().getActivated()),
                                    new XElement("icon", (getStyle().getIcon() == null ? null : getStyle().getIcon().BaseUri))
                            ));
                }
                return blub;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
            
            
            
        }

        public override void fromXML(XElement XML)
        {
            XElement blub = XML;
            double x = (double)blub.Element("X");
            this.setRectangle(new Rect((double)blub.Element("X"), (double)blub.Element("Y"), (double)blub.Element("width"), (double)blub.Element("height")));
            setText(blub.Element("text").Value);
            Style thiss = new Style();
            setStyle(thiss);
           
            

            Color fff = (Color)ColorConverter.ConvertFromString(blub.Element("style").Element("farbe").Value);
            bool fill = bool.Parse(blub.Element("style").Element("fill").Value);
            thiss.setColor(Tuple.Create(fff, fill));

            thiss.setFontsize(int.Parse(blub.Element("style").Element("font").Value));
            String form = blub.Element("form").Value;
            switch(form)
            {
                case "Ellipse":setForm(new Ellipse());
                    break;
                default:setForm(new Rechteck());
                    break;
            }
        }
    }
}
