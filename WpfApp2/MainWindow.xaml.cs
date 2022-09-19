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
                // just to display that the client is updating content
                label1.Dispatcher.Invoke(() => {
                    if (label1.Content.ToString() == "updating")
                    {
                        label1.Content = "...";
                    }
                    else
                    {
                        label1.Content = "updating";
                    }
                });
                
                // update richtextbox and stuff

                Thread.Sleep(2000);
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }
    }
}

