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
        Dictionary<ANode, Grid> _nodeRegistry;

        public MainWindow()
        {
            this._nodeRegistry = new Dictionary<ANode, Grid>();
            InitializeComponent();
        }

        private void handleNodeChanges(object sender, ANode node)
        {
            Grid nodeElement = node.getGrid();
            if (this._nodeRegistry.ContainsKey(node))
            {
                if (this.MindMapCanvas.Children.Contains(this._nodeRegistry[node]))
                    this.MindMapCanvas.Children.Remove(this._nodeRegistry[node]);
            }
            this._nodeRegistry[node] = nodeElement;
            Canvas.SetLeft(nodeElement, node.getRectangle().Left);
            Canvas.SetTop(nodeElement, node.getRectangle().Top);
            this.MindMapCanvas.Children.Add(nodeElement);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ANode node = new Node();
            node.changeNodeEvent += this.handleNodeChanges;
            node.beginUpdate();
            try
            {
                node.setForm(new Rechteck());
                node.setRectangle(new Rect(new Point(100, 100), new Point(200, 200)));
                
                IStyle nodeStyle = new Style();
                nodeStyle.setColor(Tuple.Create((this.colorRect.Fill as SolidColorBrush).Color, (bool)this.fillCheckBox.IsChecked));
                node.setStyle(nodeStyle);

                node.setParent(null);

                node.setText(this.nodeText.Text);
            }
            finally
            {
                node.endUpdate();
            }

        }

        private void AddObjectBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void AddNodeEvent_MindMap(object sender, ANode node)
        {

        }


        /// <summary>
        /// fügt Toobox hinzu
        /// </summary>
        private void ToolboxAdd()
        {
            if (this.DockPanel.Children.Contains(Toolbox) == false)
            {
                this.DockPanel.Children.Insert(1, Toolbox);
            }
        }
        private void ToolboxAdd_Click(object sender, RoutedEventArgs e)
        {
            ToolboxAdd();
        }
        /// <summary>
        /// entfernt die Toolbox
        /// </summary>
        private void ToolboxRemove()
        {
            if (this.DockPanel.Children.Contains(Toolbox))
            {
                this.DockPanel.Children.Remove(Toolbox);
            }
        }
        private void ToolboxRemove_Click(object sender, RoutedEventArgs e)
        {
            ToolboxRemove();
        }
        /// <summary>
        /// Beendet den Vollbildmodus
        /// </summary>
        private void ExitFullscreen()
        {
            this.WindowStyle = System.Windows.WindowStyle.ThreeDBorderWindow;
            this.WindowState = System.Windows.WindowState.Normal;
            this.DockPanel.Children.Insert(0, MenuBar);
            ToolboxAdd();
        }
        /// <summary>
        /// Vollbild des Canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = false;
            MyDialog.ShowDialog();
            //Color
            Color myColor = System.Windows.Media.Color.FromArgb(1, MyDialog.Color.R, MyDialog.Color.G, MyDialog.Color.B);
            //Brush
            SolidColorBrush myBrush = new SolidColorBrush();
            myBrush.Color = System.Windows.Media.Color.FromArgb(255, MyDialog.Color.R, MyDialog.Color.G, MyDialog.Color.B);
            //colorRect Farbe ändern
            colorRect.Fill = myBrush;
        }

        private void ChangeBackgroundColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = false;
            MyDialog.ShowDialog();
            SolidColorBrush myBrush = new SolidColorBrush();
            myBrush.Color = System.Windows.Media.Color.FromArgb(255, MyDialog.Color.R, MyDialog.Color.G, MyDialog.Color.B);

            this.MindMapCanvas.Background = myBrush;

        }
    }
}
