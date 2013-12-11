using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BBT
{
    class Style : AStyle
    {
        Tuple<Color, bool> farbeFill;

        public override Tuple<Color, bool>getColor()
        {
            return farbeFill;
        }

        public override void setColor(Tuple<Color, bool> color)
        {
            this.farbeFill = color;
            changeStyle(this, this);
        }
    }
  
}
