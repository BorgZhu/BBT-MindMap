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
        ANode _currentMarkedNode = null;
        Dictionary<ANode, Grid> _nodeRegistry;
        AMindMap _mindmap;

        

        public MainWindow()
        {
            this._nodeRegistry = new Dictionary<ANode, Grid>();
            this._mindmap = new MindMap();
            this._mindmap.removeNodeEvent += removeNodeEventHandler;
            InitializeComponent();
        }

        public void removeNodeEventHandler(object sender, ANode node)
        {
            this.MindMapCanvas.Children.Remove(this._nodeRegistry[node]);
            this._nodeRegistry.Remove(node);
        }

        private void handleNodeChanges(object sender, ANode node)
        {
            Tuple<Line, Grid> nodeElement = AMindMap.getDisplay(node);
            if (this._nodeRegistry.ContainsKey(node))
            {
                if (this.MindMapCanvas.Children.Contains(this._nodeRegistry[node]))
                    this.MindMapCanvas.Children.Remove(this._nodeRegistry[node]);
            }
            this._nodeRegistry[node] = nodeElement.Item2;

            if (nodeElement.Item1 != null)
            {
                Canvas.SetZIndex(nodeElement.Item1, 0);
                this.MindMapCanvas.Children.Add(nodeElement.Item1);
            }

            Canvas.SetLeft(nodeElement.Item2, node.getRectangle().Left);
            Canvas.SetTop(nodeElement.Item2, node.getRectangle().Top);
            Canvas.SetZIndex(nodeElement.Item2, 1);
            nodeElement.Item2.MouseLeftButtonDown += Node_MouseLeftButtonDown;
            this.MindMapCanvas.Children.Add(nodeElement.Item2);
            if (this._currentMarkedNode == null)
                this._currentMarkedNode = node;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MindMapCanvas.Clip = null;

            ANode node = new Node();

            
            node.changeNodeEvent += this.handleNodeChanges;
            
            node.beginUpdate();
            try
            {
                node.setForm(new Rechteck());

                Point rectStartPoint = new Point(MindMapCanvas.ActualWidth / 2 - 50, this.MindMapCanvas.ActualHeight / 2 - 25);
                Point rectEndPoint = new Point(MindMapCanvas.ActualWidth / 2 + 50, this.MindMapCanvas.ActualHeight / 2 + 25);

                node.setRectangle(new Rect(rectStartPoint, rectEndPoint));
                
                AStyle nodeStyle = new Style();
                nodeStyle.setColor(Tuple.Create((this.colorRect.Fill as SolidColorBrush).Color, (bool)this.fillCheckBox.IsChecked));
                node.setStyle(nodeStyle);

                node.setParent(null);
                this._mindmap.setMainNode(node);

                node.setText(this.nodeText.Text);
            }
            finally
            {
                node.endUpdate();
                _currentMarkedNode = node;
            }

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
        /// <summary>
        /// Wenn der Wert vom slider sich ändert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.FontSizeLabel.Content = "Schriftgrösse: " + System.Convert.ToInt32(this.FontSlider.Value).ToString() + "px";
        }
        /// <summary>
        /// Programm Beenden
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            MyDialog.AllowFullOpen = true;
            MyDialog.ShowDialog();
            //Color
            Color myColor = System.Windows.Media.Color.FromArgb(1, MyDialog.Color.R, MyDialog.Color.G, MyDialog.Color.B);
            //Brush
            SolidColorBrush myBrush = new SolidColorBrush();
            myBrush.Color = System.Windows.Media.Color.FromArgb(255, MyDialog.Color.R, MyDialog.Color.G, MyDialog.Color.B);
            //colorRect Farbe ändern
            colorRect.Fill = myBrush;

            //Ausgewählten Knoten updaten
            _currentMarkedNode.changeNodeEvent += this.handleNodeChanges;
            _currentMarkedNode.beginUpdate();
            try
            {
                IStyle nodeStyle = _currentMarkedNode.getStyle();
                nodeStyle.setColor(Tuple.Create((this.colorRect.Fill as SolidColorBrush).Color, (bool)this.fillCheckBox.IsChecked));
                _currentMarkedNode.setStyle((AStyle)nodeStyle);

            }
            finally
            {
                _currentMarkedNode.endUpdate();
            }


        }

        /// <summary>
        /// Hintergrundfarbe vom Canvas wird geändert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeBackgroundColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = false;
            MyDialog.ShowDialog();
            SolidColorBrush myBrush = new SolidColorBrush();
            myBrush.Color = System.Windows.Media.Color.FromArgb(255, MyDialog.Color.R, MyDialog.Color.G, MyDialog.Color.B);

            this.MindMapCanvas.Background = myBrush;

        }

        private void Node_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid)
            {
                Grid tempGrid = sender as Grid;
                foreach (KeyValuePair<ANode, Grid> tempKeyValuePair in this._nodeRegistry)
                {
                    if (tempKeyValuePair.Value == tempGrid)
                    {
                        this._currentMarkedNode = tempKeyValuePair.Key;

                        this.nodeText.Text = this._currentMarkedNode.getText();

                        var converter = new System.Windows.Media.BrushConverter();
                        var brush = (Brush)converter.ConvertFromString(this._currentMarkedNode.getStyle().getColor().Item1.ToString());
                       

                        this.colorRect.Fill = brush;
                        this.fillCheckBox.IsChecked = this._currentMarkedNode.getStyle().getColor().Item2;
                        /*switch (this._currentMarkedNode.getForm())
                        {
                        
                        }
                        
                        this.ChooseForm.SelectedIndex = this._currentMarkedNode.getForm();


                        */
                        break;
                    }
                }
            }
        }
        private bool dragging = false;

        private void MindMapCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is IInputElement)
            {
                if (dragging)
                {
                    Rect tempRect = this._currentMarkedNode.getRectangle();
                    tempRect.X = e.GetPosition((IInputElement)sender).X;
                    tempRect.Y = e.GetPosition((IInputElement)sender).Y;
                    this._currentMarkedNode.setRectangle(tempRect);
                }
            }
        }

        private void fillCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //Ausgewählten Knoten updaten
            _currentMarkedNode.changeNodeEvent += this.handleNodeChanges;
            _currentMarkedNode.beginUpdate();
            try
            {
                IStyle nodeStyle = _currentMarkedNode.getStyle();
                nodeStyle.setColor(Tuple.Create((this.colorRect.Fill as SolidColorBrush).Color, (bool)this.fillCheckBox.IsChecked));
                _currentMarkedNode.setStyle((AStyle)nodeStyle);

            }
            finally
            {
                _currentMarkedNode.endUpdate();
            }
        }

        private void fillCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //Ausgewählten Knoten updaten
            _currentMarkedNode.changeNodeEvent += this.handleNodeChanges;
            _currentMarkedNode.beginUpdate();
            try
            {
                IStyle nodeStyle = _currentMarkedNode.getStyle();
                nodeStyle.setColor(Tuple.Create((this.colorRect.Fill as SolidColorBrush).Color, (bool)this.fillCheckBox.IsChecked));
                _currentMarkedNode.setStyle((AStyle)nodeStyle);

            }
            finally
            {
                _currentMarkedNode.endUpdate();
            }
        }

        private void nodeText_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Ausgewählten Knoten updaten
            if (_currentMarkedNode != null)
            {
                _currentMarkedNode.changeNodeEvent += this.handleNodeChanges;
                _currentMarkedNode.beginUpdate();
                try
                {

                    _currentMarkedNode.setText(this.nodeText.Text);

                }
                finally
                {
                    _currentMarkedNode.endUpdate();
                }
            }
        }

        private void addNode_Click(object sender, RoutedEventArgs e)
        {
            ANode node = new Node();

     
            node.changeNodeEvent += this.handleNodeChanges;

            node.beginUpdate();
            try
            {
                switch (this.ChooseForm.SelectedIndex)
                {
                    case 0:
                        node.setForm(new Rechteck());
                        break;
                    case 1:
                        node.setForm(new Ellipse());
                        break;
                    default:
                        break;
                }

                node.setRectangle(new Rect(new Point(_currentMarkedNode.getRectangle().TopLeft.X+150, _currentMarkedNode.getRectangle().TopLeft.Y), new Point(_currentMarkedNode.getRectangle().BottomRight.X+150, _currentMarkedNode.getRectangle().BottomRight.Y)));

                AStyle nodeStyle = new Style();
                nodeStyle.setColor(Tuple.Create((this.colorRect.Fill as SolidColorBrush).Color, (bool)this.fillCheckBox.IsChecked));
                node.setStyle(nodeStyle);

                node.setParent(_currentMarkedNode); 
                this._mindmap.addNode(node);

                node.setText(this.nodeText.Text);
            }
            finally
            {
                node.endUpdate();
                _currentMarkedNode = node;
            }
        }

        private void ChooseForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_currentMarkedNode != null)
            {
                _currentMarkedNode.changeNodeEvent += this.handleNodeChanges;
                _currentMarkedNode.beginUpdate();
                try
                {
                    switch (this.ChooseForm.SelectedIndex)
                    {
                        case 0:
                            _currentMarkedNode.setForm(new Rechteck());
                            break;
                        case 1:
                            _currentMarkedNode.setForm(new Ellipse());
                            break;
                        default:
                            break;
                    }
                }
                finally
                {
                    _currentMarkedNode.endUpdate();
                }
            }
        }

        private void removeNOde_Click(object sender, RoutedEventArgs e)
        {
            this._mindmap.removeNode(_currentMarkedNode);
        }

    }
}
