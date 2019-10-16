using System;
using System.IO;

namespace Recorder {
    public static class Preferences {
        static readonly string UserPath = Path.Combine(
            Environment.GetEnvironmentVariable("USERPROFILE"),
            ".ppt-recorder"
        );

        static readonly string FilePath = Path.Combine(UserPath, "PPT-Recorder.config");

        static bool Initialized = false;

        static bool _info = false;
        public static bool PlayerInfo {
            get => _info;
            set {
                _info = value;
                Save();
            }
        }

        static bool _select = false;
        public static bool ReplaySelect {
            get => _select;
            set {
                _select = value;
                Save();
            }
        }

        static bool _ending = false;
        public static bool EndingMenu {
            get => _ending;
            set {
                _ending = value;
                Save();
            }
        }

        public static void Save() {
            if (!Initialized) return;

            if (!Directory.Exists(UserPath)) Directory.CreateDirectory(UserPath);

            try {
                File.WriteAllBytes(FilePath, Binary.EncodePreferences().ToArray());
            } catch (IOException) { }
        }

        static Preferences() {
            if (File.Exists(FilePath))
                using (FileStream file = File.Open(FilePath, FileMode.Open, FileAccess.Read))
                    try {
                        Binary.DecodePreferences(file);
                    } catch { }

            Initialized = true;
        }
    }
}
