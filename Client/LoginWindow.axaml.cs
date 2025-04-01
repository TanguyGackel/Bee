using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Client
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            // Vérifiez que les contrôles ne sont pas null avant d'ajouter des gestionnaires d'événements
            this.Get<Button>("LoginButton").Click += LoginButton_Click;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Vérifiez que les TextBox ne sont pas null
            var usernameTextBox = this.FindControl<TextBox>("UsernameTextBox");
            var passwordTextBox = this.FindControl<TextBox>("PasswordTextBox");

            if (usernameTextBox != null && passwordTextBox != null)
            {
                string username = usernameTextBox.Text;
                string password = passwordTextBox.Text;

                if (username == "admin" && password == "password")
                {
                    ShowMessage("Connexion réussie !", "Succès");

                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    ShowMessage("Nom d'utilisateur ou mot de passe incorrect.", "Erreur");
                }
            }
        }

        private void ShowMessage(string message, string title)
        {
            var messageWindow = new MessageWindow(message)
            {
                Title = title
            };
            messageWindow.ShowDialog(this);
        }
    }
}