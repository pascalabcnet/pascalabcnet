{$reference %GAC%\InteractiveDataDisplay.WPF.dll}
/// Модуль для визуализации данных
unit PlotWPF;

uses GraphWPF;
uses GraphWPFBase;
uses Controls;

uses InteractiveDataDisplay.WPF;
uses System.Windows.Controls;
uses System.Windows.Media;
uses System.Windows;


function RGB(r,g,b: byte) := Color.Fromrgb(r, g, b);
function ARGB(a,r,g,b: byte) := Color.FromArgb(a, r, g, b);
function GrayColor(b: byte): Color := RGB(b, b, b);
function RandomColor := RGB(PABCSystem.Random(256), PABCSystem.Random(256), PABCSystem.Random(256));
function EmptyColor: Color := ARGB(0,0,0,0);
function Pnt(x,y: real) := new Point(x,y);
function Rect(x,y,w,h: real) := new System.Windows.Rect(x,y,w,h);

type
  GridWPF = Controls.GridWPF;
  Colors = System.Windows.Media.Colors;
  MarkerType = (Circle,Box,Triangle,Diamond,Cross);
  MarkerTyp = MarkerType;
  
  ChartGr = class
  private  
    linegr: PlotBase;
    constructor (linegr: PlotBase);
    begin
      Self.linegr := linegr;
    end;
  public 
    procedure ChangeData(a,b: real; f: real -> real);
    begin
      Invoke(()-> begin
        var xx := PartitionPoints(a,b,200);
        var yy := xx.Select(f);
        ChangeData(xx,yy)
      end
      );
    end;
    procedure ChangeData(xx,yy: sequence of real);
    begin
      Invoke(()-> begin
        match linegr with
          LineGraph(lg): lg.Plot(xx,yy);
          CircleMarkerGraph(mg): mg.Plot(xx,yy);
        end;
        end
      );
    end;
    property Color: GColor 
      read Invoke&<GColor>(()->begin 
        if linegr is LineGraph(var lg) then
          Result := (lg.Stroke as SolidColorBrush).Color
        else if linegr is CircleMarkerGraph(var mg) then
          Result := (mg.Color as SolidColorBrush).Color
      end)
      write Invoke(procedure -> begin
        match linegr with
          LineGraph(lg): lg.Stroke := new SolidColorBrush(value);
          CircleMarkerGraph(mg): mg.Color := new SolidColorBrush(value);
        end;
      end);  
    property Thickness: real 
      write Invoke(procedure -> begin
        match linegr with
          LineGraph(lg): lg.StrokeThickness := value;
          MarkerGraph(mg): mg.StrokeThickness := value;
        end;
      end);
    property MarkerType: PlotWPF.MarkerType
      write Invoke(procedure -> begin
        match linegr with
          CircleMarkerGraph(mg): 
            case value of
          MarkerTyp.Circle:   mg.MarkerType := new CircleMarker();
          MarkerTyp.Box:      mg.MarkerType := new BoxMarker();
          MarkerTyp.Triangle: mg.MarkerType := new TriangleMarker();
          MarkerTyp.Diamond:  mg.MarkerType := new DiamondMarker();
          MarkerTyp.Cross:    mg.MarkerType := new CrossMarker();
            end;
        end;
      end);
  end;

  BaseGraphWPF = class(CommonControlWPF)
  private
    grd: Grid;
    property chrt: Chart read element as Chart;
    procedure Create0;
    begin
      var c := new Chart();
      c.LegendVisibility := Visibility.Hidden;
      grd := new Grid;
      c.Content := grd;
      element := c;
    end;
    procedure CreateLineP(a,b: real; f: real -> real; color: GColor);
    begin
      Create0;
      AddLineGraph(a,b,f,color);
      Init0(element,0,-1,-1);
    end;
    procedure CreateLineP1(xx,yy: sequence of real; color: GColor);
    begin
      Create0;
      AddLineGraph(xx,yy,color);
      Init0(element,0,-1,-1);
    end;
    procedure CreateMarkerP1(xx,yy: sequence of real; color: GColor; MType: MarkerType; size: real);
    begin
      Create0;
      AddMarkerGraph(xx,yy,color,MType,size);
      Init0(element,0,-1,-1);
    end;
  public  
    property Title: string read InvokeString(()->chrt.Title.ToString) 
      write Invoke(procedure -> chrt.Title := value);
    property PlotRect: GRect 
      read Invoke&<GRect>(()->GraphWPF.Rect(chrt.PlotOriginX,chrt.PlotOriginY,chrt.PlotWidth,chrt.PlotHeight))
      write Invoke(procedure -> begin
        chrt.PlotOriginX := value.x;
        chrt.PlotOriginY := value.y;
        chrt.PlotWidth := value.width;
        chrt.PlotHeight := value.height;
      end);
    property Graph[i: integer]: ChartGr 
      read new ChartGr(Invoke&<PlotBase>(()->grd.Children[i] as PlotBase));
    procedure AddLineGraph(xx,yy: sequence of real; color: GColor := Colors.Red);
    begin
      Invoke(procedure -> begin
        var linegr := new LineGraph();
        grd.Children.Add(linegr);
      
        linegr.Stroke := new SolidColorBrush(color);
        linegr.StrokeThickness := 1.4;
        linegr.Plot(xx, yy);
      end);
    end;
    procedure AddLineGraph(a,b: real; f: real -> real; color: GColor := Colors.Red);
    begin
      var x := PartitionPoints(a,b,200);
      var y := x.Select(f);
      AddLineGraph(x,y,color);
    end;
    procedure AddMarkerGraph(xx,yy: sequence of real; color: GColor := Colors.Aqua; MType: MarkerType := MarkerType.Circle; MarkerSize: real := 10);
    begin
      Invoke(procedure -> begin
        var markergr := new CircleMarkerGraph();
        grd.Children.Add(markergr);
      
        markergr.Color := new SolidColorBrush(color);
        markergr.StrokeThickness := 1.4;
        case MType of
      MarkerType.Circle:   markergr.MarkerType := new CircleMarker();
      MarkerType.Box:      markergr.MarkerType := new BoxMarker();
      MarkerType.Triangle: markergr.MarkerType := new TriangleMarker();
      MarkerType.Diamond:  markergr.MarkerType := new DiamondMarker();
      MarkerType.Cross:    markergr.MarkerType := new CrossMarker();
        end;
        markergr.PlotSize(xx, yy, MarkerSize);
      end);
    end;
    procedure AddMarkerGraph(xx,yy: sequence of real; MType: MarkerType) := 
      AddMarkerGraph(xx,yy,Colors.Aqua,MType,10);
    procedure AddMarkerGraph(xx,yy: sequence of real; MarkerSize: real) := 
      AddMarkerGraph(xx,yy,Colors.Aqua,MarkerType.Circle,MarkerSize);
  end;
  
  LineGraphWPF = class(BaseGraphWPF)
  public 
    constructor (a,b: real; f: real -> real; color: GColor := Colors.Red) := Invoke(CreateLineP,a,b,f,color);
    constructor (xx,yy: sequence of real; color: GColor := Colors.Red) := Invoke(CreateLineP1,xx,yy,color);
  end;
  
  MarkerGraphWPF = class(BaseGraphWPF)
  public 
    constructor (xx,yy: sequence of real; color: GColor := Colors.Aqua; MType: MarkerType := MarkerType.Circle; MarkerSize: real := 10) 
      := Invoke(CreateMarkerP1,xx,yy,color,MType,MarkerSize);
    constructor (xx,yy: sequence of real; MType: MarkerType) 
      := Invoke(CreateMarkerP1,xx,yy,Colors.Aqua,MType,10);
    constructor (xx,yy: sequence of real; color: GColor; MarkerSize: real) 
      := Invoke(CreateMarkerP1,xx,yy,Colors.Aqua,MarkerType.Circle,MarkerSize);
  end;

begin
  SetMainControl.AsGrid();
  GraphWPF.Window.Title := 'PlotWPF';
end.