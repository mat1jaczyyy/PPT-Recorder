using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Recorder {
    public partial class UI {
        static string InactiveString, ActiveString, StartString, StopString;

        public UI() {
            InitializeComponent();

            Version.Text = $"PPT-Recorder-{Assembly.GetExecutingAssembly().GetName().Version.Minor}";

            switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName) {
                case "ko":
                    InactiveString = "비활성화";
                    ActiveString = "활성화";
                    JobsHeader.Text = "";
                    StartString = "";
                    StopString = "";
                    Gamepad.Content = "게임패드 연결";
                    break;
                    
                case "ja":
                    InactiveString = "停止";
                    ActiveString = "動作中";
                    JobsHeader.Text = "";
                    StartString = "";
                    StopString = "";
                    Gamepad.Content = "コントローラー接続中";
                    break;
                    
                default:
                    InactiveString = "Inactive";
                    ActiveString = "Active";
                    JobsHeader.Text = "Recording Jobs";
                    StartString = "Start";
                    StopString = "Stop";
                    Gamepad.Content = "Gamepad Connected";
                    break;
            }

            UpdateActive();
            Bot.Start(this);
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

        void TriggerClicked(object sender, RoutedEventArgs e) {

        }

        void GamepadChanged(object sender, RoutedEventArgs e) 
            => Bot.SetGamepad(Gamepad.IsChecked == true);
    }
}
