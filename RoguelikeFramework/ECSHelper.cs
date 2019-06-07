using RoguelikeFramework.components;

namespace RoguelikeFramework {
    public class ECSHelper {


        public static PositionComponent GetPosition(AbstractEntity entity) {
            // Check if it is being carried
            CarryableComponent cc = (CarryableComponent)entity.getComponent(nameof(CarryableComponent));
            if (cc != null) {
                if (cc.carrier != null) {
                    PositionComponent pos = (PositionComponent)cc.carrier.getComponent(nameof(PositionComponent));
                    return pos;
                }
            }

            // Get our own position
            PositionComponent sqpos = (PositionComponent)entity.getComponent(nameof(PositionComponent));
            return sqpos;
        }

    }
}
