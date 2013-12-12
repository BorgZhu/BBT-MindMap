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
        /// <summary>
        /// erzeugt ein Rechteck mit Text und Icon wenn vorhanden
        /// </summary>
        /// <param name="node"></param>
        /// <returns>das fertig gezeichnete Nodeobjekt</returns>
        Grid IForm.getStrokeFromNode(ANode node)
        {
            Grid rechteck = new Grid();
            rechteck.Background = new SolidColorBrush(node.getStyle().getBackgroundColor());
            rechteck.Height = node.getRectangle().Height;
            rechteck.Width = node.getRectangle().Width;
            Rectangle zeichnung = new Rectangle();

            zeichnung.RadiusX = rechteck.Height/10;
            zeichnung.RadiusY = rechteck.Height/10;
            SolidColorBrush farbe = new SolidColorBrush(node.getStyle().getColor().Item1);

            if (node.getStyle().getColor().Item2)
            {               
                zeichnung.Fill = farbe;
                zeichnung.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));                              
            }
            else
            {
                zeichnung.Stroke = farbe;
                
            }
            if (node.getStyle().getActivated())
                zeichnung.StrokeThickness = 4;
            else
                zeichnung.StrokeThickness = 2;

            rechteck.Children.Add(zeichnung);

            StackPanel textPanel = new StackPanel();
            textPanel.Orientation=Orientation.Horizontal;
            textPanel.HorizontalAlignment= System.Windows.HorizontalAlignment.Center;
            textPanel.VerticalAlignment = System.Windows.VerticalAlignment.Center;


            if (node.getStyle().getIcon() != null)
            {
                Image bild = new Image();
                bild.Source = node.getStyle().getIcon();
                bild.Width = node.getRectangle().Width/4;
                bild.Height = node.getRectangle().Width / 4;
                bild.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                bild.VerticalAlignment=System.Windows.VerticalAlignment.Center;
                textPanel.Children.Add(bild);
            }

            TextBlock text = new TextBlock();
            text.FontSize = node.getStyle().getFontsize();
            text.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            text.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            text.Text = node.getText();
            textPanel.Children.Add(text);

            rechteck.Children.Add(textPanel);
            
            return rechteck;
        }

        public override string ToString()
        {
            return "Rechteck";
        }
    }
}
