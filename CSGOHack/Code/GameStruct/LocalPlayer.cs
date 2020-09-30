using System.Numerics;
using CSGOHack.Code.GameConstant;
using GameHackLib.Code;
using GameHackLib.Code.GameData;
using hazedumper;

namespace CSGOHack.Code.GameStruct
{
    class LocalPlayer: BaseLocalPlayer
    {
        public override void Read(int dwLocalPlayer)
        {
            var processMemory = ProcessMemory.Get();

            Position = processMemory.ReadVector3(dwLocalPlayer + netvars.m_vecOrigin);
            HeadPosition = new Vector3(Position.X, Position.Y, Position.Z + 69);
            TeamID = processMemory.ReadInt32(dwLocalPlayer + netvars.m_iTeamNum);
        }
    }
}
