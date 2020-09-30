namespace CSGOHack.Code.GameConstant
{
    public class Config
    {
        public static string WindowName { get; }
        public static string ProcessName { get; }

        static Config()
        {
            WindowName = "Counter-Strike: Global Offensive";
            ProcessName = "csgo";
        }
    }
}
