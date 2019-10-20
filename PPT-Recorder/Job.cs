using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Recorder {
    public class Job {
        List<int> _replays = new List<int>();
        ReadOnlyCollection<int> Replays => _replays.AsReadOnly();

        static bool Inbounds(int index) => 1 <= index && index <= GameHelper.getReplayCount();

        public bool Add(int index) {
            if (!Inbounds(index))
                return false;

            _replays.Add(index - 1);
            return true;
        }

        public static bool TryParse(string job, out Job result) {
            result = new Job();

            try {
                foreach (string single in job.Replace(" ", "").Split(','))
                    switch (single.Count(i => i == '-')) {
                        case 0:
                            if (!int.TryParse(single, out int x) || !result.Add(x)) return false;
                            break;

                        case 1:
                            string[] interval = single.Split('-');
                            if (!int.TryParse(interval[0], out int start) || !Inbounds(start)) return false;
                            if (!int.TryParse(interval[1], out int end) || !Inbounds(end)) return false;
                            int step = (start > end)? -1 : 1;

                            for (int i = start; i != end; i += step)
                                result.Add(i);

                            break;

                        default:
                            throw new ArgumentException();
                    }

            } catch {
                return false;
            }

            return true;
        }
    }
}
