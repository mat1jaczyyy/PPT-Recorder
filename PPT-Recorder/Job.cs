using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Recorder {
    public class Job {
        List<int> _replays = new List<int>();
        ReadOnlyCollection<int> Replays => _replays.AsReadOnly();

        public void Add(int index) {
            if (index < 1 || GameHelper.getReplayCount() < index)
                throw new ArgumentOutOfRangeException();

            _replays.Add(index - 1);
        }

        public static bool TryParse(string job, out Job result) {
            result = new Job();

            try {
                foreach (string single in job.Replace(" ", "").Split(','))
                    switch (single.Count(i => i == '-')) {
                        case 0:
                            result.Add(int.Parse(single));
                            break;

                        case 1:
                            string[] interval = single.Split('-');
                            int start = int.Parse(interval[0]);
                            int end = int.Parse(interval[1]);
                            int step = (start > end)? -1 : 1;

                            for (int i = start; i != end; i += step)
                                result.Add(i);

                            break;

                        default:
                            throw new ArgumentException();
                    }

            } catch {
                result = null;
                return false;
            }

            return true;
        }
    }
}
