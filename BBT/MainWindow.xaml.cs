using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BBT
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AMindMap _mindMap;
        private Dictionary<ANode, Stroke> _nodeRegistry;
        public MainWindow()
        {
            InitializeComponent();
            this._mindMap = new MindMap();
            this._mindMap.addNodeEvent += AddNodeEvent_MindMap;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void AddObjectBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void AddNodeEvent_MindMap(object sender, ANode node)
        {

        }



        private void ToolboxAdd()
        {
            if (this.DockPanel.Children.Contains(Toolbox) == false)
            {
                this.DockPanel.Children.Insert(1, Toolbox);
            }
        }
        private void ToolboxRemove()
        {
            if (this.DockPanel.Children.Contains(Toolbox))
            {
                this.DockPanel.Children.Remove(Toolbox);
            }
        }
        private void ToolboxAdd_Click(object sender, RoutedEventArgs e)
        {
            ToolboxAdd();
        }
        private void ToolboxRemove_Click(object sender, RoutedEventArgs e)
        {
            ToolboxRemove();
        }

        private void ExitFullscreen()
        {
            this.WindowStyle = System.Windows.WindowStyle.ThreeDBorderWindow;
            this.WindowState = System.Windows.WindowState.Normal;
            this.DockPanel.Children.Insert(0, MenuBar);
            ToolboxAdd();
        }
        private void Fullscreen_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowStyle != System.Windows.WindowStyle.None)
            {
                this.WindowStyle = System.Windows.WindowStyle.None;
                this.WindowState = System.Windows.WindowState.Maximized;
                this.DockPanel.Children.Remove(MenuBar);
                ToolboxRemove();
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.FontSizeLabel.Content = "Schriftgrösse: " + System.Convert.ToInt32(this.FontSlider.Value).ToString() + "px";
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void KeyPressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape && this.WindowStyle == System.Windows.WindowStyle.None)
            {
                ExitFullscreen();
            }
        }

        private void ChooseColor_Click(object sender, RoutedEventArgs e)
        {
            //Farbe Wählen
            ColorDialog MyDialog = new ColorDialog();
            // Keeps the user from selecting a custom color.
            MyDialog.AllowFullOpen = false;

            // Update the text box color if the user clicks OK 
            MyDialog.ShowDialog();
           
        }

        private void ChangeBackgroundColor_Click(object sender, RoutedEventArgs e)
        {
            //Farbe Wählen
            ColorDialog MyDialog = new ColorDialog();
            // Keeps the user from selecting a custom color.
            MyDialog.AllowFullOpen = false;

            // Update the text box color if the user clicks OK 
            MyDialog.ShowDialog();

            Color myColor = System.Windows.Media.Color.FromArgb(1, MyDialog.Color.R, MyDialog.Color.G, MyDialog.Color.B);

            SolidColorBrush myBrush = new SolidColorBrush();
            myBrush.Color = System.Windows.Media.Color.FromArgb(255, MyDialog.Color.R, MyDialog.Color.G, MyDialog.Color.B);

            this.MindMapCanvas.Background = myBrush;

        }
    }
}
