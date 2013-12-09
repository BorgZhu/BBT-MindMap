using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BBT
{
    class Style : IStyle
    {
        Tuple<Color, bool> farbeFill;

        Tuple<Color, bool> IStyle.getColor()
        {
            return farbeFill;
        }

        void IStyle.setColor(Tuple<Color, bool> color)
        {
            this.farbeFill = color;
        }
    }
  
}
