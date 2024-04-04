using System;
using System.Resources;
using System.IO;
using System.ComponentModel.Design;

namespace SampleDesignerHost
{
	/// Empty implementation of a ResourceService. Here is where you can control the way in which
	/// resources are read and written. This sample doesn't play with them though.
	public class SampleResourceService : IResourceService
	{
		private ResourceReader reader;
		private ResourceWriter writer;
		private IDesignerHost host;
		private MemoryStream ms;

		public SampleResourceService(IDesignerHost host) 
		{
			this.host = host;
		}

		#region Implementation of IResourceService

		public System.Resources.IResourceReader GetResourceReader(System.Globalization.CultureInfo info)
		{				
			if (reader == null)
			{
				if (ms == null)
				{
					ms = new MemoryStream();
				}
				reader = new ResourceReader(ms);
			}
			return reader;
		}

		public System.Resources.IResourceWriter GetResourceWriter(System.Globalization.CultureInfo info)
		{
			if (writer == null)
			{
				if (ms == null)
				{
					ms = new MemoryStream();
				}
				writer = new ResourceWriter(ms);
			}
			return writer;
		}

		#endregion
	}
}
