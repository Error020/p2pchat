using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public static void method1(Label label1)
        {
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        { 

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Thread test = new Thread(new ThreadStart(TestThread));
            test.Start();
        }
        public delegate void UpdateTextCallback(string message);

        private void TestThread()
        {
            int i = 1;
            while(i == 1)
            {
                Thread.Sleep(1000);
                richTextBox1.Dispatcher.Invoke(
                    new UpdateTextCallback(this.UpdateText),
                    new object[] { i.ToString() }
                );
            }
        }
        private void UpdateText(string message)
        {
            richTextBox1.AppendText(message + "\n");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Thread test = new Thread(new ThreadStart(TestThread));
            test.Start();
        }

        private void richTextBox1_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            Thread test = new Thread(new ThreadStart(TestThread));
            test.Start();
        }
    }
}

