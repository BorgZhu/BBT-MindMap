using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BBT
{
    interface IStyle
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Farbe und True, wenn gefüllt</returns>
        Tuple<Color, bool> getColor();
        void setColor(Tuple<Color, bool> color);
    }
}
