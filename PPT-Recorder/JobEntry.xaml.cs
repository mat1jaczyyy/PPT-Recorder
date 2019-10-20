using System.Windows.Controls;
using System.Windows.Media;

namespace Recorder {
    public partial class JobEntry: TextBox {
        bool _valid = false;
        public bool IsValid {
            get => _valid;
            private set => Foreground = new SolidColorBrush(
                (_valid = value)? Color.FromRgb(220, 220, 220) : Color.FromRgb(220, 0, 0)
            );
        }

        public Job Job { get; private set; }

        public JobEntry() {
            InitializeComponent();
        }

        public void Revalidate() {
            if (IsValid = Job.TryParse(Text, out Job job))
                Job = job;
        }

        void InputChanged(object sender, TextChangedEventArgs e) {
            Revalidate();

            UI.Window.RefreshJobs(false);
        }
    }
}
