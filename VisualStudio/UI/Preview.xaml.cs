using System.Windows;

namespace VisualStudio.UI
{
    public partial class Preview : Window
    {
        public Preview()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public void Display(Window owner, string content)
        {
            Owner = owner;
            Content.Text = content;
            ShowDialog();
        }
    }
}
