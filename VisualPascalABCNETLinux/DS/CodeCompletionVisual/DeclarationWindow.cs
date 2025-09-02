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
		public bool in_completion_list;
		
		public DeclarationWindow(Form parent):base(parent)
		{
			
		}
		
		private bool has_tags()
		{
			return Description.Contains("<returns>") || Description.Contains("<params>");
		}
		
		protected override void OnPaint(PaintEventArgs pe)
		{
			//pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			if (Description != null && Description.Length > 0) {
				if (!in_completion_list)
					ICSharpCode.TextEditor.Util.TipPainterTools.DrawHelpTipFromCombinedDescription(this, pe.Graphics, Font, null, Description,-1, -1, false);
				else
				{
					if (has_tags())
					{
						string tmp = Description;
						int return_ind = tmp.IndexOf("<returns>");
						int params_ind = tmp.IndexOf("<params>");
						if (return_ind != -1 && params_ind != -1)
							tmp = tmp.Substring(0,Math.Min(return_ind,params_ind)).Trim(' ','\n','\t','\r');
						else
						if (return_ind == -1)
							tmp = tmp.Substring(0,params_ind).Trim(' ','\n','\t','\r');
						else
							tmp = tmp.Substring(0,return_ind).Trim(' ','\n','\t','\r');
						Description = tmp;
						return;
					}
					else
					base.OnPaint(pe);
					
				}
			}
			else
				base.OnPaint(pe);
		}
	}
}

