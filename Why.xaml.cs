using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SQLGenerator {
    /// <summary>
    /// Interaction logic for Why.xaml
    /// </summary>
    public partial class Why : Window {
        public Why() {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e) {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show("Are you completely sure?", "Turn back",MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) {
                MessageBoxResult result1 = MessageBox.Show("Are you 100% absolutely, completely fucking sure-sure?!", "TURN THE FUCKING BACK!!!", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result1 == MessageBoxResult.Yes) {
                    MessageBox.Show("You were winlocked.");
                    Thread.Sleep(1000);
                    Process.Start("WL.exe");
                    Application.Current.Shutdown();
                }
                
            }
        }
    }
}
