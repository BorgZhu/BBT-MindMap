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
            ellipse.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            ellipse.Height = node.getRectangle().Height;
            ellipse.Width = node.getRectangle().Width;
            Rectangle zeichnung = new Rectangle();
            zeichnung.RadiusX = ellipse.Width;
            zeichnung.RadiusY = ellipse.Height/2;
            SolidColorBrush farbe = new SolidColorBrush(node.getStyle().getColor().Item1);

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

            if (node.getStyle().getActivated())
                zeichnung.StrokeThickness = 4;
            else
                zeichnung.StrokeThickness = 2;

            StackPanel textPanel = new StackPanel();
            textPanel.Orientation = Orientation.Horizontal;
            textPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            textPanel.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            if (node.getStyle().getIcon() != null)
            {
                Image bild = new Image();
                bild.Source = node.getStyle().getIcon();
                bild.Width = node.getRectangle().Width / 4;
                bild.Height = node.getRectangle().Width / 4;
                bild.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                bild.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                textPanel.Children.Add(bild);
            }

            TextBlock text = new TextBlock();
            text.FontSize = node.getStyle().getFontsize();
            text.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            text.VerticalAlignment = System.Windows.VerticalAlignment.Center;

           
            text.Text = node.getText();
                  
            textPanel.Children.Add(text);
            ellipse.Children.Add(textPanel);

            return ellipse;
        }

        public override string ToString()
        {
            return "Ellipse";
        }
    }
}
