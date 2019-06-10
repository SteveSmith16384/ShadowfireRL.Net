using RoguelikeFramework.components;

namespace RoguelikeFramework {
    public class ECSHelper {


        public static PositionComponent GetPosition(AbstractEntity entity) {
            // Check if it is being carried
            CarryableComponent cc = (CarryableComponent)entity.GetComponent(nameof(CarryableComponent));
            if (cc != null) {
                if (cc.carrier != null) {
                    PositionComponent pos = (PositionComponent)cc.carrier.GetComponent(nameof(PositionComponent));
                    return pos;
                }
            }

            // Get our own position
            PositionComponent sqpos = (PositionComponent)entity.GetComponent(nameof(PositionComponent));
            return sqpos;
        }

    }
}
