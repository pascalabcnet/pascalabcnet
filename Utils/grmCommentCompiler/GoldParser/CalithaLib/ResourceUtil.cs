using System;
using System.Resources;
using System.IO;
using System.Reflection;

namespace com.calitha.commons
{
	/// <summary>
	/// Summary description for ResourceUtil.
	/// </summary>
	public sealed class ResourceUtil
	{
		private ResourceUtil()
		{}

		public static void AddFileAsStringResource(string resourceFile, 
                                                   string resourceName,
                                                   string inputFile)
		{
			ResourceWriter writer = new ResourceWriter(resourceFile);
			StreamReader reader = new StreamReader(inputFile);
			string s = reader.ReadToEnd();
			reader.Close();
			writer.AddResource(resourceName, s);
			writer.Close();
		}

		public static string GetStringResource(Assembly assembly,
                                               string baseName, 
			                                   string resourceName)
		{
			System.Resources.ResourceManager rm =
				new System.Resources.ResourceManager(baseName, assembly);
			return (String)rm.GetObject(resourceName);
		}

		public static byte[] GetByteArrayResource(Assembly assembly,
			                                      string baseName,
			                                      string resourceName)
		{
			System.Resources.ResourceManager rm =
				new System.Resources.ResourceManager(baseName, assembly);
			return (System.Byte[])rm.GetObject(resourceName);
		}

	}
}
