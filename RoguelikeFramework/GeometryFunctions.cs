using System;

namespace RoguelikeFramework {

    public class GeometryFunctions {

        public static double Distance(int x1, int y1, int x2, int y2) {
            int a = x2 - x1;
            int b = y2 - y1;
            return Math.Sqrt(a * a + b * b);
        }

    }

}
