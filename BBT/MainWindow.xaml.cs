using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Xml.Linq;

namespace BBT
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ANode _currentMarkedNode = null;
        Dictionary<ANode, Tuple<Line, Grid>> _nodeRegistry;
        AMindMap _mindmap;

        

        public MainWindow()
        {
            this._nodeRegistry = new Dictionary<ANode, Tuple<Line, Grid>>();
            this._mindmap = new MindMap();
            this._mindmap.removeNodeEvent += removeNodeEventHandler;
            this._mindmap.changeSizeEvent += _mindmap_changeSizeEvent;
            this._mindmap.addNodeEvent += _mindmap_addNodeEvent;
            InitializeComponent();
            this.changeActiveNodeEvent += activeNodeChangedHandler;
        }

        void _mindmap_addNodeEvent(object sender, ANode node)
        {
            node.changeNodeEvent += this.handleNodeChanges;
            node.invalidate();
        }

        void _mindmap_changeSizeEvent(object sender, Size newSize)
        {
            this.MindMapCanvas.Width = newSize.Width;
            this.MindMapCanvas.Height = newSize.Height;
        }

        public void removeNodeEventHandler(object sender, ANode node)
        {
            if (this._nodeRegistry.ContainsKey(node))
            {
                if (this.MindMapCanvas.Children.Contains(this._nodeRegistry[node].Item2))
                    this.MindMapCanvas.Children.Remove(this._nodeRegistry[node].Item2);

                if (this.MindMapCanvas.Children.Contains(this._nodeRegistry[node].Item1))
                    this.MindMapCanvas.Children.Remove(this._nodeRegistry[node].Item1);
            }
            this._nodeRegistry.Remove(node);
        }

        private void handleNodeChanges(object sender, ANode node)
        {
            Tuple<Line, Grid> nodeElement = AMindMap.getDisplay(node);
            if (this._nodeRegistry.ContainsKey(node))
            {
                if (this.MindMapCanvas.Children.Contains(this._nodeRegistry[node].Item2))
                        this.MindMapCanvas.Children.Remove(this._nodeRegistry[node].Item2);

                if (this.MindMapCanvas.Children.Contains(this._nodeRegistry[node].Item1))
                    this.MindMapCanvas.Children.Remove(this._nodeRegistry[node].Item1);
            }
            this._nodeRegistry[node] = nodeElement;

            if (nodeElement.Item1 != null)
            {
                Canvas.SetZIndex(nodeElement.Item1, 0);
                this.MindMapCanvas.Children.Add(nodeElement.Item1);
            }

            Canvas.SetLeft(nodeElement.Item2, node.getRectangle().Left);
            Canvas.SetTop(nodeElement.Item2, node.getRectangle().Top);
            Canvas.SetZIndex(nodeElement.Item2, 1);
            nodeElement.Item2.MouseLeftButtonDown += Node_MouseLeftButton;
            nodeElement.Item2.MouseLeftButtonUp += Node_MouseLeftButton;
            this.MindMapCanvas.Children.Add(nodeElement.Item2);
            
            if (this._currentMarkedNode == null)
                changeActiveNode(this, node);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MindMapCanvas.Clip = null;

            ANode node = new Node();
            
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
                node.getStyle().setActivated(true);

                node.setText(this.nodeText.Text);
                
            }
            finally
            {
                node.endUpdate();
                
                _currentMarkedNode = node;
                //_currentMarkedNode.fromXML(_currentMarkedNode.toXML());
                //Console.Write(_mindmap.toXML().ToString());
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
        /// <summary>
        /// Toolbox hinzufügen Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Toolbox entfernen Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            if(this._currentMarkedNode != null)
                this._currentMarkedNode.getStyle().setFontsize((int)this.FontSlider.Value);
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
        /// <summary>
        /// Wenn Taste gedrückt wird
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyPressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape && this.WindowStyle == System.Windows.WindowStyle.None)
            {
                ExitFullscreen();
            }
        }
        /// <summary>
        /// Zeichenfarben mit ColorDialog wählen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;
            MyDialog.ShowDialog();
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
        /// Hintergrundfarbe vom Canvas mit ColorDialog wählen
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
            //this.Background = myBrush;
        }
        Point click;
        Rect grundRechteck;
        private void Node_MouseLeftButton(object sender, MouseButtonEventArgs e)
        {
            if ((sender is Grid))
            {
                if ((e.GetPosition(MindMapCanvas).X > (this._currentMarkedNode.getRectangle().X + this._currentMarkedNode.getRectangle().Width - 10)) && (e.GetPosition(MindMapCanvas).X < (this._currentMarkedNode.getRectangle().X + this._currentMarkedNode.getRectangle().Width)) && (e.GetPosition(MindMapCanvas).Y > (this._currentMarkedNode.getRectangle().Y + this._currentMarkedNode.getRectangle().Height - 10)) && (e.GetPosition(MindMapCanvas).Y < (this._currentMarkedNode.getRectangle().Y + this._currentMarkedNode.getRectangle().Height)))
                {
                    Mouse.SetCursor(System.Windows.Input.Cursors.Cross);
                }
            }
            if ((sender is Grid) && (e.LeftButton == MouseButtonState.Pressed))
            {
                if ((e.GetPosition(MindMapCanvas).X > (this._currentMarkedNode.getRectangle().X + this._currentMarkedNode.getRectangle().Width - 10)) && (e.GetPosition(MindMapCanvas).X < (this._currentMarkedNode.getRectangle().X + this._currentMarkedNode.getRectangle().Width)) && (e.GetPosition(MindMapCanvas).Y > (this._currentMarkedNode.getRectangle().Y + this._currentMarkedNode.getRectangle().Height - 10)) && (e.GetPosition(MindMapCanvas).Y < (this._currentMarkedNode.getRectangle().Y + this._currentMarkedNode.getRectangle().Height)))
                {
                    size = true;
                    click = e.GetPosition(MindMapCanvas);
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.SizeAll;
                    grundRechteck = this._currentMarkedNode.getRectangle();
                }
                else
                    this.dragging = true;
                Grid tempGrid = sender as Grid;
                this.dragStart = e.GetPosition(tempGrid);
                
                foreach (KeyValuePair<ANode, Tuple<Line, Grid>> tempKeyValuePair in this._nodeRegistry)
                {
                    if (tempKeyValuePair.Value.Item2 == tempGrid)
                    {
                        this.changeActiveNode(this, tempKeyValuePair.Key);
                        break;
                    }
                }
            }
            else if ((sender is Grid) && (e.LeftButton == MouseButtonState.Released))
            {
                this.dragging = false;
                this.size = false;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
            if (e.LeftButton == MouseButtonState.Released)
            {
                this.dragging = false;
                this.size = false;
            }
        }
        private bool dragging = false;
        private Point dragStart;
        bool size = false;
        private void MindMapCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is IInputElement)
            {
                Rect tempRect;
                if (dragging)
                {
                    tempRect = this._currentMarkedNode.getRectangle();
                    tempRect.X = e.GetPosition((IInputElement)sender).X - this.dragStart.X;
                    tempRect.Y = e.GetPosition((IInputElement)sender).Y - this.dragStart.Y;
                    this._currentMarkedNode.setRectangle(tempRect);
                    
                }
                else if (size)
                {           
                    tempRect = this._currentMarkedNode.getRectangle();

                    if ((e.GetPosition((IInputElement)sender).X - this.click.X + grundRechteck.Width) >= 29)
                        tempRect.Width = e.GetPosition((IInputElement)sender).X - this.click.X + grundRechteck.Width;
                    else
                    {
                        
                        tempRect.Width = 30;
                    }
                    if ((e.GetPosition((IInputElement)sender).Y - this.click.Y + grundRechteck.Height) >= 29)
                        tempRect.Height = e.GetPosition((IInputElement)sender).Y - this.click.Y + grundRechteck.Height;
                    else
                        tempRect.Height = 30;
                    this._currentMarkedNode.setRectangle(tempRect);
                }
                
            }
        }

        /// <summary>
        /// FillCheckbox wird gecheckt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// FillCheckbox wird ungecheckt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Text im Werkzeugkasten wird geändert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Neuen Knoten hinzufügen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addNode_Click(object sender, RoutedEventArgs e)
        {
            ANode node = new Node();
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
                //Punkte bestimmen
                //Links
                Rect rLeft = new Rect(new Point(_currentMarkedNode.getRectangle().TopLeft.X + 150, _currentMarkedNode.getRectangle().TopLeft.Y), new Point(_currentMarkedNode.getRectangle().BottomRight.X + 150, _currentMarkedNode.getRectangle().BottomRight.Y));
                //Rechts
                Rect rRight = new Rect(new Point(_currentMarkedNode.getRectangle().TopLeft.X - 150, _currentMarkedNode.getRectangle().TopLeft.Y), new Point(_currentMarkedNode.getRectangle().BottomRight.X - 150, _currentMarkedNode.getRectangle().BottomRight.Y));
                //Oben
                Rect rTop = new Rect(new Point(_currentMarkedNode.getRectangle().TopLeft.X, _currentMarkedNode.getRectangle().TopLeft.Y-150), new Point(_currentMarkedNode.getRectangle().BottomRight.X, _currentMarkedNode.getRectangle().BottomRight.Y-150));
                //Unten
                Rect rBot = new Rect(new Point(_currentMarkedNode.getRectangle().TopLeft.X, _currentMarkedNode.getRectangle().TopLeft.Y + 150), new Point(_currentMarkedNode.getRectangle().BottomRight.X, _currentMarkedNode.getRectangle().BottomRight.Y + 150));
                Rect r = new Rect();
                Random random = new Random();
                switch (random.Next(0, 4))
                {
                    case 0:
                        r = rLeft;
                        break;
                    case 1:
                        r = rRight;
                        break;
                    case 2:
                        r = rTop;
                        break;
                    case 3:
                        r = rBot;
                        break;
                }
                node.setRectangle(r);

                AStyle nodeStyle = new Style();
                nodeStyle.setColor(Tuple.Create((this.colorRect.Fill as SolidColorBrush).Color, (bool)this.fillCheckBox.IsChecked));
                node.setStyle(nodeStyle);

                node.setParent(_currentMarkedNode); 
                this._mindmap.addNode(node);

                node.setText("Neuer Knoten");
            }
            finally
            {
                node.endUpdate();
                this.changeActiveNode(this, node);
            }
        }

        protected void changeActiveNode(object sender, ANode node)
        {
            if ((this.changeActiveNodeEvent != null))
                changeActiveNodeEvent(this, node);
        }

        public delegate void changedActiveNodeEventHandler(object sender, ANode node);

        public event changedActiveNodeEventHandler changeActiveNodeEvent;
        /// <summary>
        /// Wenn sich der aktive Knoten ändert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="node"></param>
        private void activeNodeChangedHandler(object sender, ANode node)
        {
            if (this._currentMarkedNode != node)
            {
                if (this._currentMarkedNode != null)
                    this._currentMarkedNode.getStyle().setActivated(false);
                this._currentMarkedNode = node;

                this.removeNOde.IsEnabled = (this._currentMarkedNode.getParent() != null);

                this._currentMarkedNode.getStyle().setActivated(true);

                this.nodeText.Text = this._currentMarkedNode.getText();
                
                this.FontSlider.Value = this._currentMarkedNode.getStyle().getFontsize();

                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString(this._currentMarkedNode.getStyle().getColor().Item1.ToString());


                this.colorRect.Fill = brush;
                this.fillCheckBox.IsChecked = this._currentMarkedNode.getStyle().getColor().Item2;

                if (this._currentMarkedNode.getForm().ToString() == "Rechteck")
                {
                    this.ChooseForm.SelectedIndex = 0;
                }
                else
                {
                    this.ChooseForm.SelectedIndex = 1;
                }

                this.IconImage.Source = _currentMarkedNode.getStyle().getIcon();


                
            }
        }
        /// <summary>
        /// Form wird in Toolbox geändert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            ANode an = _currentMarkedNode;
            if (this._currentMarkedNode.getParent() != null)
                changeActiveNode(this, _currentMarkedNode.getParent());
            this._mindmap.removeNode(an, true);
            
        }


        /// <summary>
        /// Add Icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".png";
            dlg.Filter = "Pictures (.png)|*.png";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;

                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(filename);
                logo.DecodePixelWidth = 14;
                logo.DecodePixelHeight = 14;
                logo.EndInit();
                this.IconImage.Source = logo;
                this._currentMarkedNode.getStyle().setICon(logo);
                this._currentMarkedNode.endUpdate();

                StackPanel sp = new StackPanel();
                sp.Orientation = System.Windows.Controls.Orientation.Horizontal;

                Image img = new Image();
                img.Width = 16;
                img.Height = 16;
                img.Source = logo;
                sp.Children.Add(img);

                System.Windows.Controls.Label label = new System.Windows.Controls.Label();
                label.Content = filename;
                sp.Children.Add(label);

               // label.MouseDown += ;

                this.IconComboBox.Items.Add(sp);
            }

        }

        private void PickIcon_MouseDown(object sender, MouseButtonEventArgs e, String path)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();

            String uriString = sender.GetType().FullName;

            //logo.UriSource = new Uri();
            logo.EndInit();
            this.IconImage.Source = logo;
            this._currentMarkedNode.getStyle().setICon(logo);
            this._currentMarkedNode.endUpdate();
        }

        private void MindMapCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this._mindmap.setDrawSize(e.NewSize);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Mindmap"; // Default file name
            dlg.DefaultExt = ".png"; // Default file extension

             dlg.Filter = "Picture documents (.png)|*.png|Xml Document(.xml)|*.xml"; // Filter files by extension


            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();
             //Process save file dialog box results

            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                Uri path = new Uri(filename);
                if (dlg.FileName.Contains(".xml"))
                    ExportXml(path, _mindmap.toXML().ToString());
                else
                    ExportToPng(path, this.MindMapCanvas);
            }
            

            
            
        }

        public void ExportXml(Uri path, String xml)
        {
            FileStream outStream = new FileStream(path.LocalPath, FileMode.Create);
            byte[] penis = System.Text.Encoding.UTF8.GetBytes(xml);
            outStream.Write(penis,0, penis.Length);
            outStream.Close();
            

                
        }

        /// <summary>
        /// Export Canvas to PNG
        /// </summary>
        /// <param name="path">wo gespeichert wird</param>
        /// <param name="surface">canvas welches gespeichert werden soll</param>
        public void ExportToPng(Uri path, Canvas surface)
        {
            if (path == null) return;

            // Save current canvas transform
            Transform transform = surface.LayoutTransform;
            // reset current transform (in case it is scaled or rotated)
            surface.LayoutTransform = null;

            // Get the size of canvas
            Size size = new Size(surface.ActualWidth, surface.ActualHeight);
            // Measure and arrange the surface
            // VERY IMPORTANT
            surface.Measure(size);
            surface.Arrange(new Rect(size));

            // Create a render bitmap and push the surface to it
            RenderTargetBitmap renderBitmap =
              new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                96d,
                96d,
                PixelFormats.Pbgra32);
            renderBitmap.Render(surface);

            // Create a file stream for saving image
            using (FileStream outStream = new FileStream(path.LocalPath, FileMode.Create))
            {
                // Use png encoder for our data
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                // push the rendered bitmap to it
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                // save the data to the stream
                encoder.Save(outStream);
            }

            // Restore previously saved layout
            surface.LayoutTransform = transform;
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Mindmap"; // Default file name
                dlg.DefaultExt = ".xml"; // Default file extension

             dlg.Filter = "Xml Document(.xml)|*.xml"; // Filter files by extension


            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();
             //Process save file dialog box results

            if (result == true)
            {
                this._currentMarkedNode = null;
                this._nodeRegistry.Clear();
                this.MindMapCanvas.Children.Clear();
                FileStream input = new FileStream(dlg.FileName, FileMode.Open);
                byte[] xxx = new byte[input.Length];
                input.Read(xxx, 0, (int)input.Length);
                string xml = System.Text.Encoding.UTF8.GetString(xxx);
                input.Close();
                XElement hgsfg = XElement.Parse(xml);
                _mindmap.fromXML(hgsfg);
                this.changeActiveNode(this, _mindmap.getMainNode());
            }
            
        }
    }
}
