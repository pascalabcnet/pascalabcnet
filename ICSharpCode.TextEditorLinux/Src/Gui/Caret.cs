// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 2683 $</version>
// </file>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

using ICSharpCode.TextEditor.Document;
using System.Reflection;
using System.Windows.Forms;

namespace ICSharpCode.TextEditor
{
	/// <summary>
	/// In this enumeration are all caret modes listed.
	/// </summary>
	public enum CaretMode {
		/// <summary>
		/// If the caret is in insert mode typed characters will be
		/// inserted at the caret position
		/// </summary>
		InsertMode,
		
		/// <summary>
		/// If the caret is in overwirte mode typed characters will
		/// overwrite the character at the caret position
		/// </summary>
		OverwriteMode
	}
	
	
	public class Caret : System.IDisposable
	{
		int       line          = 0;
		int       column        = 0;
		int       desiredXPos   = 0;
		CaretMode caretMode;
		
		static bool     caretCreated = false;
		bool     hidden       = true;
		TextArea textArea;
        static TextArea staticTextArea;
		Point    currentPos   = new Point(-1, -1);
		Ime      ime          = null;
		
		/// <value>
		/// The 'prefered' xPos in which the caret moves, when it is moved
		/// up/down.
		/// </value>
		public int DesiredColumn {
			get {
				return desiredXPos;
			}
			set {
				desiredXPos = value;
			}
		}
		
		/// <value>
		/// The current caret mode.
		/// </value>
		public CaretMode CaretMode {
			get {
				return caretMode;
			}
			set {
				caretMode = value;
				OnCaretModeChanged(EventArgs.Empty);
			}
		}
		
		public int Line {
			get {
				return line;
			}
			set {
				line = value;
				ValidateCaretPos();
				UpdateCaretPosition();
				OnPositionChanged(EventArgs.Empty);
			}
		}
		
		public int Column {
			get {
				return column;
			}
			set {
				column = value;
				ValidateCaretPos();
				UpdateCaretPosition();
				OnPositionChanged(EventArgs.Empty);
			}
		}
		
		public TextLocation Position {
			get {
				return new TextLocation(column, line);
			}
			set {
				line   = value.Y;
				column = value.X;
				ValidateCaretPos();
				UpdateCaretPosition();
				OnPositionChanged(EventArgs.Empty);
			}
		}
		
		public int Offset {
			get {
				return textArea.Document.PositionToOffset(Position);
			}
		}
		
		public Caret(TextArea textArea)
		{
			this.textArea = textArea;
            staticTextArea = textArea;
			textArea.GotFocus  += new EventHandler(GotFocus);
			textArea.LostFocus += new EventHandler(LostFocus);
		}
		
		public void Dispose()
		{
			textArea.GotFocus  -= new EventHandler(GotFocus);
			textArea.LostFocus -= new EventHandler(LostFocus);
			textArea = null;
            staticTextArea = null;
//			DestroyCaret();
//			caretCreated = false;
		}
		
		public TextLocation ValidatePosition(TextLocation pos)
		{
			int line   = Math.Max(0, Math.Min(textArea.Document.TotalNumberOfLines - 1, pos.Y));
			int column = Math.Max(0, pos.X);
			
			if (column == int.MaxValue || !textArea.TextEditorProperties.AllowCaretBeyondEOL) {
				LineSegment lineSegment = textArea.Document.GetLineSegment(line);
				column = Math.Min(column, lineSegment.Length);
			}
			return new TextLocation(column, line);
		}
		
		/// <remarks>
		/// If the caret position is outside the document text bounds
		/// it is set to the correct position by calling ValidateCaretPos.
		/// </remarks>
		public void ValidateCaretPos()
		{
			line = Math.Max(0, Math.Min(textArea.Document.TotalNumberOfLines - 1, line));
			column = Math.Max(0, column);
			
			if (column == int.MaxValue || !textArea.TextEditorProperties.AllowCaretBeyondEOL) {
				LineSegment lineSegment = textArea.Document.GetLineSegment(line);
				column = Math.Min(column, lineSegment.Length);
			}
		}
		
