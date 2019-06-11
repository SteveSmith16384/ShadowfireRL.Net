using RoguelikeFramework.models;
using System.Collections.Generic;
using System.Drawing;

namespace RoguelikeFramework.view {

    public interface IDataForView {

        MapData GetMapData();

        string GetHoverText();

        List<string> GetLog();

        List<Point> GetLine();

        List<AbstractEntity> GetUnits();

        Dictionary<int, AbstractEntity> GetItemSelectionList();

        AbstractEntity GetCurrentUnit();

        List<string> GetStatsFor(AbstractEntity e);
    }
}
