using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace Recorder {
    public partial class App {
        [Conditional("DEBUG")]
        void OverrideLocale(string locale) =>
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo(locale);

        void Main(object sender, StartupEventArgs e) => OverrideLocale("en-US");

        void Exiting(object sender, ExitEventArgs e) => Bot.Dispose();
    }
}
