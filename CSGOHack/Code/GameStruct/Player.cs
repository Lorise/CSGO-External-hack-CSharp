using System;
using CSGOHack.Code.GameConstant;
using GameHackLib.Code;
using GameHackLib.Code.GameData;
using hazedumper;

namespace CSGOHack.Code.GameStruct
{
    class Player: BasePlayer
    {
        public const int PlayerSize = 0x10;
        public const int BoneSize = 0x30;

        public override void Read(int dwPlayer)
        {
            var processMemory = ProcessMemory.Get();

            Position = processMemory.ReadVector3(dwPlayer + netvars.m_vecOrigin);
            Yaw = (float)Math.PI / 180 * processMemory.ReadFloat(dwPlayer + 0x12C);                         //////////////////////////
            Pitch = 0;
            TeamID = processMemory.ReadInt32(dwPlayer + netvars.m_iTeamNum);
            Health = processMemory.ReadInt32(dwPlayer + netvars.m_iHealth);
            Armor = processMemory.ReadInt32(dwPlayer + netvars.m_ArmorValue);

            var m_dwBoneMatrix = processMemory.ReadInt32(dwPlayer + netvars.m_dwBoneMatrix);
            if (processMemory.IsValid(m_dwBoneMatrix))
            {
                if(TeamID == CSGOConstants.TeamID_t)
                    ReadSkeleton_t(m_dwBoneMatrix);

                else if(TeamID == CSGOConstants.TeamID_ct)
                    ReadSkeleton_ct(m_dwBoneMatrix);
            }
        }

        public override bool IsValid()
        {
            return
                (Health > CSGOConstants.MinHealth && Health <= CSGOConstants.MaxHealth) &&
                (Armor >= CSGOConstants.MinArmor && Armor <= CSGOConstants.MaxArmor) &&
                (TeamID == CSGOConstants.TeamID_ct || TeamID == CSGOConstants.TeamID_t);
        }

        protected override void ReadSkeleton_t(int m_dwBoneMatrix)
        {
            Skeleton.Head = ReadBone(m_dwBoneMatrix, 8, BoneSize);

            Skeleton.Neck = ReadBone(m_dwBoneMatrix, 7, BoneSize);

            Skeleton.LeftShoulder = ReadBone(m_dwBoneMatrix, 39, BoneSize);
            Skeleton.LeftElbow = ReadBone(m_dwBoneMatrix, 40, BoneSize);
            Skeleton.LeftHand = ReadBone(m_dwBoneMatrix, 41, BoneSize);

            Skeleton.RightShoulder = ReadBone(m_dwBoneMatrix, 11, BoneSize);
            Skeleton.RightElbow = ReadBone(m_dwBoneMatrix, 12, BoneSize);
            Skeleton.RightHand = ReadBone(m_dwBoneMatrix, 13, BoneSize);

            Skeleton.Spine1 = ReadBone(m_dwBoneMatrix, 6, BoneSize);
            Skeleton.Spine2 = ReadBone(m_dwBoneMatrix, 0, BoneSize);

            Skeleton.LeftKnee = ReadBone(m_dwBoneMatrix, 74, BoneSize);
            Skeleton.LeftFoot1 = ReadBone(m_dwBoneMatrix, 75, BoneSize);
            Skeleton.LeftFoot2 = ReadBone(m_dwBoneMatrix, 76, BoneSize);

            Skeleton.RightKnee = ReadBone(m_dwBoneMatrix, 67, BoneSize);
            Skeleton.RightFoot1 = ReadBone(m_dwBoneMatrix, 68, BoneSize);
            Skeleton.RightFoot2 = ReadBone(m_dwBoneMatrix, 69, BoneSize);
        }

        protected override void ReadSkeleton_ct(int m_dwBoneMatrix)
        {
            Skeleton.Head = ReadBone(m_dwBoneMatrix, 8, BoneSize);

            Skeleton.Neck = ReadBone(m_dwBoneMatrix, 7, BoneSize);

            Skeleton.LeftShoulder = ReadBone(m_dwBoneMatrix, 68, BoneSize);
            Skeleton.LeftElbow = ReadBone(m_dwBoneMatrix, 42, BoneSize);
            Skeleton.LeftHand = ReadBone(m_dwBoneMatrix, 43, BoneSize);

            Skeleton.RightShoulder = ReadBone(m_dwBoneMatrix, 38, BoneSize);
            Skeleton.RightElbow = ReadBone(m_dwBoneMatrix, 12, BoneSize);
            Skeleton.RightHand = ReadBone(m_dwBoneMatrix, 13, BoneSize);

            Skeleton.Spine1 = ReadBone(m_dwBoneMatrix, 6, BoneSize);
            Skeleton.Spine2 = ReadBone(m_dwBoneMatrix, 0, BoneSize);

            Skeleton.LeftKnee = ReadBone(m_dwBoneMatrix, 78, BoneSize);
            Skeleton.LeftFoot1 = ReadBone(m_dwBoneMatrix, 79, BoneSize);
            Skeleton.LeftFoot2 = ReadBone(m_dwBoneMatrix, 80, BoneSize);

            Skeleton.RightKnee = ReadBone(m_dwBoneMatrix, 71, BoneSize);
            Skeleton.RightFoot1 = ReadBone(m_dwBoneMatrix, 72, BoneSize);
            Skeleton.RightFoot2 = ReadBone(m_dwBoneMatrix, 73, BoneSize);
        }
    }
}
