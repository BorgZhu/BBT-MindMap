using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BBT
{
    public class Style : AStyle
    {
        Tuple<Color, bool> farbeFill;
        BitmapImage icon;
        bool isActive = false;
        int fontSize = 12;

        public override Tuple<Color, bool>getColor()
        {
            return farbeFill;
        }

        public override void setColor(Tuple<Color, bool> color)
        {
            if (this.farbeFill != color)
            {
                this.farbeFill = color;
                changeStyle(this, this);
            }
        }

        public override BitmapImage getIcon()
        {
            return icon;
        }

        public override void setICon(BitmapImage icon)
        {
            this.icon = icon;
        }

        public override void setActivated(bool active)
        {
            if (this.isActive != active)
            {
                this.isActive = active;
                changeStyle(this, this);
            }
        }

        public override void setFontsize(int fontSize)
        {
            if (this.fontSize != fontSize)
            {
                this.fontSize = fontSize;
                changeStyle(this, this);
            }
        }

        public override double getFontsize()
        {
            return fontSize;
        }

        public override bool getActivated()
        {
            return this.isActive;
        }
    }
  
}
