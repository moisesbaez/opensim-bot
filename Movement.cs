using System;
using OpenMetaverse;

namespace FirstBot
{
    static class Movement
    {
        private static uint regionX = 256000;
        private static uint regionY = 256000;

        public static void MoveBot(double x, double y, double z, GridClient Client)
        {
            x += (double)regionX;
            y += (double)regionY;

            Client.Self.AutoPilot(x, y, z);
        }
    }
}
