using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
    }
}
