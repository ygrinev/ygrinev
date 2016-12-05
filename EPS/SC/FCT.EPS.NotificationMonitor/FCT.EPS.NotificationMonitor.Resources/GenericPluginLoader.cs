using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace FCT.EPS.NotificationMonitor.Resources
{
	public static class GenericPluginLoader<T>
	{
		public static ICollection<T> LoadPlugins(string path)
		{
            SolutionTraceClass.WriteLineVerbose("Start");
            SolutionTraceClass.WriteLineInfo(string.Format("The path is '{0}'",path));
			string[] dllFileNames = null;
            string[] dirNames = null;

            AppDomainSetup myAppDomainSetup = new AppDomainSetup();
           

			if(Directory.Exists(path))
			{

                ICollection<T> plugins = new List<T>();

                //Check subdirectories
                dirNames = Directory.GetDirectories(path);
                foreach(string myPath in dirNames)
                {
                    ICollection<T> temp = GenericPluginLoader<T>.LoadPlugins(myPath);
                    foreach (T type in temp)
                    {
                        SolutionTraceClass.WriteLineVerbose(string.Format("Adding file '{0}' to plugins collection.", type.GetType().AssemblyQualifiedName));
                        plugins.Add(type);
                    }
                }

				dllFileNames = Directory.GetFiles(path, "*.dll");

				ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
				foreach(string dllFile in dllFileNames)
				{
					AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                    Assembly assembly = Assembly.Load(an);
					assemblies.Add(assembly);
				}

                Type pluginType = typeof(T);
                ICollection<Type> pluginTypes = new List<Type>();
				foreach(Assembly assembly in assemblies)
				{
					if(assembly != null)
					{
						Type[] types = assembly.GetTypes();

						foreach(Type type in types)
						{
							if(type.IsInterface || type.IsAbstract)
							{
								continue;
							}
							else
							{
								if(type.GetInterface(pluginType.FullName) != null)
								{
									pluginTypes.Add(type);
								}
							}
						}
					}
				}

				foreach(Type type in pluginTypes)
				{
                    AppDomainSetup ads = new AppDomainSetup();
                    ads.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;//Path.GetDirectoryName(type.Assembly.CodeBase);// Assembly.GetExecutingAssembly().CodeBase;
                    //string pluginLocation = Path.GetDirectoryName(type.Assembly.Location);
                    //string appDirectory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                    ads.PrivateBinPath = Utility.GetRelativeSearchPath(Path.GetDirectoryName(type.Assembly.Location), AppDomain.CurrentDomain.BaseDirectory, ";");
                    ads.ConfigurationFile = type.Assembly.CodeBase + ".config";
                    AppDomain myAppDomail = System.AppDomain.CreateDomain(Guid.NewGuid().ToString(), AppDomain.CurrentDomain.Evidence, ads, AppDomain.CurrentDomain.PermissionSet);

                    T plugin = (T)myAppDomail.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);

                    //T plugin = (T)Activator.CreateInstance(type);
					plugins.Add(plugin);
				}

                SolutionTraceClass.WriteLineVerbose("End with plugins");
				return plugins;
			}
            else
            {
                SolutionTraceClass.WriteLineInfo(string.Format("Path '{0}' does not exist.", path));
            }
            SolutionTraceClass.WriteLineVerbose("End with no plugins");
			return null;
		}
	}
}
