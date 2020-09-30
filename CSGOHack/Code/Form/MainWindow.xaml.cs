using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using CSGOHack.Code.GameConstant;
using GameHackLib.Code;
using GameHackLib.Code.Form;
using GameHackLib.Code.GameHack;
using GameHackLib.Code.GameHack.Aimbot;
using GameHackLib.Code.GameHack.ESPHack;
using System.Numerics;
using System.Runtime.CompilerServices;
using CSGOHack.Code.GameStruct;
using GameHackLib.Code.Log;
using Memory;

namespace CSGOHack.Code.Form
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly ExternalWindow _externalWindow = new ExternalWindow();
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly ProcessMemory _processMemory = ProcessMemory.Get();
        private readonly Game _game = new Game();

        public MainWindow()
        {
            InitializeComponent();

            Title = Config.WindowName;
            LabelTitle.Content = Config.WindowName;
            
            #region ESP

            TogleSwitchEspHealth.IsChecked = ESP.Option.Health;
            TogleSwitchEspHealth.IsCheckedChanged += (sender, args) => { ESP.Option.Health = TogleSwitchEspHealth.IsChecked.Value; };

            TogleSwitchEspName.IsChecked = ESP.Option.Name;
            TogleSwitchEspName.IsCheckedChanged += (sender, args) => { ESP.Option.Name = TogleSwitchEspName.IsChecked.Value; };

            TogleSwitchEspDistance.IsChecked = ESP.Option.Distance;
            TogleSwitchEspSnapline.IsChecked = ESP.Option.Snapline;
            TogleSwitchEspBox2D.IsChecked = ESP.Option.Box2D;
            TogleSwitchEspBox3D.IsChecked = ESP.Option.Box3D;
            TogleSwitchEspOnlyEnemy.IsChecked = true;
            TogleSwitchEspOnlyEnemy.IsCheckedChanged += (sender, args) =>
            {
                Box.Option.IsOnlyEnemyVisible = 
                    SnapLine.Option.IsOnlyEnemyVisible = 
                        Health.Option.IsOnlyEnemyVisible =
                            Armor.Option.IsOnlyEnemyVisible =
                                Bones.Option.IsOnlyEnemyVisible =
                                    Distance.Text.Option.IsOnlyEnemyVisible = TogleSwitchEspOnlyEnemy.IsChecked.Value;
            };


            TogleSwitchAimbot.IsChecked = Aimbot.Option.IsEnable;
            TogleSwitchAimbotDrawFov.IsChecked = Aimbot.Option.DrawFov;
            SliderAimbotFov.Value = Aimbot.Option.Fov;

            #endregion
            LabelAimbotFov.Content = Math.Round(SliderAimbotFov.Value, 2);

            //Test();
        }

        static async void Test()
        {
            //"name": "dwLocalPlayer",
            //"extra": 4,
            //"relative": true,
            //"module": "client_panorama.dll",
            //"offsets": [
            //3
            //    ],
            //"pattern": "8D 34 85 ? ? ? ? 89 15 ? ? ? ? 8B 41 08 8B 48 04 83 F9 FF"

            //dwLocalPlayer = 0xCF6A4C;

            Mem mem = new Mem();
            mem.OpenProcess("csgo");

            long client_panorama = mem.modules["client_panorama.dll"].ToInt64();
            long addr = (await mem.AoBScan("8D 34 85 ? ? ? ? 89 15 ? ? ? ? 8B 41 08 8B 48 04 83 F9 FF")).FirstOrDefault() + 3;
            long dwLocalPlayer = mem.readInt(addr.ToString());

            long a = 0xCF1A3C - dwLocalPlayer;

            Console.ReadLine();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _externalWindow.Owner = this;
            _externalWindow.Show();

            while (!_processMemory.OpenProcess(Config.ProcessName))
                Thread.Sleep(100);

            Thread.Sleep(1000);
            Console.WriteLine("Hack on.");

            _timer.Tick += (o, args) => Hack.Run(_game);
            _timer.Interval = TimeSpan.FromMilliseconds(10);
            _timer.Start();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _timer.Stop();
            _externalWindow.Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                Application.Current.Shutdown();
        }

        private void SliderAimbotFov_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LabelAimbotFov.Content = Math.Round(e.NewValue, 2);
        }
    }
}
