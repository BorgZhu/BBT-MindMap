using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BBT
{
    class Icon : IForm
    {

        public Grid getStrokeFromNode(ANode node)
        {
            Grid rechteck = new Grid();
            rechteck.Height = node.getRectangle().Height;
            rechteck.Width = node.getRectangle().Width;




            return rechteck;
        }
    }
}