		void CreateCaret()
		{
			while (!caretCreated) {
				switch (caretMode) {
					case CaretMode.InsertMode:
						caretCreated = CreateCaret(textArea.Handle, 0, 2, textArea.TextView.FontHeight);
						break;
					case CaretMode.OverwriteMode:
						caretCreated = CreateCaret(textArea.Handle, 0, (int)textArea.TextView.SpaceWidth, textArea.TextView.FontHeight);
						break;
				}
			}
			if (currentPos.X  < 0) {
				ValidateCaretPos();
				currentPos = ScreenPosition;
			}
			SetCaretPos(currentPos.X, currentPos.Y);
			ShowCaret(textArea.Handle);
		}
		
		public void RecreateCaret()
		{
			DisposeCaret();
			if (!hidden) {
				CreateCaret();
			}
		}
		
		void DisposeCaret()
		{
			caretCreated = false;
            //if (Environment.OSVersion.Platform != PlatformID.Unix && Environment.OSVersion.Platform != PlatformID.MacOSX)
			HideCaret(textArea.Handle);
			DestroyCaret();
		}
		
		void GotFocus(object sender, EventArgs e)
		{
			hidden = false;
			if (!textArea.MotherTextEditorControl.IsInUpdate) {
				CreateCaret();
				UpdateCaretPosition();
			}
		}
		
		void LostFocus(object sender, EventArgs e)
		{
			hidden       = true;
			DisposeCaret();
		}
		
		public Point ScreenPosition {
			get {
				int xpos = textArea.TextView.GetDrawingXPos(this.line, this.column);
				return new Point(textArea.TextView.DrawingPosition.X + xpos,
				                 textArea.TextView.DrawingPosition.Y
				                 + (textArea.Document.GetVisibleLine(this.line)) * textArea.TextView.FontHeight
				                 - textArea.TextView.TextArea.VirtualTop.Y);
			}
		}
		int oldLine = -1;
		public void UpdateCaretPosition()
		{
            //Console.WriteLine("updatecaret " + ScreenPosition.X+"-"+ScreenPosition.Y+"-"+oldLine+"-"+line);
            if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                if (oldLine != line)
                {
                    //Console.WriteLine(oldLine + "-" + line);
                    textArea.UpdateLine(oldLine);
                    //textArea.UpdateLine(line);
                }
            }
            else
            {
                if (textArea.MotherTextAreaControl.TextEditorProperties.LineViewerStyle == LineViewerStyle.FullRow && oldLine != line)
                {
                    textArea.UpdateLine(oldLine);
                    textArea.UpdateLine(line);
                }
            }

           
			oldLine = line;
			
			
			if (hidden || textArea.MotherTextEditorControl.IsInUpdate) {
				return;
			}

            //DisposeCaret();
            //Console.WriteLine("updatecaret " + ScreenPosition.X + "-" + ScreenPosition.Y + "-" + oldLine + "-" + line);
            
			if (!caretCreated) {
				CreateCaret();
			}
			if (caretCreated) {
				ValidateCaretPos();
				int lineNr = this.line;
				int xpos = textArea.TextView.GetDrawingXPos(lineNr, this.column);
				//LineSegment lineSegment = textArea.Document.GetLineSegment(lineNr);
				Point pos = ScreenPosition;
				if (xpos >= 0) {
                   
					bool success = SetCaretPos(pos.X, pos.Y);
                    if (!success)
                    {
                        DestroyCaret();
                        caretCreated = false;
                        UpdateCaretPosition();
                    }
                    
				}
				// set the input method editor location
				/*if (ime == null) {
					ime = new Ime(textArea.Handle, textArea.Document.TextEditorProperties.Font);
				} else {
					ime.HWnd = textArea.Handle;
					ime.Font = textArea.Document.TextEditorProperties.Font;
				}
				ime.SetIMEWindowLocation(pos.X, pos.Y);*/
				
				currentPos = pos;
			}
		}
		
