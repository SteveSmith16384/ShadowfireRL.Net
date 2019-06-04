using RLNET;

namespace RoguelikeFramework.components {

    public class GraphicComponent : AbstractComponent {

        private RLCell ch;
        public bool is_visible = true;

        public GraphicComponent(int _ch, RLColor foreground, RLColor background) {
            this.ch = new RLCell(background, foreground, _ch);
        }


        public RLCell getChar() {
            return this.ch;
        }
    }
}
