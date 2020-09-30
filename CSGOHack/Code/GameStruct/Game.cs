using CSGOHack.Code.GameConstant;
using GameHackLib.Code;
using GameHackLib.Code.GameData;
using hazedumper;

namespace CSGOHack.Code.GameStruct
{
    class Game: BaseGame
    {
        public Game() : base(new LocalPlayer())
        {
        }

        public override void Read()
        {
            var processMemory = ProcessMemory.Get();

            var engine = processMemory.GetDllAddress("engine.dll");
            if (processMemory.IsValid(engine))
            {
                MaxPlayers = processMemory.ReadInt32(engine + signatures.dwClientState_MaxPlayer);

                long dwClientState = processMemory.ReadInt32(engine + signatures.dwClientState);
                if (processMemory.IsValid(dwClientState))
                {
                    ViewAngles = processMemory.ReadVector2(dwClientState + signatures.dwClientState_ViewAngles);
                    ViewAnglesAddress = dwClientState + signatures.dwClientState_ViewAngles;
                }
            }

            var client_panorama = processMemory.GetDllAddress("client.dll");

            if (processMemory.IsValid(client_panorama))
            {
                ViewMatrix = processMemory.ReadMatrix(client_panorama + signatures.dwViewMatrix);

                var dwLocalPlayer = processMemory.ReadInt32(client_panorama + signatures.dwLocalPlayer);
                if (processMemory.IsValid(dwLocalPlayer))
                    LocalPlayer.Read(dwLocalPlayer);

                Players.Clear();
                for (var i = 0; i < 32; i++)
                {
                    var dwPlayer = processMemory.ReadInt32(client_panorama + signatures.dwEntityList + i * Player.PlayerSize);

                    if (processMemory.IsValid(dwPlayer))
                    {
                        var player = new Player();
                        player.Read(dwPlayer);
                        if (player.IsValid())
                            Players.Add(player);
                    }
                }
            }
        }
    }
}
