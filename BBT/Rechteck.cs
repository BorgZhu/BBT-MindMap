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
            Grid litit = new Grid();
            litit.Height = node.getRectangle().Height;
            litit.Width = node.getRectangle().Width;

            Rectangle rectum = new Rectangle();
            rectum.Fill = new SolidColorBrush(Colors.Black);
            litit.Children.Add(rectum);



            return litit;
        }
    }
}
