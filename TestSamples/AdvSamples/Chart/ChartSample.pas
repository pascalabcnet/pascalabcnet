#reference 'System.Windows.Forms.dll'
#reference 'System.Drawing.dll'
#reference 'ZedGraph.dll'
#apptype windows

uses System, System.Windows.Forms, System.Drawing, System.Drawing.Drawing2D, ZedGraph;

type 
ContextMenuObjectState = (InactiveSelection,ActiveSelection,Background);
Form1 = class(Form)
zg1 : ZedGraph.ZedGraphControl;
constructor;
begin
InitializeComponent;
end;

procedure Form1_Load(sender : object; e : EventArgs);
begin
      var myPane := zg1.GraphPane;

			// Set the titles and axis labels
			myPane.Title.Text := 'Demonstration of Dual Y Graph';
			myPane.XAxis.Title.Text := 'Time, Days';
			myPane.YAxis.Title.Text := 'Parameter A';
			myPane.Y2Axis.Title.Text := 'Parameter B';

			// Make up some data points based on the Sine function
			var list := new PointPairList();
			var list2 := new PointPairList();
      var list3 := new PointPairList();
			for var i := 0 to 35 do
			begin
				var x : real := i * 5.0;
				var y : real := sin( real(i) * pi / 15.0 ) * 16.0;
				var y2 : real := y * 13.5;
				list.Add(x, y );
				list2.Add(x, y2 );
        list3.Add(x, y);
			end;
            
      myPane.AddHiLowBar('Bar', list3, Color.Yellow);
			// Generate a red curve with diamond symbols, and "Alpha" in the legend
			var myCurve : LineItem := myPane.AddCurve('Alpha',list, Color.Red, SymbolType.Diamond);
			// Fill the symbols with white
			myCurve.Symbol.Fill := new Fill( Color.White );

			// Generate a blue curve with circle symbols, and "Beta" in the legend
			myCurve := myPane.AddCurve( 'Beta',
				list2, Color.Blue, SymbolType.Circle );
			// Fill the symbols with white
			myCurve.Symbol.Fill := new Fill( Color.White );
			// Associate this curve with the Y2 axis
			myCurve.IsY2Axis := true;

			// Show the x axis grid
			myPane.XAxis.MajorGrid.IsVisible := true;

			// Make the Y axis scale red
			myPane.YAxis.Scale.FontSpec.FontColor := Color.Red;
			myPane.YAxis.Title.FontSpec.FontColor := Color.Red;
			// turn off the opposite tics so the Y tics don't show up on the Y2 axis
			myPane.YAxis.MajorTic.IsOpposite := false;
			myPane.YAxis.MinorTic.IsOpposite := false;
			// Don't display the Y zero line
			myPane.YAxis.MajorGrid.IsZeroLine := false;
			// Align the Y axis labels so they are flush to the axis
			myPane.YAxis.Scale.Align := AlignP.Inside;
			// Manually set the axis range
			myPane.YAxis.Scale.Min := -30;
			myPane.YAxis.Scale.Max := 30;

			// Enable the Y2 axis display
			myPane.Y2Axis.IsVisible := true;
			// Make the Y2 axis scale blue
			myPane.Y2Axis.Scale.FontSpec.FontColor := Color.Blue;
			myPane.Y2Axis.Title.FontSpec.FontColor := Color.Blue;
			// turn off the opposite tics so the Y2 tics don't show up on the Y axis
			myPane.Y2Axis.MajorTic.IsOpposite := false;
			myPane.Y2Axis.MinorTic.IsOpposite := false;
			// Display the Y2 axis grid lines
			myPane.Y2Axis.MajorGrid.IsVisible := true;
			// Align the Y2 axis labels so they are flush to the axis
			myPane.Y2Axis.Scale.Align := AlignP.Inside;

			// Fill the axis background with a gradient
			myPane.Chart.Fill := new Fill( Color.White, Color.LightGray, 45.0{f} );

			// Add a text box with instructions
			var text := new TextObj(
				'Zoom: left mouse & drag\nPan: middle mouse & drag\nContext Menu: right mouse',
				0.05{f}, 0.95{f}, CoordType.ChartFraction, AlignH.Left, AlignV.Bottom );
			text.FontSpec.StringAlignment := StringAlignment.Near;
			myPane.GraphObjList.Add(text);

			// Enable scrollbars if needed
			zg1.IsShowHScrollBar := true;
			zg1.IsShowVScrollBar := true;
			zg1.IsAutoScrollRange := true;
			zg1.IsScrollY2 := true;

			// OPTIONAL: Show tooltips when the mouse hovers over a point
			zg1.IsShowPointValues := true;
			zg1.PointValueEvent += MyPointValueHandler;

			// OPTIONAL: Add a custom context menu item
			zg1.ContextMenuBuilder += MyContextMenuBuilder;

			// OPTIONAL: Handle the Zoom Event
			zg1.ZoomEvent += MyZoomEvent;

			// Size the control to fit the window
			SetSize();

			// Tell ZedGraph to calculate the axis ranges
			// Note that you MUST call this after enabling IsAutoScrollRange, since AxisChange() sets
			// up the proper scrolling parameters
			zg1.AxisChange();
			// Make sure the Graph gets redrawn
			zg1.Invalidate();
end;

procedure SetSize;
begin
  zg1.Location := new Point( 10, 10 );
			// Leave a small margin around the outside of the control
	zg1.Size := new System.Drawing.Size(self.ClientRectangle.Width - 20,self.ClientRectangle.Height - 20 );
