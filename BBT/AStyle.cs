using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BBT
{
    abstract class AStyle : IStyle
    {

        protected void changeStyle(object sender, IStyle node)
        {
            if ((this.changeStyleEvent != null))
                changeStyleEvent(this, node);
        }

        public abstract Tuple<Color, bool> getColor();      
        public abstract void setColor(Tuple<Color, bool> color);
       
        public delegate void changedStyleEventHandler(object sender, IStyle node);

        public event changedStyleEventHandler changeStyleEvent;

        public abstract void setICon(BitmapImage icon);
        public abstract BitmapImage getIcon();

        public abstract void setActivated(bool active);


        public abstract void setFontsize(int fontSize);

        public abstract double getFontsize();


        public abstract bool getActivated();
    }
}
