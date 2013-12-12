using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BBT
{
    /// <summary>
    /// Interface für Style
    /// </summary>
    interface IStyle
    {
       

        Tuple<Color, bool> getColor();
        void setColor(Tuple<Color, bool> color);

        void setICon(BitmapImage icon);
        BitmapImage getIcon();

        void setBackgroundColor(Color blub);
        Color getBackgroundColor();

        void setFontsize(int fontSize);
        double getFontsize();

        void setActivated(bool active);
        bool getActivated();
        

    }
}