		#region Native caret functions
		/*[DllImport("User32.dll")]
		static extern bool CreateCaret(IntPtr hWnd, int hBitmap, int nWidth, int nHeight);
		
		[DllImport("User32.dll")]
		static extern bool SetCaretPos(int x, int y);
		
		[DllImport("User32.dll")]
		static extern bool DestroyCaret();
		
		[DllImport("User32.dll")]
		static extern bool ShowCaret(IntPtr hWnd);
		
		[DllImport("User32.dll")]
		static extern bool HideCaret(IntPtr hWnd);*/
		
		[DllImport("User32.dll", EntryPoint="CreateCaret")]
		static extern bool CreateCaretNative(IntPtr hWnd, int hBitmap, int nWidth, int nHeight);
		
		[DllImport("User32.dll", EntryPoint="SetCaretPos")]
		static extern bool SetCaretPosNative(int x, int y);
		
		[DllImport("User32.dll", EntryPoint="DestroyCaret")]
		static extern bool DestroyCaretNative();
		
		[DllImport("User32.dll", EntryPoint="ShowCaret")]
		static extern bool ShowCaretNative(IntPtr hWnd);
		
		[DllImport("User32.dll", EntryPoint="HideCaret")]
		static extern bool HideCaretNative(IntPtr hWnd);
		
		static int caret_X;
		static int caret_Y;
		static int caret_width;
		static int caret_height;
        static int cur_line;
		static IntPtr hwnd;
		static bool visible;
		static System.Windows.Forms.Timer timer;
		
