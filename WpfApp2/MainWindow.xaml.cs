using System.Threading;
using System.Windows;
using System.Windows.Controls;

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

        public delegate void UpdateTextCallback(string message);

        public void DoSomething()
        {
            while (true)
            {
                label1.Dispatcher.Invoke(() => {
                    texty();
                });
                Thread.Sleep(2000);
            }
        }

        public void texty()
        {
            if (label1.Content == "updating")
            {
                label1.Content = "...";
            }
            else
            {
                label1.Content = "updating";
            }
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
        }

        private void richTextBox1_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            // Thread test = new Thread(new ThreadStart(TestThread));
            // test.Start();
            new Thread(DoSomething).Start();
        }
    }
}

