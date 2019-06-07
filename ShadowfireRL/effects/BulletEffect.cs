﻿using RLNET;
using RoguelikeFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowfireRL.effects {

    public class BulletEffect : AbstractEffect {

        public List<Point> line;

        public BulletEffect(int sx, int sy, int ex, int ey) : base(100) {

            this.line = Misc.GetLine(sx, sy, ex, ey);
        }

        protected override void subprocess() {
            this.line.RemoveAt(0);
        }

        public override void draw(RLConsole map) {
            if (this.line.Count > 0) {
                var p = this.line[0];
                map.SetChar(p.X, p.Y, '*');
            }
        }

        public override bool hasEnded() {
            return this.line.Count == 0;
        }

    }

}
