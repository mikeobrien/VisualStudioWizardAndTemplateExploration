namespace VisualStudio.UI
{
    public interface IMessageBox
    {
        void DisplayMessage(string title, string messageFormat, params object[] args);
    }
}
