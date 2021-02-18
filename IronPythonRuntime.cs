using System;
using CSR;
using System.Collections.Generic;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.IO;
using IronPython.Runtime;

namespace IronPythonRunner
{
    public class IronPythonRuntime
    {
        public static Dictionary<string, List<dynamic>> PyFunctiON = new Dictionary<string, List<dynamic>>();
        public static Dictionary<string, IntPtr> ptr = new Dictionary<string, IntPtr>();
        public static string version = "Release0218";
        public class MCPYAPI
        {
            private MCCSAPI api { get; set; }
            private Dictionary<string, int> TPFuncPtr { get; set; }
            public MCPYAPI(MCCSAPI api)
            {
                this.api = api;
            }     
            #region IPYRAPI         
            public void tellraw(string towho, string msg)
            {
                api.runcmd("tellraw \"" + towho + "\" {\"rawtext\":[{\"text\":\"" + msg + "\"}]}");
            }           
            public GUI.GUIBuilder creatGUI(string title)
            {
                return new GUI.GUIBuilder(api, title);
            }
            public CsPlayer creatPlayerObject(string uuid) 
            {
                try
                {
                    var pl = ptr[uuid];
                    return new CsPlayer(api, pl);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
            public CsActor getActorFromUniqueid(ulong uniqueid)
            {
                return CsActor.getFromUniqueId(api, uniqueid);
            }
            public CsPlayer getPlayerFromUniqueid(ulong uniqueid)
            {
                return (CsPlayer)CsPlayer.getFromUniqueId(api, uniqueid);
            }
            public CsActor[] getFromAABB(int did,float x1,float y1,float z1, float x2, float y2, float z2) 
            {
                var temp = new List<CsActor>();
                var raw = CsActor.getsFromAABB(api, did, x1, y1, z2, x2, y2, z2);
                foreach (var i in raw)
                {
                    temp.Add((CsActor)i);
                }
                return temp.ToArray();
            }
            public CsPlayer convertActorToPlayer(CsActor ac)
            { 
                return (CsPlayer)ac;
            }
            #endregion
        }
        public static void RunIronPython(MCCSAPI api)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[IPYR2] Load! version = " + version);
            Console.WriteLine("[IPYR2] Based on IronPythonRunner");
            Console.WriteLine("[IPYR2] License Apache2.0");
            List<IntPtr> uuid = new List<IntPtr>();
            const String path = "./ipy";
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists("./IronPython27.zip"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[IPYR2] Cannot Find The Libs，put it inside root directory!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine("[IPYR2] Reading Plugins...");
            DirectoryInfo Allfolder = new DirectoryInfo(path);
            var tool = new ILR.ToolFunc();
            ScriptEngine pyEngine = Python.CreateEngine();
            var Libpath = pyEngine.GetSearchPaths();
            List<string> LST = new List<string>(Libpath.Count)
            {
                    "C:\\Program Files\\IronPython 2.7\\Lib",
                    ".\\IronPython27.zip"
            };
            pyEngine.SetSearchPaths(LST);
            pyEngine.CreateModule("ipyapi");
            pyEngine.CreateModule("tool");
            pyEngine.GetClrModule();
            ScriptScope py_ = pyEngine.CreateScope();
            py_.SetVariable("tool",tool);
            py_.SetVariable("ipyapi", new MCPYAPI(api));
            foreach (FileInfo file in Allfolder.GetFiles("*.net.py"))
            {
                try
                {
                    Console.WriteLine("[IPYR2] Load\\ipy\\" + file.Name);
                    pyEngine.ExecuteFile(file.FullName,py_);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("[IPYR2] " + file.Name + " Load Successful");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.WriteLine("[IPYR2] Failed to load " + file.Name);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            api.addBeforeActListener(EventKey.onLoadName, x =>
            {
                var a = BaseEvent.getFrom(x) as LoadNameEvent;
                ptr.Add(a.uuid, a.playerPtr);
                return true;
            });
            api.addBeforeActListener(EventKey.onPlayerLeft, x =>
            {
                var a = BaseEvent.getFrom(x) as PlayerLeftEvent;
                ptr.Remove(a.uuid);
                return true;
            });
        }
    }
}

namespace CSR
{
    partial class Plugin
    {
        public static void onStart(MCCSAPI api)
        {
            IronPythonRunner.IronPythonRuntime.RunIronPython(api);
            Console.WriteLine("[IPYR2] IronPythonRunner2 loaded!");
        }
    }
}
