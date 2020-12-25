using Placehold.Keyboard;
using Placehold.Template;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;

namespace Placehold.Plugin
{
    public class PluginManager
    {
        public void InitPlugins()
        {
            foreach (var filePath in Directory.GetFiles(ConfigurationManager.AppSettings["pluginDir"], "*.dll", SearchOption.AllDirectories))
            {
                var assembly = GetAssembly(filePath);
                
                foreach (var placeholdEvent in GetPlaceholdEvents(assembly))
                {
                    KeyboardManager.templateTriggerHook += placeholdEvent.OnCaptured;
                } 
            }
        }

        private IEnumerable<IPlaceholdEvent> GetPlaceholdEvents(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(IPlaceholdEvent).IsAssignableFrom(type))
                {
                    var result = Activator.CreateInstance(type) as IPlaceholdEvent;
                    if (result != null)
                    {
                        yield return result;
                    }
                }
            }
        }

        private Assembly GetAssembly(string root)
        {
            var pluginLocation = root;
            PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);

            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }
    }
}
