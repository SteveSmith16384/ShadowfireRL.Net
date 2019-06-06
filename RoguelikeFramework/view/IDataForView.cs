using RoguelikeFramework.models;
using System.Collections.Generic;

namespace RoguelikeFramework.view {

    public interface IDataForView {

        MapData GetMapData();

        string GetHoverText();

        List<string> GetLog();

        List<Point> GetLine();

        Dictionary<int, AbstractEntity> GetUnits();

        Dictionary<int, AbstractEntity> GetItemSelectionList();

        AbstractEntity GetCurrentUnit();

        List<string> GetStatsFor(AbstractEntity e);
    }
}
