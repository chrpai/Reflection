using System.Collections.Generic;
using System.IO;
using System;
using System.Reflection;

namespace Libs
{
    public interface IPlugin
    {
        int Test { get; }
    }
    public class LibStandardPlugin : IPlugin
    {
        int IPlugin.Test { get { return 42; } }
    }
    public class Server
    {
        public static void Test()
        {
            string currentDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            List<Assembly> pluginAssemblies = new List<Assembly>();
            string[] files = Directory.GetFiles(currentDirectory, "*.dll", SearchOption.AllDirectories);
            bool which = false;
            if (which)
            {
                pluginAssemblies.Add(Assembly.GetExecutingAssembly());
            }
            else
            {
                foreach (string file in files)
                {
                    pluginAssemblies.Add(Assembly.LoadFile(file));
                }
            }
            foreach (Assembly pluginAssembly in pluginAssemblies)
            {
                // Look for class(s) with our interface and construct them
                Type[] types = pluginAssembly.GetTypes();
                foreach (var type in types)
                {
                    Type iDesigner = type.GetInterface(typeof(IPlugin).FullName);
                    if (iDesigner != null)
                    {
                        object instance = Activator.CreateInstance(type); // creates an object 
                        IPlugin plugin = (IPlugin)instance; // throws an exception
                        Console.WriteLine(plugin.Test);
                    }
                }
            }
        }
    }
}