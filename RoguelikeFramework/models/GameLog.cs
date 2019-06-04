using System.Collections.Generic;

namespace RoguelikeFramework.models {

    public class GameLog {

        List<string> entries = new List<string>();

        public List<string> GetEntries() {
            return this.entries;
        }
    }

}
