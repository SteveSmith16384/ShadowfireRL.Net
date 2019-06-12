using System.Collections.Generic;

namespace RoguelikeFramework.models {

    public class GameLog {

        private List<string> entries = new List<string>();
        private int max;

        public GameLog(int m) {
            this.max = m;
        }


        public List<string> GetEntries() {
            return this.entries;
        }


        public void Add(string s) {
            this.entries.Add(s);

            while (this.entries.Count > this.max) {
                this.entries.RemoveAt(0);
            }
        }

    }

}
