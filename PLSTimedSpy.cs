using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using Rocket.Core.Logging;
using Steamworks;
using SDG.Unturned;
using System.IO;
using System.Threading;
using System;
using System.Collections.Generic;
using Rocket.Unturned.Chat;
using System.Runtime.Serialization;

//Creado y Codificado por Dreiken. Steam /realdreiken/

namespace PLSTimedSpy
{
    public class PLSTimedSpy : RocketPlugin<PLSTimedSpyConfiguration>
    {
        public string serverFolder;
        public static PLSTimedSpy Instance;
        public DateTime tiempo1;

        void FixedUpdate()
        {
            if (this.Configuration.Instance.TimeSpy) TimeSpy();
        }

        protected override void Load()
        {
            Logger.LogWarning("PLSTimedSpy Loaded!");
            Logger.LogWarning("Created by Dreiken");
            Instance = this;
            serverFolder = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).ToString();
            DirectoryExist("PleaseGamingFunctionalities");
            DirectoryExist("KillSpy");
            DirectoryExist("TimeSpy");
            DirectoryExist("BackupSpy");
            UnturnedPlayerEvents.OnPlayerDeath += OnPlayerDeath;
            U.Events.OnPlayerConnected += OnPlayerConnected;

        }

        protected override void Unload()
        {
            Logger.LogWarning("PLSTimedSpy Unloaded!");
            UnturnedPlayerEvents.OnPlayerDeath -= OnPlayerDeath;
            U.Events.OnPlayerConnected -= OnPlayerConnected;
        }

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            EspiadorInvi(player, true);
        }

        private void OnPlayerDeath(UnturnedPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer)
        {
            UnturnedPlayer asesino = UnturnedPlayer.FromCSteamID(murderer);

            if ((cause.ToString() == "BLEEDING") || (cause.ToString() == "ZOMBIE") || (cause.ToString() == "BONES") || (cause.ToString() == "FOOD")
                || (cause.ToString() == "INFECTION") || (cause.ToString() == "WATER") || (cause.ToString() == "SHRED") || ((cause.ToString() == "SUICIDE") || (cause.ToString() == "BREATH"))) return;
            
            if (asesino == null) return;

            else
            { 
                if (this.Configuration.Instance.KillSpy)
                {
                    EspiadorInvi(asesino, false);
                }
            }

        }

        void Spy(UnturnedPlayer player)
        {
            player.Player.sendScreenshot((CSteamID)0);
        }

        void MoveSpy(UnturnedPlayer player, string DirectoryName, int Times)
        {
            if (File.Exists(Path.GetFullPath(serverFolder + "/Spy/" + player.CSteamID.ToString() + ".jpg")))
                File.Move(Path.GetFullPath(serverFolder + "/Spy/" + player.CSteamID.ToString() + ".jpg"), Path.GetFullPath(serverFolder + "/PleaseGamingFunctionalities/" + DirectoryName + "/" + player.DisplayName.ToString() + "_" + player.CSteamID.ToString() + "_" + CheckExist(player, DirectoryName, Times) + ".jpg"));
        }

        int CheckExist(UnturnedPlayer player, string DirectoryName, int Times)
        {
            DateTime[] creacion = new DateTime[(Times)];
            for (int a = 0; a <= (Times - 1); a++)
            {
                if (File.Exists(Path.GetFullPath(serverFolder + "/PleaseGamingFunctionalities/" + DirectoryName + "/" + player.DisplayName.ToString() + "_" + player.CSteamID.ToString() + "_" + a + ".jpg"))) ;
                else
                {
                    return a;
                }
            }
            for (int a = 0; a <= (Times - 1); a++)
            {
                creacion[a] =
                File.GetLastWriteTime(Path.GetFullPath(serverFolder + "/PleaseGamingFunctionalities/" + DirectoryName + "/" + player.DisplayName.ToString() + "_" + player.CSteamID.ToString() + "_" + a + ".jpg"));
            }
            if (creacion[0].CompareTo(creacion[Times - 1]) < 0)
            {
                if (this.Configuration.Instance.BackupSpy == true)
                {
                    File.Copy(Path.GetFullPath(serverFolder + "/PleaseGamingFunctionalities/" + DirectoryName + "/" + player.DisplayName.ToString() + "_" + player.CSteamID.ToString() + "_" + 0 + ".jpg"),
                   Path.GetFullPath(serverFolder + "/PleaseGamingFunctionalities/" + "BackupSpy/" + player.DisplayName.ToString() + "_" + DateTime.Now.ToString() + ".jpg"));
                    File.Delete(Path.GetFullPath(serverFolder + "/PleaseGamingFunctionalities/" + DirectoryName + "/" + player.DisplayName.ToString() + "_" + player.CSteamID.ToString() + "_" + 0 + ".jpg"));
                }

                else File.Delete(Path.GetFullPath(serverFolder + "/PleaseGamingFunctionalities/" + DirectoryName + "/" + player.DisplayName.ToString() + "_" + player.CSteamID.ToString() + "_" + 0 + ".jpg"));

                return 0;
            }
            for (int a = 0; a <= (Times - 1); a++)
            {
                if (creacion[(a + 1)].CompareTo(creacion[a]) < 0)
                {
                    if (this.Configuration.Instance.BackupSpy == true)
                    {
                        File.Copy(Path.GetFullPath(serverFolder + "/PleaseGamingFunctionalities/" + DirectoryName + "/" + player.DisplayName.ToString() + "_" + player.CSteamID.ToString() + "_" + (a + 1) + ".jpg"),
                   Path.GetFullPath(serverFolder + "/PleaseGamingFunctionalities/" + "BackupSpy/" + player.DisplayName.ToString() + "_" + DateTime.Now.ToString() + ".jpg"));
                        File.Delete(Path.GetFullPath(serverFolder + "/PleaseGamingFunctionalities/" + DirectoryName + "/" + player.DisplayName.ToString() + "_" + player.CSteamID.ToString() + "_" + (a + 1) + ".jpg"));
                    }

                    else File.Delete(Path.GetFullPath(serverFolder + "/PleaseGamingFunctionalities/" + DirectoryName + "/" + player.DisplayName.ToString() + "_" + player.CSteamID.ToString() + "_" + (a + 1) + ".jpg"));

                    return (a + 1);
                }
            }
            return 0;
        }

        private void TimeSpy()
        {
            if ((tiempo1.AddSeconds(this.Configuration.Instance.TimeSpyFrequency) - DateTime.Now).TotalSeconds < 0)
            {
                foreach (var steamplayer in Provider.Players)
                {
                    EspiadorTiempo(UnturnedPlayer.FromPlayer(steamplayer.Player));
                }
                tiempo1 = DateTime.Now;
            }
        }

        void DirectoryExist(string DirectoryName)
        {
            if (DirectoryName == "PleaseGamingFunctionalities")
            {
                if (System.IO.Directory.Exists(Path.GetFullPath(serverFolder + "/PleaseGamingFunctionalities/"))) return;
                else System.IO.Directory.CreateDirectory(Path.GetFullPath(serverFolder + "/PleaseGamingFunctionalities/"));
            }
            else
            {
                if (System.IO.Directory.Exists(Path.GetFullPath(serverFolder + "/PleaseGamingFunctionalities/" + DirectoryName))) return;
                else System.IO.Directory.CreateDirectory(Path.GetFullPath(serverFolder + "/PleaseGamingFunctionalities/" + DirectoryName));
            }
        }

        public void EspiadorTiempo(UnturnedPlayer player)
        {
            new Thread(() =>
            {
                Spy(player);
                Thread.Sleep(3500);
                MoveSpy(player, "TimeSpy", this.Configuration.Instance.TimeSpyScreens);
            })
            {
                IsBackground = true
            }.Start();
        }

        public void EspiadorInvi(UnturnedPlayer player, bool connected)
        {
            new Thread(() =>
            {
                if (connected)
                {
                    Thread.Sleep(6000);
                    Spy(player);
                }
                else
                {
                    Spy(player);
                    Thread.Sleep(3500);
                    MoveSpy(player, "KillSpy", this.Configuration.Instance.KillSpyScreens);
                }

            })
            {
                IsBackground = true
            }.Start();
        }
    }
}
