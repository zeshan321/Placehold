using Placehold.Keyboard;
using Placehold.Keyboard.Hook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Placehold
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private readonly KeyboardManager keyboardManager;

        public MainWindow()
        {
            keyboardManager = new KeyboardManager();

            InitializeComponent();
        }

        public void Dispose()
        {
            keyboardManager.Dispose();
        }
    }
}
