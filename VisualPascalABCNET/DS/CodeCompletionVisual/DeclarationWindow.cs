using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ICSharpCode.TextEditor;

namespace ICSharpCode.TextEditor.Gui.CompletionWindow
{
	public class DeclarationWindow : DeclarationViewWindow
	{
		internal const int CompletionGap = 2;
		internal Form CompletionWindow { get; set; }
		
		public DeclarationWindow(Form parent):base(parent)
		{
			
		}
		
		protected override void OnPaint(PaintEventArgs pe)
		{
			string description = Description;
			if (string.IsNullOrEmpty(description))
			{
				base.OnPaint(pe);
				return;
			}

			if (CompletionWindow != null)
			{
				int returnsIndex = description.IndexOf("<returns>", StringComparison.Ordinal);
				int paramsIndex = description.IndexOf("<params>", StringComparison.Ordinal);
				int detailsIndex = returnsIndex < 0 ? paramsIndex
					: paramsIndex < 0 ? returnsIndex : Math.Min(returnsIndex, paramsIndex);
				if (detailsIndex >= 0)
					description = description.Substring(0, detailsIndex).TrimEnd();
			}
			ICSharpCode.TextEditor.Util.TipPainterTools.DrawHelpTipFromCombinedDescription(
				this, pe.Graphics, Font, null, description, -1, -1, false);
		}
	}
}

