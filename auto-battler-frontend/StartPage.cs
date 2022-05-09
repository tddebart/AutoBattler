using System.Diagnostics;
using System.Windows.Forms;
using auto_battler_frontend.Properties;

namespace auto_battler_frontend
{
    public partial class StartPage : Form
    {
        public string username;
        
        public StartPage()
        {
            InitializeComponent();
            
            if(Debugger.IsAttached) 
                Settings.Default.Reset();
            
            // See if username stored in settings
            if (Properties.Settings.Default.Username != "")
            {
                username = Settings.Default.Username;
                // usernameInput.Text = username;
                var mainPage = new MainPage(username);
                this.Hide();
                mainPage.ShowDialog();
                this.Close();
            }
        }
        
        public void usernameInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                username = usernameInput.Text;
                Settings.Default.Username = username;
                Settings.Default.Save();
                var mainPage = new MainPage(username);
                mainPage.Location = this.Location;
                mainPage.StartPosition = FormStartPosition.Manual;
                this.Hide();
                mainPage.ShowDialog();
                this.Close();
            }
        }
    }
}