end;

function MyPointValueHandler(control : ZedGraphControl; pane : GraphPane; curve : CurveItem; iPt: integer) : string;
begin
  var pt : PointPair := curve[iPt];
  Result := curve.Label.Text + ' is ' + pt.Y.ToString('f2') + ' units at ' + pt.X.ToString('f1') + ' days';
end;

procedure MyContextMenuBuilder(control: ZedGraphControl; menuStrip: System.Windows.Forms.ContextMenuStrip;
						mousePt: Point; objState: ZedGraphControl.ContextMenuObjectState);
begin
	var item := new ToolStripMenuItem();
	item.Name := 'add-beta';
	item.Tag := 'add-beta';
	item.Text := 'Add a new Beta Point';
	item.Click += AddBetaPoint;
  menuStrip.Items.Add(item);
end;

procedure AddBetaPoint(sender : object; args: EventArgs);
begin
  // Get a reference to the "Beta" curve IPointListEdit
	var ip : IPointListEdit; 
	ip := (zg1.GraphPane.CurveList['Beta'].Points as IPointListEdit);
	if ip <> nil then
	begin
		var x : real := ip.Count * 5.0;
		var y : real := sin( ip.Count * pi / 15.0 ) * 16.0 * 13.5;
		ip.Add(x, y);
		zg1.AxisChange();
		zg1.Refresh();
	end;
end;

procedure MyZoomEvent(control: ZedGraphControl; oldState: ZoomState; newState: ZoomState);
begin
end;

procedure Form1_Resize(sender : object; e : EventArgs);
begin
  SetSize;
end;

procedure InitializeComponent;
begin
      self.zg1 := new ZedGraph.ZedGraphControl();
			self.SuspendLayout();
			// 
			// zg1
			// 
			self.zg1.EditButtons := System.Windows.Forms.MouseButtons.Left;
			self.zg1.EditModifierKeys := System.Windows.Forms.Keys( System.Windows.Forms.Keys.Alt or System.Windows.Forms.Keys.None);
			self.zg1.IsAutoScrollRange := false;
			self.zg1.IsEnableHEdit := false;
			self.zg1.IsEnableHPan := true;
			self.zg1.IsEnableHZoom := true;
			self.zg1.IsEnableVEdit := false;
			self.zg1.IsEnableVPan := true;
			self.zg1.IsEnableVZoom := true;
			self.zg1.IsPrintFillPage := true;
			self.zg1.IsPrintKeepAspectRatio := true;
			self.zg1.IsScrollY2 := false;
			self.zg1.IsShowContextMenu := true;
			self.zg1.IsShowCopyMessage := true;
			self.zg1.IsShowCursorValues := false;
			self.zg1.IsShowHScrollBar := false;
			self.zg1.IsShowPointValues := false;
			self.zg1.IsShowVScrollBar := false;
			self.zg1.IsSynchronizeXAxes := false;
			self.zg1.IsSynchronizeYAxes := false;
			self.zg1.IsZoomOnMouseCenter := false;
			self.zg1.LinkButtons := System.Windows.Forms.MouseButtons.Left;
			self.zg1.LinkModifierKeys := System.Windows.Forms.Keys( System.Windows.Forms.Keys.Alt or System.Windows.Forms.Keys.None );
			self.zg1.Location := new System.Drawing.Point( 12, 12 );
			self.zg1.Name := 'zg1';
			self.zg1.PanButtons := System.Windows.Forms.MouseButtons.Left;
			self.zg1.PanButtons2 := System.Windows.Forms.MouseButtons.Middle;
			self.zg1.PanModifierKeys := System.Windows.Forms.Keys( System.Windows.Forms.Keys.Shift or System.Windows.Forms.Keys.None );
			self.zg1.PanModifierKeys2 := System.Windows.Forms.Keys.None;
			self.zg1.PointDateFormat := 'g';
			self.zg1.PointValueFormat := 'G';
			self.zg1.ScrollMaxX := 0;
			self.zg1.ScrollMaxY := 0;
			self.zg1.ScrollMaxY2 := 0;
			self.zg1.ScrollMinX := 0;
			self.zg1.ScrollMinY := 0;
			self.zg1.ScrollMinY2 := 0;
			self.zg1.Size := new System.Drawing.Size( 499, 333 );
			self.zg1.TabIndex := 0;
			self.zg1.ZoomButtons := System.Windows.Forms.MouseButtons.Left;
			self.zg1.ZoomButtons2 := System.Windows.Forms.MouseButtons.None;
			self.zg1.ZoomModifierKeys := System.Windows.Forms.Keys.None;
			self.zg1.ZoomModifierKeys2 := System.Windows.Forms.Keys.None;
			self.zg1.ZoomStepFraction := 0.1;
			// 
			// Form1
			// 
			self.AutoScaleDimensions := new System.Drawing.SizeF(6{F},13{F});
			self.AutoScaleMode := System.Windows.Forms.AutoScaleMode.Font;
			self.ClientSize := new System.Drawing.Size( 523, 357 );
			self.Controls.Add(self.zg1);
			self.Name := 'Form1';
			self.Text := 'Form1';
			self.Resize += self.Form1_Resize;
			self.Load += self.Form1_Load;
			self.ResumeLayout(false);
end;

end;

begin
var f := new Form1();
Application.Run(f);
end.