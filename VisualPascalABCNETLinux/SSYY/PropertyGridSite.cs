//------------------------------------------------------------------------------
/// <copyright from='1997' to='2002' company='Microsoft Corporation'>
///    Copyright (c) Microsoft Corporation. All Rights Reserved.
///
///    This source code is intended only as a supplement to Microsoft
///    Development Tools and/or on-line documentation.  See these other
///    materials for detailed information regarding Microsoft code samples.
///
/// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;

namespace SampleDesignerApplication
{
	/// A nearly empty implementation of ISite, this class merely passes on
	/// service requests to the host. It is required when we add the events
	/// tab to our property grid.
	public class PropertyGridSite : System.ComponentModel.ISite
	{
		private IServiceProvider sp;
		private IComponent component;

		public PropertyGridSite(IServiceProvider sp, IComponent component)
		{
			this.sp = sp;
			this.component = component;
		}
		#region Implementation of ISite

		public System.ComponentModel.IComponent Component
		{
			get
			{
				return component;
			}
		}

		public System.ComponentModel.IContainer Container
		{
			get
			{
				return null;
			}
		}

		public bool DesignMode
		{
			get
			{
				return false;
			}
		}

		public string Name
		{
			get
			{
				return null;
			}
			set {}
		}

		#endregion

		#region Implementation of IServiceProvider

		public object GetService(Type serviceType)
		{
			if (sp != null)
			{
				return sp.GetService(serviceType);
			}
			return null;
		}

		#endregion
	}
}
