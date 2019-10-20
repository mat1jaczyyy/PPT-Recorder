using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Recorder {
    public partial class UI {
        public static UI Window { get; private set; }

        static bool FreezeEvents = true;

        static string InactiveString, ActiveString, StartString, StopString;

        public UI() {
            InitializeComponent();

            Window = this;

            FreezeEvents = false;

            PlayerInfo.IsChecked = Preferences.PlayerInfo;
            ReplaySelect.IsChecked = Preferences.ReplaySelect;
            EndingMenu.IsChecked = Preferences.EndingMenu;

            Version.Text = $"PPT-Recorder-{Assembly.GetExecutingAssembly().GetName().Version.Minor}";

            switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName) {
                case "ko":
                    InactiveString = "비활성화";
                    ActiveString = "활성화";
                    JobsHeader.Text = "";
                    PlayerInfo.Content = "";
                    ReplaySelect.Content = "";
                    EndingMenu.Content = "";
                    StartString = "";
                    StopString = "";
                    Gamepad.Content = "게임패드 연결";
                    break;

                case "ja":
                    InactiveString = "停止";
                    ActiveString = "動作中";
                    JobsHeader.Text = "";
                    PlayerInfo.Content = "";
                    ReplaySelect.Content = "";
                    EndingMenu.Content = "";
                    StartString = "";
                    StopString = "";
                    Gamepad.Content = "コントローラー接続中";
                    break;

                default:
                    InactiveString = "Inactive";
                    ActiveString = "Active";
                    JobsHeader.Text = "Recording Jobs";
                    PlayerInfo.Content = "Display Player Information";
                    ReplaySelect.Content = "Include Replay Select Screen";
                    EndingMenu.Content = "Include Ending Menu";
                    StartString = "Start";
                    StopString = "Stop";
                    Gamepad.Content = "Gamepad Connected";
                    break;
            }

            UpdateActive();
            Bot.Start();
        }

        bool _active = false;
        public bool Active {
            get => _active;
            set {
                if (_active != value) {
                    _active = value;

                    Dispatcher.InvokeAsync(() => UpdateActive());
                }
            }
        }

        void UpdateActive() {
            State.Text = Active? ActiveString : InactiveString;
            Trigger.Content = Active? StopString : StartString;
            Gamepad.IsEnabled = !Active;
        }

        public void RefreshJobs() {
            bool invalid = false;

            foreach (JobEntry entry in JobEntries.Children.OfType<JobEntry>()) {
                entry.Revalidate();

                invalid |= !entry.IsValid;
            }

            Trigger.IsEnabled = !invalid;
        }

        void PlayerInfoChanged(object sender, RoutedEventArgs e) {
            if (!FreezeEvents) Preferences.PlayerInfo = PlayerInfo.IsChecked == true;
        }

        void ReplaySelectChanged(object sender, RoutedEventArgs e) {
            if (!FreezeEvents) Preferences.ReplaySelect = ReplaySelect.IsChecked == true;
        }

        void EndingMenuChanged(object sender, RoutedEventArgs e) {
            if (!FreezeEvents) Preferences.EndingMenu = EndingMenu.IsChecked == true;
        }

        void TriggerClicked(object sender, RoutedEventArgs e) {

        }

        void GamepadChanged(object sender, RoutedEventArgs e) {
            if (!FreezeEvents) Bot.SetGamepad(Gamepad.IsChecked == true);
        }
    }
}
