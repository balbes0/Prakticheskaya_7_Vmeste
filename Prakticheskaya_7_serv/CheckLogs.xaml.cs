using System.Windows;

namespace Prakticheskaya_7_serv
{
    public partial class CheckLogs : Window
    {
        public CheckLogs(List<string> logs)
        {
            InitializeComponent();
            logsListBox.ItemsSource = logs;
        }
    }
}
