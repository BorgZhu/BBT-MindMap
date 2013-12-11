using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BBT
{
    class Style : AStyle
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
            this.farbeFill = color;
            changeStyle(this, this);
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
            this.isActive=active;
        }

        public override void setFontsize(int fontSize)
        {
            this.fontSize = fontSize;
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
