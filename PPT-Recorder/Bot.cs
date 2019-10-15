using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using ScpDriverInterface;

namespace Recorder {
    public static class Bot {
        static UI Window = null;

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
            if (Window != null)
                Window.Active = false;
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
            }

            Disposed = true;
        }

        public static bool Started { get; private set; } = false;

        public static void Start(UI window) {
            if (Started) return;

            Window = window;

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
