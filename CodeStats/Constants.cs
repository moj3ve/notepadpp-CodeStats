﻿using System;
using System.Net;
using System.Text.RegularExpressions;
using Kbg.NppPluginNET.PluginInfrastructure;

namespace CodeStats
{
    internal static class Constants
    {
        internal const string PluginName = "Code::Stats";
        internal const string PluginKey = "notepadpp-CodeStats";
        internal static string PluginVersion = string.Format("{0}.{1}.{2}", CodeStatsPackage.CoreAssembly.Version.Major, CodeStatsPackage.CoreAssembly.Version.Minor, CodeStatsPackage.CoreAssembly.Version.Build);
        internal const string EditorName = "notepadpp";
        internal static string EditorVersion {
            get
            {
                var ver = (int)Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETNPPVERSION, 0, 0);
                return ver.ToString();
            }
        }

        internal static string ApiMyPulsesEndpoint = "https://codestats.net/api/my/pulses";
        
        internal static Func<string> LatestPluginVersion = () =>
        {
            var regex = new Regex(@"\[assembly: AssemblyFileVersion\(\""(([0-9]+\.?){3})\""\)\]");

            var client = new WebClient { Proxy = CodeStatsPackage.GetProxy() };

            try
            {
                var about = client.DownloadString("https://raw.githubusercontent.com/p0358/notepadpp-CodeStats/master/CodeStats/Properties/AssemblyInfo.cs");
                var match = regex.Match(about);

                if (match.Success)
                {
                    /*var grp1 = match.Groups[2];
                    var regexVersion = new Regex("([0-9]+)");
                    var match2 = regexVersion.Matches(grp1.Value);
                    return string.Format("{0}.{1}.{2}", match2[0].Value, match2[1].Value, match2[2].Value);*/
                    return match.Groups[2].Value;
                }
                else
                {
                    Logger.Warning("Couldn't auto resolve CodeStats plugin version");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Exception when checking current CodeStats plugin version: ", ex);
            }
            return string.Empty;
        };
    }
}
