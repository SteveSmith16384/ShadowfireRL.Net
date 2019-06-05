using OpenTK;
using RoguelikeFramework.models;
using System;
using System.Collections.Generic;

namespace RoguelikeFramework.view {

    public interface IDataForView {

        MapData GetMapData();

        string GetHoverText();

        List<string> GetLog();

        List<Point> GetLine();

        Dictionary<int, AbstractEntity> GetUnits();

        AbstractEntity GetCurrentUnit();
    }
}
