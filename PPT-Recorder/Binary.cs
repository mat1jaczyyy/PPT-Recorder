using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Recorder {
    public static class Binary {
        static readonly int Version = 0;

        static byte[] CreateHeader() => Encoding.ASCII.GetBytes("PTRC").Concat(BitConverter.GetBytes(Version)).ToArray();

        static int DecodeHeader(BinaryReader reader) {
            if (!reader.ReadChars(4).SequenceEqual(new char[] { 'P', 'T', 'R', 'C' })) throw new InvalidDataException();
            int version = reader.ReadInt32();

            if (version > Version) throw new InvalidDataException();

            return version;
        }

        public static MemoryStream EncodePreferences() {
            MemoryStream output = new MemoryStream();

            using (BinaryWriter writer = new BinaryWriter(output)) {
                writer.Write(CreateHeader());

                writer.Write(Preferences.PlayerInfo);
                writer.Write(Preferences.ReplaySelect);
                writer.Write(Preferences.EndingMenu);
            }

            return output;
        }

        public static void DecodePreferences(FileStream input) {
            using (BinaryReader reader = new BinaryReader(input)) {
                int version = DecodeHeader(reader);

                Preferences.PlayerInfo = reader.ReadBoolean();
                Preferences.ReplaySelect = reader.ReadBoolean();
                Preferences.EndingMenu = reader.ReadBoolean();
            }
        }
    }
}
