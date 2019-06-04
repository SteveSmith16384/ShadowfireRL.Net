using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowfireRL
{
    public interface IGameView
    {
        void startScreen();

        void clear();

        void refresh();

        void drawCharacter(int x, int y, RLCell ch);

        void setTextForegroundColor(ConsoleColor colour);

        void drawString(int x, int y, String s);

        //KeyStroke getInput() throws IOException;

        void close();
    }
}
