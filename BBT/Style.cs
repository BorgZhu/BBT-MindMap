using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BBT
{
    /// <summary>
    /// Datenstruktur für die verschiedenen Styles und getter und setter methoden
    /// </summary>
    class Style : AStyle
    {
        Tuple<Color, bool> farbeFill;
        BitmapImage icon;
        Color BackgroundColor=Color.FromArgb(255, 255, 255, 255);
        bool isActive = false;
        int fontSize = 12;

        /// <summary>
        /// gibt farbe und Füllstatus zurück
        /// </summary>
        /// <returns>Farbe und füllstatus</returns>
        public override Tuple<Color, bool>getColor()
        {
            return farbeFill;
        }

        /// <summary>
        /// setzt Farbe und Füllstatus
        /// </summary>
        /// <param name="color"></param>
        public override void setColor(Tuple<Color, bool> color)
        {
            this.farbeFill = color;
            changeStyle(this, this);
        }

        /// <summary>
        /// verwendetes Icon wird zurückgegeben
        /// </summary>
        /// <returns></returns>
        public override BitmapImage getIcon()
        {
            return icon;
        }

        /// <summary>
        /// Icon wird gesetzt
        /// </summary>
        /// <param name="icon"></param>
        public override void setICon(BitmapImage icon)
        {
            this.icon = icon;
        }

        /// <summary>
        /// setzt ausgewählt
        /// </summary>
        /// <param name="active"></param>
        public override void setActivated(bool active)
        {
            this.isActive=active;
        }

        /// <summary>
        /// setzt Schriftgröße
        /// </summary>
        /// <param name="fontSize"></param>
        public override void setFontsize(int fontSize)
        {
            this.fontSize = fontSize;
        }

        /// <summary>
        /// gibt Fontsize zurück
        /// </summary>
        /// <returns>double Fontsize</returns>
        public override double getFontsize()
        {
            return fontSize;
        }

        /// <summary>
        /// gibt zurück ob ausgewählt
        /// </summary>
        /// <returns>bool isActive</returns>
        public override bool getActivated()
        {
            return this.isActive;
        }

        /// <summary>
        /// setzt den Hintergrund für Grid auf dem gezeichnet wird
        /// </summary>
        /// <param name="blub"></param>
        public override void setBackgroundColor(Color blub)
        {
            this.BackgroundColor = blub;
        }

        /// <summary>
        /// gibt Hintergrundfarbe des Grid zurück auf dem gezeichnet wird
        /// </summary>
        /// <returns></returns>
        public override Color getBackgroundColor()
        {
            return this.BackgroundColor;
        }
    }
  
}
