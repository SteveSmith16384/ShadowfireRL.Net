using System;
using System.Collections.Generic;

namespace RoguelikeFramework {
    public class Misc {

        public static Random random = new Random();


        // Modified from https://stackoverflow.com/questions/11678693/all-cases-covered-bresenhams-line-algorithm
        public static List<Tuple<int, int>> line(int x, int y, int x2, int y2) {
            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest)) {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            for (int i = 0; i <= longest; i++) {
                list.Add(new Tuple<int, int>(x, y));
                numerator += shortest;
                if (!(numerator < longest)) {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                } else {
                    x += dx2;
                    y += dy2;
                }
            }
            return list;
        }


    }
}
