using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BBT
{

    class Rechteck : IForm
    {
        Grid IForm.getStrokeFromNode(ANode node)
        {
            Grid rechteck = new Grid();
            rechteck.Height = node.getRectangle().Height;
            rechteck.Width = node.getRectangle().Width;
            Rectangle zeichnung = new Rectangle();
            
            zeichnung.RadiusX = rechteck.Height/10;
            zeichnung.RadiusY = rechteck.Height/10;
            SolidColorBrush farbe = new SolidColorBrush(Color.FromRgb(100, 100, 100)); //new SolidColorBrush(node.getStyle().getColor().Item1);

            if (node.getStyle().getColor().Item2)
            {               
                zeichnung.Fill = farbe;
                zeichnung.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                zeichnung.StrokeThickness = 2;
                rechteck.Children.Add(zeichnung);               
            }
            else
            {
                zeichnung.Stroke = farbe;
                zeichnung.StrokeThickness = 2;
                rechteck.Children.Add(zeichnung);
            } 
            
            return rechteck;
        }

        public override string ToString()
        {
            return "Rechteck";
        }
    }
}
