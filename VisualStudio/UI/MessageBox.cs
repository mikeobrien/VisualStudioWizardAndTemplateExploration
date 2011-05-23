namespace VisualStudio.UI
{
    public class MessageBox : IMessageBox
    {
        public void DisplayMessage(string title, string messageFormat, params object[] args)
        {
            System.Windows.MessageBox.Show(messageFormat.FormatWith(args), title);
        }
    }
}
