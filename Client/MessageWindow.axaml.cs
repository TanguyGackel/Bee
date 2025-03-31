using Avalonia.Controls;

namespace Client
{
    public partial class MessageWindow : Window
    {
        public MessageWindow(string message)
        {
            InitializeComponent();
            MessageTextBlock.Text = message;
        }

        private void OkButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            this.Close();
        }
    }
}