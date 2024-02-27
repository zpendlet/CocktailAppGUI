using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CocktailAppVS
{
    public partial class InputDialog : Window
    {
        private TextBox inputTextBox;
        public string Result { get; private set; }

        public InputDialog(string prompt)
        {
            InitializeComponent();
            Title = prompt;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            inputTextBox = this.FindControl<TextBox>("InputTextBox");
            inputTextBox.Focus();
        }

        private void OKClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            // Handle OK button click logic
            Result = inputTextBox.Text;
            Close(inputTextBox.Text);
        }

        private void CancelClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            // Handle Cancel button click logic
            Close();
        }
    }
}
