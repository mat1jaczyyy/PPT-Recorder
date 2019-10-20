using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Recorder {
    public partial class AddButton: UserControl {
        bool mouseHeld = false;
        public AddButton() {
            InitializeComponent();

            Leave(this, null);
        }

        void Enter(object sender, MouseEventArgs e) {
            Path.Stroke = new SolidColorBrush(
                mouseHeld? Color.FromRgb(100, 100, 100) : Color.FromRgb(120, 120, 120)
            );
        }

        void Leave(object sender, MouseEventArgs e) {
            Path.Stroke = new SolidColorBrush(
                Color.FromRgb(80, 80, 80)
            );
            mouseHeld = false;
        }

        void Down(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                mouseHeld = true;

                Path.Stroke = new SolidColorBrush(
                    Color.FromRgb(100, 100, 100)
                );
            }
        }

        void Up(object sender, MouseButtonEventArgs e) {
            if (mouseHeld && (e.ChangedButton == MouseButton.Left)) {
                mouseHeld = false;

                Enter(sender, null);

                // UI Add
            }
        }
    }
}
