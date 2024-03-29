﻿using RLNET;
using RoguelikeFramework;
using System.Collections.Generic;
using System.Drawing;

namespace ShadowfireRL.effects {

    public class BulletEffect : AbstractEffect {

        private static RLCell ch = new RLCell(RLColor.Black, RLColor.Yellow, '*');
        public List<Point> line;

        public BulletEffect(int sx, int sy, int ex, int ey) : base(100) {

            this.line = Misc.GetLine(sx, sy, ex, ey, true);
        }

        protected override void subprocess() {
            this.line.RemoveAt(0);
        }

        public override void draw(RLConsole map) {
            if (this.line.Count > 0) {
                var p = this.line[0];
                map.Set(p.X, p.Y, ch);
            }
        }

        public override bool hasEnded() {
            return this.line.Count == 0;
        }

    }

}

