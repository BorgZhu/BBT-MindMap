using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBT
{
    class Style : IStyle
    {
        Tuple<System.Windows.Media.Color, bool> farbeFill;

        Tuple<System.Windows.Media.Color, bool> IStyle.getColor()
        {
            return farbeFill;
        }

        void IStyle.setColor(Tuple<System.Windows.Media.Color, bool> color)
        {
            this.farbeFill = color;
        }
    }
  
}
