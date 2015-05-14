using System;
using System.IO;
using System.Collections;
using com.calitha.goldparser;
using com.calitha.commons;

namespace grmCommentCompiler
{
	
	class Class1
	{
		public static MyParser parser;

		static string key_NAME="%NAME%";
		static string key_CODE="%CODE%";
		static string key_PARAMS="%PARAMS%";
		static string key_TEXT="%TEXT%";
		static string key_LEFTTOKEN="%LEFTTOKEN%";
		static string key_RIGHTTOKEN="%RIGHTTOKEN%";
		static int maxnontemtempl=9;
		
		static string ExtractTemplateFromComment(string Comments,string TemplateName)
		{
			int pos=Comments.IndexOf(TemplateName);
			if (pos==-1) return "";
			int endpos=parser.Comments.IndexOf("*!",pos);
			pos+=TemplateName.Length;
			return Comments.Substring(pos,endpos-pos);
		}
		static string CompileTemplateForTerminal(string Template,string cstext)
		{//<%NAME%(...)> %CODE%
			int pos=cstext.IndexOf('<')+1;
            string prms = "LRParser.TokenText";
			//string prms="parser.TokenText";
			
			if (pos!=0)
			{
				string name=cstext.Substring(pos,cstext.IndexOf('>')-pos);
				pos=cstext.IndexOf('(');
				if ((pos!=-1)&(cstext.IndexOf('>')>pos)) 
				{
					prms=name.Substring(pos,name.Length-pos-1);
					name=name.Substring(0,pos-1);
				}
				pos=cstext.IndexOf('>')+1;
				string code=cstext.Substring(pos,cstext.Length-pos);
				Template=Template.Replace(key_NAME,name);
				Template=Template.Replace(key_CODE,code);
				Template=Template.Replace(key_PARAMS,prms);
                Template = Template.Replace(key_TEXT, "LRParser.TokenText");
				Template=Template.Replace("$$=","return ");
				Template=Template.Replace("$$","_"+name);
			}
			else
			{
				Template=cstext;
			}
			Template=Template.Replace("$$=","return ");
            Template = Template.Replace("$$", "LRParser.TokenText");
			return Template;
		}
		//...key=value;...
		static string getParamSet(string text,string key)
		{
			key+="=";
			int bg=text.IndexOf(key);
			if (bg==-1) return null;
			int ed=text.IndexOf(";",bg+1);
			bg+=key.Length;
			string s=text.Substring(bg,ed-bg);
			return s;
		}
		static string CompileTemplateForNonTerminal(string[] TemplateArr,string cstext)
		{
			string s,name="",prms="",code=cstext,Template;
			int bg=cstext.IndexOf('<'),ed;
			if ((bg!=-1)&(cstext[0]!=' '))//n%NAME%<%PARAMS%> %CODE%
			{
				int n=Convert.ToInt32(cstext.Substring(0,1));
				cstext=cstext.Substring(1,cstext.Length-1);
				bg=cstext.IndexOf('<');ed=cstext.IndexOf(">");
				Template=TemplateArr[n];
				name=cstext.Substring(0,bg);
				prms=cstext.Substring(bg+1,ed-bg-1);
				code="";
				if (ed<cstext.Length-1)
					code=cstext.Substring(ed+1,cstext.Length-ed-1);
				Template=Template.Replace(key_CODE,code);
				Template=Template.Replace(key_NAME,name);
				Template=Template.Replace(key_PARAMS,prms);
				Template=Template.Replace("$$=","_"+name+"=");
				code=Template;
			}
			int mxdol=20,lt=mxdol,rt=-1;
			//$$=
			code=code.Replace("$$=","return ");
			//$$
			if (bg!=-1) 
				code=code.Replace("$$","_"+name);
			else 
				//code=code.Replace("$$","token.UserObject");
				code=code.Replace("$$","LRParser.TokenSyntaxNode");
				
			//$n
			for (int i=mxdol;i>0;i--)
			{
				s="$"+i;
				if (code.IndexOf(s)!=-1) 
				{
					if (rt==-1) rt=i;
					if (i<lt) lt=i;
					//code=code.Replace(s,"token.Tokens["+Convert.ToString(i-1)+"].UserObject");
					code=code.Replace(s,"LRParser.GetReductionSyntaxNode("+Convert.ToString(i-1)+")");
				}
			}
			// %LEFTTOKEN% %RIGHTTOKEN%
			s=getParamSet(cstext,key_LEFTTOKEN);
			if (s!=null) lt=Convert.ToInt32(s);
			s=getParamSet(cstext,key_RIGHTTOKEN);
			if (s!=null) rt=Convert.ToInt32(s);
			//code=code.Replace(key_LEFTTOKEN,"token.Tokens["+Convert.ToString(lt-1)+"].UserObject");
		    //code=code.Replace(key_RIGHTTOKEN,"token.Tokens["+Convert.ToString(rt-1)+"].UserObject");			
			code=code.Replace(key_LEFTTOKEN,"LRParser.GetReductionSyntaxNode("+Convert.ToString(lt-1)+")");
            code = code.Replace(key_RIGHTTOKEN, "LRParser.GetReductionSyntaxNode(" + Convert.ToString(rt - 1) + ")");			
			return code;
		}
		static void Main(string[] args)
		{	
			if (args.Length==0)
			{
				args=new string[3];
                Console.WriteLine("args: grammar.grm template.tmpl result.cs");
                return;
				args[0]="pascalabc.grm";args[1]="PABCParserTemplate.cs";args[2]="PABCParser.cs";
			}

			parser=new MyParser(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName)+"\\grmCommentCompiler.cgt");
			StreamReader sr=File.OpenText(args[0]);
			Console.Write("Parsing file "+args[0]+"...");
			string s=sr.ReadToEnd();
			if (parser.Parse(s)==null) return;
			Console.WriteLine("OK");

