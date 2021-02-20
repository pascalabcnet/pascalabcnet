﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;

namespace ICSharpCode.Core
{
	public interface IStringTagProvider
	{
		string ProvideString(string tag, StringTagPair[] customTags);
	}
}
