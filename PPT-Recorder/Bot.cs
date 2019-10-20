using System.Threading;
using System.Threading.Tasks;

using ScpDriverInterface;

namespace Recorder {
    public static class Bot {
        static int gamepadIndex = 4;
        static ScpBus scp = new ScpBus();
        static X360Controller gamepad = new X360Controller();

        public static void SetGamepad(bool state) {
            scp.Unplug(gamepadIndex);
            scp = new ScpBus();
            gamepad = new X360Controller();

            if (state) scp.PlugIn(gamepadIndex);
        }

        static int globalFrames;

        static X360Buttons previousInputs = X360Buttons.None;

        static void applyInputs() {
            // Logic goes here

            gamepad.Buttons &= ~previousInputs;
            previousInputs = gamepad.Buttons;

            scp.Report(gamepadIndex, gamepad.GetReport());
        }

        static void updateUI() {
            if (UI.Window == null) return;

            UI.Window.Active = false;
            UI.Window.RefreshJobs(true);
        }

        static void Loop() {
            while (!Disposing) {
                if (GameHelper.CheckProcess()) {
                    GameHelper.TrustProcess = true;

                    int prev = globalFrames;
                    globalFrames = GameHelper.getMenuFrameCount();

                    if (globalFrames > prev)
                        applyInputs();

                    GameHelper.TrustProcess = false;
                }

                updateUI();

                Thread.Sleep(30); // We don't need high precision, might as well not rape CPU
            }

            Disposed = true;
        }

        public static bool Started { get; private set; } = false;

        public static void Start() {
            if (Started) return;

            scp.UnplugAll();

            scp = new ScpBus();
            scp.PlugIn(gamepadIndex);

            Started = true;

            Task.Run(() => Loop());
        }

        static bool Disposing = false;
        public static bool Disposed { get; private set; } = false;

        public static void Dispose() {
            Disposing = true;

            while (!Disposed && Started);

            scp.UnplugAll();
        }
    }
}
