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
    class Ellipse : IForm
    {
        Grid IForm.getStrokeFromNode(ANode node)
        {
            Grid ellipse = new Grid();
            ellipse.Height = node.getRectangle().Height;
            ellipse.Width = node.getRectangle().Width;
            Rectangle zeichnung = new Rectangle();
            zeichnung.RadiusX = ellipse.Width;
            zeichnung.RadiusY = ellipse.Height/2;
            SolidColorBrush farbe = new SolidColorBrush(Color.FromRgb(100, 100, 100)); //new SolidColorBrush(node.getStyle().getColor().Item1);

            if (node.getStyle().getColor().Item2)
            {
                zeichnung.Fill = farbe;
                zeichnung.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                zeichnung.StrokeThickness = 2;
                ellipse.Children.Add(zeichnung);
            }
            else
            {
                zeichnung.Stroke = farbe;
                zeichnung.StrokeThickness = 2;
                ellipse.Children.Add(zeichnung);
            }

            return ellipse;
        }

        public override string ToString()
        {
            return "Ellipse";
        }
    }
}