		static bool CreateCaret(IntPtr hWnd, int hBitmap, int nWidth, int nHeight)
		{
			if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
			{
				caret_width = nWidth;
				caret_height = nHeight;
				hwnd = hWnd;
                if (create_caret == null)
                {
                    create_caret = get_xplat().GetMethod("CreateCaret", BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
                }
                create_caret.Invoke(null, new object[] { hWnd, nWidth, nHeight });
                return true;
			}
			else
				return CreateCaretNative(hWnd, hBitmap, nWidth, nHeight);
		}
		
		static bool DestroyCaret()
		{
			if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
			{
				/*if (timer != null && timer.Enabled)
				{
					timer.Stop();
					visible = false;
				}*/

                if (destroy_caret == null)
                {
                    destroy_caret = get_xplat().GetMethod("DestroyCaret", BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
                }
                destroy_caret.Invoke(null, new object[] { hwnd });
				
				return true;
			}
			else
				return DestroyCaretNative();
		}

        static MethodInfo set_caret_pos = null;
        static MethodInfo create_caret = null;
        static MethodInfo destroy_caret = null;
        static MethodInfo caret_visible = null;
        static Type xplat = null;

		static bool SetCaretPos(int x, int y)
		{
			if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
			{
                
                /*if (caret_X != x || caret_Y != y)
                {
                    HideCaret(hwnd);
                    caret_X = x;
                    caret_Y = y;

                    ShowCaret(hwnd);
                }
                else
                    ShowCaret(hwnd);
				return true;*/
               
                if (set_caret_pos == null)
                {
                    set_caret_pos = get_xplat().GetMethod("SetCaretPos", BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
                }
                set_caret_pos.Invoke(null, new object[] { hwnd, x, y});
                return true;
			}
			else
				return SetCaretPosNative(x, y);
		}
		
		static bool ShowCaret(IntPtr hWnd)
		{
			if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
			{
                
				/*if (timer != null && timer.Enabled)
				{
					timer.Stop();
				}
                
                if (timer == null)
                {
                    timer = new System.Windows.Forms.Timer();
                    timer.Interval = 500;
                    visible = false;
                   _ShowCaret(hwnd);
                   visible = true;
                   timer.Tick += delegate { if (!visible) _ShowCaret(hwnd); else _HideCaret(hwnd); };
                }
                visible = false;
				timer.Start();*/
                if (caret_visible == null)
                {
                    caret_visible = get_xplat().GetMethod("CaretVisible", BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
                }
                caret_visible.Invoke(null, new object[] { hWnd, true });
                return true;
				
			}
			else
				return ShowCaretNative(hWnd);
		}

        static Type get_xplat()
        {
            if (xplat != null)
                return xplat;
            Type[] types = typeof(Form).Assembly.GetTypes();
            foreach (Type t in types)
            {
                if (t.FullName == "System.Windows.Forms.XplatUI")
                {
                    xplat = t;
                    return t;
                }
            }
            return null;
        }

		static bool HideCaret(IntPtr hWnd)
		{
            if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                /*if (timer != null && timer.Enabled)
                {
                    timer.Stop();
                }
                visible = false;
                //if (staticTextArea != null)
                //   staticTextArea.UpdateLine(staticTextArea.TextView.GetLogicalLine(caret_Y));
                Graphics gr = Graphics.FromHwnd(hWnd);
                //gr.DrawRectangle(new Pen(Color.Black,caret_width),0,0,10,10);
                gr.DrawLine(new Pen(Color.White, caret_width), caret_X, caret_Y, caret_X, caret_Y + caret_height);
                gr.Dispose();*/
                if (caret_visible == null)
                {
                   
                    caret_visible = get_xplat().GetMethod("CaretVisible", BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
                }
                caret_visible.Invoke(null, new object[] { hWnd, false });
                return true;
            }
            else
                return HideCaretNative(hWnd);
		}
		
		static void _ShowCaret(IntPtr hWnd)
		{
            visible = true;
			Graphics gr = Graphics.FromHwnd(hWnd);
			//gr.DrawRectangle(new Pen(Color.Black,caret_width),0,0,10,10);
			gr.DrawLine(new Pen(Color.Black,caret_width),caret_X,caret_Y,caret_X,caret_Y+caret_height);
			gr.Dispose();
            
		}
		
		static void _HideCaret(IntPtr hWnd)
		{
            /*if (timer != null && timer.Enabled)
            {
                timer.Stop();
            }*/
            visible = false;
            Graphics gr = Graphics.FromHwnd(hWnd);
            //gr.DrawRectangle(new Pen(Color.Black,caret_width),0,0,10,10);
            gr.DrawLine(new Pen(Color.White, caret_width), caret_X, caret_Y, caret_X, caret_Y + caret_height);
            gr.Dispose();
            //staticTextArea.UpdateLine(staticTextArea.TextView.GetLogicalLine(caret_Y));
               
		}
		
		
		#endregion
		
		bool firePositionChangedAfterUpdateEnd;
		
		void FirePositionChangedAfterUpdateEnd(object sender, EventArgs e)
		{
			OnPositionChanged(EventArgs.Empty);
		}
		
		protected virtual void OnPositionChanged(EventArgs e)
		{
			if (this.textArea.MotherTextEditorControl.IsInUpdate) {
				if (firePositionChangedAfterUpdateEnd == false) {
					firePositionChangedAfterUpdateEnd = true;
					this.textArea.Document.UpdateCommited += FirePositionChangedAfterUpdateEnd;
				}
				return;
			} else if (firePositionChangedAfterUpdateEnd) {
				this.textArea.Document.UpdateCommited -= FirePositionChangedAfterUpdateEnd;
				firePositionChangedAfterUpdateEnd = false;
			}
			
			List<FoldMarker> foldings = textArea.Document.FoldingManager.GetFoldingsFromPosition(line, column);
			bool  shouldUpdate = false;
			foreach (FoldMarker foldMarker in foldings) {
				shouldUpdate |= foldMarker.IsFolded;
				foldMarker.IsFolded = false;
			}
			
			if (shouldUpdate) {
				textArea.Document.FoldingManager.NotifyFoldingsChanged(EventArgs.Empty);
			}
			
			if (PositionChanged != null) {
				PositionChanged(this, e);
			}
			textArea.ScrollToCaret();
		}
		
		protected virtual void OnCaretModeChanged(EventArgs e)
		{
			if (CaretModeChanged != null) {
				CaretModeChanged(this, e);
			}
			HideCaret(textArea.Handle);
			DestroyCaret();
			caretCreated = false;
			CreateCaret();
			ShowCaret(textArea.Handle);
		}
		
		/// <remarks>
		/// Is called each time the caret is moved.
		/// </remarks>
		public event EventHandler PositionChanged;
		
		/// <remarks>
		/// Is called each time the CaretMode has changed.
		/// </remarks>
		public event EventHandler CaretModeChanged;
	}
}