			string TerminalTemplate=ExtractTemplateFromComment(parser.Comments,"[TERMINALTEMPLATE]");
			if (TerminalTemplate=="")
			{
				Console.WriteLine("Error: template [TERMINALTEMPLATE] not defined");
				return;
			}
			//Console.WriteLine(TerminalTemplate);
			
			string[] NonTerminalTemplate=new string[maxnontemtempl+1];
			for (int i=0;i<=maxnontemtempl;i++)
				NonTerminalTemplate[i]=ExtractTemplateFromComment(parser.Comments,"[NONTERMINALTEMPLATE"+i+"]");
			if (NonTerminalTemplate[0]=="")
			{
				Console.WriteLine("Error: template [NONTERMINALTEMPLATE0] not defined");
				return;
			}
			//Console.WriteLine(NonTerminalTemplate);
			
			string csterminaldef="//TERMINAL:";
			string csterminalend="//ENDTERMINAL";
			string csnonterminaldef="//NONTERMINAL:";
			string csnonterminalend="//ENDNONTERMINAL";
			StreamReader csfile=File.OpenText(args[1]);
			StreamWriter outfile=new StreamWriter(args[2]);
			string grmname,cstext;
			int bg,ed,tc=0,ntc=0;
			while(csfile.Peek()>=0)
			{
				s=csfile.ReadLine();
				if (s.IndexOf(csterminaldef)!=-1)
				{
					bg=s.IndexOf(':')+1;
					grmname=s.Substring(bg,s.Length-bg);
					cstext=parser.TerminalsDefs[grmname];
					if (cstext!=null)
					{
						while(s.IndexOf(csterminalend)==-1) s=csfile.ReadLine();
						s=CompileTemplateForTerminal(TerminalTemplate,cstext);
						tc++;
					}
				}
				else
				{
					if (s.IndexOf(csnonterminaldef)!=-1)
					{
						bg=s.IndexOf(':')+1;
						grmname=s.Substring(bg,s.Length-bg);
						grmname=grmname.Replace("'","");
						cstext=parser.TerminalsDefs[grmname];
						if (cstext!=null)
						{
							while(s.IndexOf(csnonterminalend)==-1) s=csfile.ReadLine();
							s=CompileTemplateForNonTerminal(NonTerminalTemplate,cstext);
							//debug
							//s="Console.WriteLine(\""+grmname+"\");"+s;

							ntc++;
							//Console.WriteLine(s);
						}
					}
				}

				outfile.WriteLine(s);
			}
			csfile.Close();
			outfile.Flush();
			outfile.Close();
			Console.WriteLine("Changed: "+tc+" terminals, "+ntc+" nonteminals");


		}
	}
}
