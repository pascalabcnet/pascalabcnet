// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="none" email=""/>
//     <version>$Revision: 915 $</version>
// </file>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ICSharpCode.TextEditor.Util
{
	class TipPainterTools
	{
		const int SpacerSize = 4;
		
		private TipPainterTools()
		{
			
		}
        public static Size GetDrawingSizeHelpTipFromCombinedDescription(Control control,
                                                              Graphics graphics,
                                                              Font font,
                                                              string countMessage,
                                                              string description,
                                                              int param_num, int paramsCount, bool addit_info)
        {
            GetToolipParts(description, param_num, paramsCount, out var basicDescription, out var documentation, out var bold_beg, out var bold_len);

            return GetDrawingSizeDrawHelpTip(control, graphics, font, countMessage, basicDescription, documentation, bold_beg, bold_len, param_num, addit_info);
        }

        public static Size DrawHelpTipFromCombinedDescription(Control control,
                                                              Graphics graphics,
                                                              Font font,
                                                              string countMessage,
                                                              string description, int param_num, int paramsCount, bool addit_info)
        {
            GetToolipParts(description, param_num, paramsCount, out var basicDescription, out var documentation, out var bold_beg, out var bold_len);

            return DrawHelpTip(control, graphics, font, countMessage,
                        basicDescription, documentation, bold_beg, bold_len, param_num, addit_info);
        }

        private static void GetToolipParts(string description, int param_num, int paramsCount, out string basicDescription, out string documentation, out int bold_beg, out int bold_len)
        {
            basicDescription = null;
            documentation = null;
            bold_beg = 0;
            bold_len = 0;
            if (IsVisibleText(description))
            {
                string[] splitDescription = description.Split(new char[] { '\n' }, 2);

                if (splitDescription.Length > 0)
                {
                    if (splitDescription.Length > 1)
                    {
                        documentation = splitDescription[1].Trim();
                    }

                    basicDescription = splitDescription[0];

                    if (param_num == -1)
                        return;

                    var languageIntellisenseSupport = CodeCompletion.CodeCompletionController.CurrentLanguage.LanguageIntellisenseSupport;

                    int startIndex = basicDescription.IndexOf("(", basicDescription.IndexOf("(" + PascalABCCompiler.StringResources.Get("CODE_COMPLETION_EXTENSION")) + 1) + 1;

                    int paranthesisIndex = languageIntellisenseSupport.FindClosingParenthesis(basicDescription.Substring(startIndex), ')');

                    if (paranthesisIndex == -1)
                        return;

                    int end_sk = paranthesisIndex + startIndex;

                    if (param_num == 1)
                    {
                        bold_beg = startIndex;

                        if (bold_beg != 0)
                        {
                            int paramDelimIndex = languageIntellisenseSupport.FindParamDelim(basicDescription.Substring(bold_beg), 1);

                            if (paramDelimIndex == -1)
                            {
                                bold_len = end_sk - bold_beg;
                            }
                            else
                            {
                                bold_len = paramDelimIndex;
                            }
                        }
                    }
                    else
                    {
                        // Здесь учтется случай последнего параметра типа params EVA
                        if (param_num > paramsCount)
                        {
                            if (paramsCount == 1)
                            {
                                string paramDescription = basicDescription.Substring(startIndex, paranthesisIndex);

                                if (languageIntellisenseSupport.IsParams(paramDescription))
                                {
                                    bold_beg = startIndex;
                                    bold_len = paranthesisIndex;
                                }
                            }
                            else
                            {
                                string descriptionAfterBracket = basicDescription.Substring(startIndex);

                                int paramDelimIndex = languageIntellisenseSupport.FindParamDelim(descriptionAfterBracket, paramsCount - 1);

                                if (paramDelimIndex != -1)
                                {
                                    int paramDelimLength = languageIntellisenseSupport.ParameterDelimiter.Length;

                                    // Проверка на всякий случай, чтобы не возникало ошибки с отрицательным length в Substring  EVA
                                    if (paranthesisIndex - paramDelimLength < paramDelimLength)
                                        return;

                                    string paramDescription = descriptionAfterBracket.Substring(paramDelimIndex + paramDelimLength, paranthesisIndex - paramDelimIndex - paramDelimLength);

                                    if (languageIntellisenseSupport.IsParams(paramDescription))
                                    {
                                        bold_beg = paramDelimIndex + startIndex + paramDelimLength;
                                        bold_len = end_sk - bold_beg;
                                    }
                                }
                            }
                        }
                        else
                        {
                            int paramDelimIndex = languageIntellisenseSupport.FindParamDelim(basicDescription.Substring(startIndex), param_num - 1);

                            if (paramDelimIndex != -1)
                            {
                                bold_beg = paramDelimIndex + startIndex + languageIntellisenseSupport.ParameterDelimiter.Length;
                                int secondParamDelimIndex = languageIntellisenseSupport.FindParamDelim(basicDescription.Substring(startIndex), param_num);

                                if (secondParamDelimIndex == -1)
                                {
                                    bold_len = end_sk - bold_beg;
                                }
                                else
                                {
                                    bold_len = secondParamDelimIndex + startIndex - bold_beg;
                                }
                            }
                        }
                    }
                }
            }
        }

        // btw. I know it's ugly.
        public static Rectangle DrawingRectangle1;
		public static Rectangle DrawingRectangle2;

        public static Size GetDrawingSizeDrawHelpTip(Control control,
                                       Graphics graphics, Font font,
                                       string countMessage,
                                       string basicDescription,
                                       string documentation,
                                       int beg_bold, int bold_len, int param_num, bool addit_info)
        {
            if (IsVisibleText(countMessage) ||
                IsVisibleText(basicDescription) ||
                IsVisibleText(documentation))
            {
                // Create all the TipSection objects.
                CountTipText countMessageTip = new CountTipText(graphics, font, countMessage);

                TipSpacer countSpacer = new TipSpacer(graphics, new SizeF(IsVisibleText(countMessage) ? 4 : 0, 0));

                TipText descriptionTip = new TipText(graphics, font, basicDescription);
                descriptionTip.insight_wnd = control is ICSharpCode.TextEditor.Gui.InsightWindow.PABCNETInsightWindow;
                descriptionTip.beg_bold = beg_bold;
                descriptionTip.len_bold = bold_len;
                TipText descriptionTip1 = new TipText(graphics, font, basicDescription.Substring(0, beg_bold));
                TipText descriptionTip2 = new TipText(graphics, new Font(font, FontStyle.Bold), basicDescription.Substring(beg_bold, bold_len));
                TipText descriptionTip3 = new TipText(graphics, font, basicDescription.Substring(beg_bold + bold_len));
                descriptionTip.desc1 = descriptionTip1;
                descriptionTip.desc2 = descriptionTip2;
                descriptionTip.desc3 = descriptionTip3;
                TipSpacer docSpacer = new TipSpacer(graphics, new SizeF(0, IsVisibleText(documentation) ? 4 : 0));

                List<TipSection> sections = new List<TipSection>();
                if (documentation != null)
                {
                    try
                    {

                        int params_ind = documentation.IndexOf("<params>");
                        TipText paramsTextTipHeader = null;
                        TipText paramsTextTip = null;
                        if (params_ind != -1 && addit_info)
                        {
                            paramsTextTipHeader = new TipText(graphics, new Font(font, FontStyle.Bold), PascalABCCompiler.StringResources.Get("CODE_COMPLETION_PARAMETERS"));
                            paramsTextTipHeader.is_head = true;
                            int end_param = documentation.IndexOf("<returns>", params_ind);
                            string params_text;
                            if (end_param == -1)
                            {
                                params_text = documentation.Substring(params_ind + 8).Trim(' ', '\n', '\r', '\t');
                                //paramsTextTip = new TipText(graphics,font,params_text);
                            }
                            else
                            {
                                params_text = documentation.Substring(params_ind + 8, end_param - params_ind - 8).Trim(' ', '\n', '\r', '\t');
                                //paramsTextTip = new TipText(graphics,font,documentation.Substring(params_ind+8,end_param-params_ind-8).Trim(' ','\n','\r','\t'));
                            }
                            List<string> prms_list = new List<string>();
                            int ind = params_text.IndexOf("<param>", 0);
                            while (ind != -1)
                            {
                                int end_ind = params_text.IndexOf("</param>", ind + 1);
                                prms_list.Add(params_text.Substring(ind + 7, end_ind - ind - 7).Trim(' ', '\n', '\t', '\r'));
                                ind = params_text.IndexOf("<param>", end_ind);
                                if (ind != -1)
                                {
                                    if (ind - end_ind - 7 > 0)
                                        prms_list.Add(params_text.Substring(end_ind + 8, ind - end_ind - 8).Trim(' ', '\n', '\t', '\r'));
                                    else
                                        prms_list.Add("");
                                }
                                else
                                {
                                    prms_list.Add(params_text.Substring(end_ind + 8).Trim(' ', '\n', '\t', '\r'));
                                }
                            }

                            List<TipSection> prm_sections = new List<TipSection>();
                            int i = 0;
                            while (i < prms_list.Count)
                            {
                                TipText prm_name_sect = new TipText(graphics, font, prms_list[i]);
                                prm_name_sect.param_name = true;
                                TipText desc_param_sect = new TipText(graphics, font, prms_list[i + 1]);
                                desc_param_sect.need_tab = true;
                                desc_param_sect.param_desc = true;
                                //TipSplitter spltr = new TipSplitter(graphics,true,prm_name_sect,desc_param_sect);
                                if (!string.IsNullOrEmpty(prms_list[i + 1]))
                                {
                                    prm_sections.Add(prm_name_sect);
                                    prm_sections.Add(desc_param_sect);
                                }
                                i += 2;
                            }
                            if (prm_sections.Count > 0)
                            {
                                sections.Add(new TipText(graphics, new Font(font.FontFamily, 3), " "));
                                sections.Add(paramsTextTipHeader);
                                sections.AddRange(prm_sections);
                            }
                        }
                        int return_ind = documentation.IndexOf("<returns>");
                        /*TipText returnTextTipHeader = null;
                        TipText returnTextTip = null;
                        if (return_ind != -1 && addit_info)
                        {
                            returnTextTipHeader = new TipText(graphics,new Font(font, FontStyle.Bold),PascalABCCompiler.StringResources.Get("CODE_COMPLETION_RETURN"));
                            returnTextTipHeader.is_head = true;
                            int end_return = documentation.IndexOf("<params>",return_ind);
                            string return_text;
                            if (end_return == -1)
                                return_text = documentation.Substring(return_ind+9).Trim(' ','\n','\r','\t');
                            else
                                return_text = documentation.Substring(return_ind+9,end_return-return_ind-9).Trim(' ','\n','\r','\t');
					
                            if (!string.IsNullOrEmpty(return_text))
                            {
                                returnTextTip = new TipText(graphics,font,return_text);
                                sections.Add(new TipText(graphics,new Font(font.FontFamily,3)," "));
                                sections.Add(returnTextTipHeader);
                                sections.Add(returnTextTip);
                                returnTextTip.need_tab = true;
                            }
                        }*/
                        if (params_ind != -1 && return_ind != -1)
                            documentation = documentation.Substring(0, Math.Min(params_ind, return_ind)).Trim(' ', '\n', '\t', '\r');
                        else
                            if (params_ind != -1)
                                documentation = documentation.Substring(0, params_ind).Trim(' ', '\n', '\t', '\r');
                            else
                                if (return_ind != -1)
                                    documentation = documentation.Substring(0, return_ind).Trim(' ', '\n', '\t', '\r');
                    }
                    catch
                    {

                    }
                }
                TipText docTip = new TipText(graphics, font, documentation);
                //docTip.is_doc = true;
                docTip.need_tab = true;
                sections.Insert(0, docTip);
                if (!string.IsNullOrEmpty(documentation))
                {
                    TipText descr_head = new TipText(graphics, new Font(font, FontStyle.Bold), PascalABCCompiler.StringResources.Get("CODE_COMPLETION_DESCRIPTION"));
                    descr_head.is_head = true;
                    sections.Insert(0, descr_head);
                    sections.Insert(0, new TipText(graphics, new Font(font.FontFamily, 1), " "));
                }
                //docTip.is_doc = true;
                docTip.need_tab = true;
                TipSplitter descSplitter = new TipSplitter(graphics, true,
                                                           descriptionTip,
                                                           docSpacer
                                                           );
                // Now put them together.
                /*TipSplitter descSplitter = new TipSplitter(graphics, true,
                                                           //descriptionTip,
                                                           descriptionTip1,
                                                           descriptionTip2,
                                                           descriptionTip3,
                                                           docSpacer
                                                           );*/
                descSplitter.is_desc = true;
                /*TipSplitter descSplitter1 = new TipSplitter(graphics, false,
                                                           //descriptionTip,
                                                           descriptionTip1
                                                           );
				
                TipSplitter descSplitter2 = new TipSplitter(graphics, false,
                                                           //descriptionTip,
                                                           descriptionTip2
                                                           );
				
                TipSplitter descSplitter3 = new TipSplitter(graphics, false,
                                                           //descriptionTip,
                                                           descriptionTip3,
                                                           docSpacer
                                                           );*/

                TipSplitter mainSplitter = new TipSplitter(graphics, true,
                                                           countMessageTip,
                                                           countSpacer,
                                                           descSplitter);

                sections.Insert(0, mainSplitter);
                TipSplitter mainSplitter2 = new TipSplitter(graphics, false,
                                                           sections.ToArray());

                // Show it.
                Size size = TipPainter.GetTipSize(control, graphics, mainSplitter2);
                DrawingRectangle1 = countMessageTip.DrawingRectangle1;
                DrawingRectangle2 = countMessageTip.DrawingRectangle2;
                return size;
            }
            return Size.Empty;
        }

        public static Size DrawHelpTip(Control control,
                                       Graphics graphics, Font font,
                                       string countMessage,
                                       string basicDescription,
                                       string documentation,
                                       int beg_bold, int bold_len, int param_num, bool addit_info)
        {
            if (IsVisibleText(countMessage) ||
                IsVisibleText(basicDescription) ||
                IsVisibleText(documentation))
            {
                // Create all the TipSection objects.
                CountTipText countMessageTip = new CountTipText(graphics, font, countMessage);

                TipSpacer countSpacer = new TipSpacer(graphics, new SizeF(IsVisibleText(countMessage) ? 4 : 0, 0));
				if (basicDescription.Length < 30)
					basicDescription = basicDescription.PadRight(30);
                TipText descriptionTip = new TipText(graphics, font, basicDescription);
                descriptionTip.insight_wnd = control is ICSharpCode.TextEditor.Gui.InsightWindow.PABCNETInsightWindow;
                descriptionTip.beg_bold = beg_bold;
                descriptionTip.len_bold = bold_len;
                TipText descriptionTip1 = new TipText(graphics, font, basicDescription.Substring(0, beg_bold));
                TipText descriptionTip2 = new TipText(graphics, new Font(font, FontStyle.Bold), basicDescription.Substring(beg_bold, bold_len));
                TipText descriptionTip3 = new TipText(graphics, font, basicDescription.Substring(beg_bold + bold_len));
                descriptionTip.desc1 = descriptionTip1;
                descriptionTip.desc2 = descriptionTip2;
                descriptionTip.desc3 = descriptionTip3;
                TipSpacer docSpacer = new TipSpacer(graphics, new SizeF(0, IsVisibleText(documentation) ? 4 : 0));
                List<TipSection> sections = new List<TipSection>();
                if (documentation != null)
                {
                    try
                    {

                        int params_ind = documentation.IndexOf("<params>");
                        TipText paramsTextTipHeader = null;
                        TipText paramsTextTip = null;
                        if (params_ind != -1 && addit_info)
                        {
                            paramsTextTipHeader = new TipText(graphics, new Font(font, FontStyle.Bold), PascalABCCompiler.StringResources.Get("CODE_COMPLETION_PARAMETERS"));
                            paramsTextTipHeader.is_head = true;
                            int end_param = documentation.IndexOf("<returns>", params_ind);
                            string params_text;
                            if (end_param == -1)
                            {
                                params_text = documentation.Substring(params_ind + 8).Trim(' ', '\n', '\r', '\t');
                                //paramsTextTip = new TipText(graphics,font,params_text);
                            }
                            else
                            {
                                params_text = documentation.Substring(params_ind + 8, end_param - params_ind - 8).Trim(' ', '\n', '\r', '\t');
                                //paramsTextTip = new TipText(graphics,font,documentation.Substring(params_ind+8,end_param-params_ind-8).Trim(' ','\n','\r','\t'));
                            }
                            List<string> prms_list = new List<string>();
                            int ind = params_text.IndexOf("<param>", 0);
                            while (ind != -1)
                            {
                                int end_ind = params_text.IndexOf("</param>", ind + 1);
                                prms_list.Add(params_text.Substring(ind + 7, end_ind - ind - 7).Trim(' ', '\n', '\t', '\r'));
                                ind = params_text.IndexOf("<param>", end_ind);
                                if (ind != -1)
                                {
                                    if (ind - end_ind - 7 > 0)
                                        prms_list.Add(params_text.Substring(end_ind + 8, ind - end_ind - 8).Trim(' ', '\n', '\t', '\r'));
                                    else
                                        prms_list.Add("");
                                }
                                else
                                {
                                    prms_list.Add(params_text.Substring(end_ind + 8).Trim(' ', '\n', '\t', '\r'));
                                }
                            }

                            List<TipSection> prm_sections = new List<TipSection>();
                            int i = 0;
                            while (i < prms_list.Count)
                            {
                                TipText prm_name_sect = new TipText(graphics, font, prms_list[i]);
                                prm_name_sect.param_name = true;
                                TipText desc_param_sect = new TipText(graphics, font, prms_list[i + 1]);
                                desc_param_sect.need_tab = true;
                                desc_param_sect.param_desc = true;
                                //TipSplitter spltr = new TipSplitter(graphics,true,prm_name_sect,desc_param_sect);
                                if (!string.IsNullOrEmpty(prms_list[i + 1]))
                                {
                                    prm_sections.Add(prm_name_sect);
                                    prm_sections.Add(desc_param_sect);
                                }
                                i += 2;
                            }
                            if (prm_sections.Count > 0)
                            {
                                sections.Add(new TipText(graphics, new Font(font.FontFamily, 3), " "));
                                sections.Add(paramsTextTipHeader);
                                sections.AddRange(prm_sections);
                            }
                        }
                        int return_ind = documentation.IndexOf("<returns>");
                        /*TipText returnTextTipHeader = null;
                        TipText returnTextTip = null;
                        if (return_ind != -1 && addit_info)
                        {
                            returnTextTipHeader = new TipText(graphics,new Font(font, FontStyle.Bold),PascalABCCompiler.StringResources.Get("CODE_COMPLETION_RETURN"));
                            returnTextTipHeader.is_head = true;
                            int end_return = documentation.IndexOf("<params>",return_ind);
                            string return_text;
                            if (end_return == -1)
                                return_text = documentation.Substring(return_ind+9).Trim(' ','\n','\r','\t');
                            else
                                return_text = documentation.Substring(return_ind+9,end_return-return_ind-9).Trim(' ','\n','\r','\t');
					
                            if (!string.IsNullOrEmpty(return_text))
                            {
                                returnTextTip = new TipText(graphics,font,return_text);
                                sections.Add(new TipText(graphics,new Font(font.FontFamily,3)," "));
                                sections.Add(returnTextTipHeader);
                                sections.Add(returnTextTip);
                                returnTextTip.need_tab = true;
                            }
                        }*/
                        if (params_ind != -1 && return_ind != -1)
                            documentation = documentation.Substring(0, Math.Min(params_ind, return_ind)).Trim(' ', '\n', '\t', '\r');
                        else
                            if (params_ind != -1)
                                documentation = documentation.Substring(0, params_ind).Trim(' ', '\n', '\t', '\r');
                            else
                                if (return_ind != -1)
                                    documentation = documentation.Substring(0, return_ind).Trim(' ', '\n', '\t', '\r');
                    }
                    catch
                    {

                    }
                }
				
                TipText docTip = new TipText(graphics, font, documentation);
                //docTip.is_doc = true;
                docTip.need_tab = true;
                sections.Insert(0, docTip);
                if (!string.IsNullOrEmpty(documentation))
                {
                    TipText descr_head = new TipText(graphics, new Font(font, FontStyle.Bold), PascalABCCompiler.StringResources.Get("CODE_COMPLETION_DESCRIPTION"));
                    descr_head.is_head = true;
                    sections.Insert(0, descr_head);
                    sections.Insert(0, new TipText(graphics, new Font(font.FontFamily, 1), " "));
                }
                TipSplitter descSplitter = new TipSplitter(graphics, true,
                                                           descriptionTip,
                                                           docSpacer
                                                           );
                // Now put them together.
                /*TipSplitter descSplitter = new TipSplitter(graphics,true,
                                                           descriptionTip1,
                                                           descriptionTip2,
                                                           descriptionTip3,
                                                           docSpacer
                                                           );*/
                descSplitter.is_desc = true;
                /*TipSplitter descSplitter1 = new TipSplitter(graphics, false,
                                                           //descriptionTip,
                                                           descriptionTip1
                                                           );
				
                TipSplitter descSplitter2 = new TipSplitter(graphics, false,
                                                           //descriptionTip,
                                                           descriptionTip2
                                                           );
				
                TipSplitter descSplitter3 = new TipSplitter(graphics, false,
                                                           //descriptionTip,
                                                           descriptionTip3,
                                                           docSpacer
                                                           );*/

                TipSplitter mainSplitter = new TipSplitter(graphics, true,
                                                           countMessageTip,
                                                           countSpacer,
                                                           descSplitter);

                sections.Insert(0, mainSplitter);
                TipSplitter mainSplitter2 = new TipSplitter(graphics, false,
                                                           sections.ToArray());
                // Show it.
                Size size = TipPainter.DrawTip(control, graphics, mainSplitter2);
                DrawingRectangle1 = countMessageTip.DrawingRectangle1;
                DrawingRectangle2 = countMessageTip.DrawingRectangle2;
                return size;
            }
            return Size.Empty;
        }
		
		static bool IsVisibleText(string text)
		{
			return text != null && text.Length > 0;
		}
    }
}
