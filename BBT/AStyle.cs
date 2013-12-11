using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBT
{
    abstract class AStyle : IStyle
    {

        protected void changeStyle(object sender, IStyle node)
        {
            if ((this.changeStyleEvent != null))
                changeStyleEvent(this, node);
        }

        public abstract Tuple<System.Windows.Media.Color, bool> getColor();
       

        public abstract void setColor(Tuple<System.Windows.Media.Color, bool> color);
       

        public delegate void changedStyleEventHandler(object sender, IStyle node);

        public event changedStyleEventHandler changeStyleEvent;

        public abstract void setIcon(System.Drawing.Bitmap);
        public abstract System.Drawing.Bitmap getIcon();
    }
}
