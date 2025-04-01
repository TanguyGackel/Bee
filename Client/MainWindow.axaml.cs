using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace Client
{
    public partial class MainWindow : Window
    {
        private Button _lastSelectedButton;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Réinitialiser le style du bouton précédemment sélectionné
            if (_lastSelectedButton != null)
            {
                _lastSelectedButton.Background = Brushes.LightGray;
            }

            // Mettre à jour le bouton actuellement sélectionné
            var button = sender as Button;
            button.Background = Brushes.DarkGray;
            _lastSelectedButton = button;

            // Masquer tous les contenus
            FreezBeeContent.IsVisible = false;
            IngredientsContent.IsVisible = false;
            RecettesContent.IsVisible = false;

            // Afficher le contenu correspondant au bouton cliqué
            if (sender == FreezBeeButton)
            {
                FreezBeeContent.IsVisible = true;
            }
            else if (sender == IngredientsButton)
            {
                IngredientsContent.IsVisible = true;
            }
            else if (sender == RecettesButton)
            {
                RecettesContent.IsVisible = true;
            }
        }
    }
}