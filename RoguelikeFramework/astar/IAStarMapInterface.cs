using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeFramework.astar {

    public interface IAStarMapInterface {

        int GetMapWidth();

        int GetMapHeight();

        bool IsMapSquareTraversable(int x, int y);

        float GetMapSquareDifficulty(int x, int y);

    }
}
