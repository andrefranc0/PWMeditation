using System;
using System.Collections.Generic;
using System.Text;

namespace SiviliaFramework.Network
{
    public abstract class Plugin
    {
        virtual internal protected void Initialize(OOGHost oogHost) { }
        virtual internal protected void Open() { }
        virtual internal protected void Close() { }
    }
    public class PluginManager
    {
        public PluginManager(OOGHost oogHost)
        {
            HostPlugin = oogHost;
        }

        public OOGHost HostPlugin { get; set; }

        private Dictionary<Type, Plugin> plugins = new Dictionary<Type, Plugin>();

        public T IncludePlugin<T>() where T : Plugin
        {
            Type pluginType = typeof(T);

            Plugin res;
            if (plugins.TryGetValue(pluginType, out res))
                return (T)res;

            res = (Plugin)pluginType.GetConstructor(new Type[0]).Invoke(new object[0]);
            res.Initialize(HostPlugin);
            plugins.Add(pluginType, res);

            return (T)res;
        }
        public void Open()
        {
            foreach (Plugin plugin in plugins.Values)
            {
                plugin.Open();
            }
        }
        public void Close()
        {
            foreach (Plugin plugin in plugins.Values)
            {
                plugin.Close();
            }
        }
    }
}
