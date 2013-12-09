using System;
using System.Collections.Generic;
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

            Rectangle rectum = new Rectangle();
            rectum.Fill = new SolidColorBrush(Colors.Black);
            rechteck.Children.Add(rectum);



            return rechteck;
        }
    }
}
