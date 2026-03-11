unit PlotML;

{$reference %GAC%\InteractiveDataDisplay.WPF.dll}
{$reference 'PresentationFramework.dll'}
{$reference 'WindowsBase.dll'}
{$reference 'PresentationCore.dll'}

interface

uses System,
     System.Windows,
     System.Windows.Controls,
     System.Windows.Media,
     System.Threading,
     System.Windows.Threading,
     InteractiveDataDisplay.WPF;
     
type
  ApplicationWPF = System.Windows.Application;
  WindowWPF = System.Windows.Window;
  GridWPF = System.Windows.Controls.Grid;
  ChartWPF = InteractiveDataDisplay.WPF.Chart;
  LineGraphWPF = InteractiveDataDisplay.WPF.LineGraph;
  MarkerGraphWPF = InteractiveDataDisplay.WPF.CircleMarkerGraph;
  BrushWPF = System.Windows.Media.SolidColorBrush;
  Colors = System.Windows.Media.Colors;
  ColorWPF = System.Windows.Media.Color;
  
const DefaultColor = default(ColorWPF);
  
type
  MarkerType = (Circle, Box, Triangle, Diamond, Cross);

  Palettes = static class
  public
    const &Default = 'default';
    const Dark     = 'dark';
    const Pastel   = 'pastel';
    const Bright   = 'bright';
    const Muted    = 'muted';
  end;

  Palette = class
  public
    Colors: array of Color;
    constructor Create(params c: array of Color);
  end;

  Figure = class;

  Cell = class
  private
    parentGrid: GridWPF;
    row,col: integer;
    chart: ChartWPF;

    palette: Palette;
    paletteIndex: integer := 0;

    function NextColor: Color;
    procedure EnsureChart;

  public
    constructor Create(g: GridWPF; r,c: integer);

    procedure LineGraph(x, y: array of real; color: ColorWPF := DefaultColor; 
      thickness: real := 2; legend: string := nil);
      
    procedure Points(x, y: array of real; color: ColorWPF := DefaultColor; 
      size: real := 6; marker: MarkerType := MarkerType.Circle; legend: string := nil);
      
    procedure Points(x, y: array of real; labels: array of integer;
      size: real := 6; marker: MarkerType := MarkerType.Circle);  
      
    procedure Heatmap(m: array[,] of real);
    
    procedure Text(s: string; x: real := 0.5; y: real := 0.5);
    
    procedure SetPalette(name: string);
    procedure Title(s: string);
    procedure XLabel(s: string);
    procedure YLabel(s: string);
    
    procedure Limits(xmin, xmax, ymin, ymax: real);
    procedure XLim(xmin,xmax: real);
    procedure YLim(ymin,ymax: real);
    
    procedure Clear;
  end;

  Figure = class
  private
    grid: GridWPF;
    cells: array[,] of Cell;
  public
    constructor Create(rows,cols: integer);
    property Item[r,c: integer]: Cell read cells[r,c]; default;
  end;

  Plot = static class
  private
    static procedure RunUI(a: procedure);

    static function CreateLineSeries(x,y: array of real; c: Color): LineGraphWPF;
    static function CreatePointSeries(x, y: array of real; 
      color: ColorWPF; size: real; marker: MarkerType): MarkerGraphWPF;
    
    static procedure DrawLine(chart: ChartWPF; x, y: array of real;
      color: ColorWPF; thickness: real; legend: string);
      
    static procedure DrawText(chart: ChartWPF; s: string; x, y: real);  
  
    static procedure DrawPoints(chart: ChartWPF; x, y: array of real;
      color: ColorWPF; size: real; marker: MarkerType; legend: string);
      
    static procedure DrawHeatmap(chart: ChartWPF; m: array[,] of real);  
    
  public
    static procedure AddSeries(chart: ChartWPF; series: UIElement);

    static procedure LineGraph(x, y: array of real;
      color: ColorWPF := DefaultColor; thickness: real := 2; legend: string := nil);
    static procedure Points(x, y: array of real; 
      color: ColorWPF := DefaultColor; size: real := 6; marker: MarkerType := MarkerType.Circle; legend: string := nil);
    static procedure Points(x, y: array of real;
      labels: array of integer; color: ColorWPF := DefaultColor; size: real := 6; marker: MarkerType := MarkerType.Circle);
      
    static procedure Heatmap(m: array[,] of real);  
    
    static function Grid(rows,cols: integer): Figure;

    static procedure SetPalette(name: string);
    
    static procedure Limits(xmin,xmax,ymin,ymax: real);
    static procedure XLim(xmin,xmax: real);
    static procedure YLim(ymin,ymax: real);
    static procedure Title(s: string);
    static procedure XLabel(s: string);
    static procedure YLabel(s: string);
    static procedure SetLabels(title: string := ''; xlabel: string := ''; ylabel: string := '');
    
    static procedure Clear;
    
    static procedure Save(filename: string);
  end;

implementation

var
  uiThread: Thread;
  uiDispatcher: Dispatcher;

  app: ApplicationWPF;
  win: WindowWPF;
  rootChart: ChartWPF;

  paletteDict: Dictionary<string,Palette>;
  currentPalette: Palette;
  rootPaletteIndex: integer := 0;
  
function CreateMarker(t: MarkerType): InteractiveDataDisplay.WPF.ColorSizeMarker;
begin
  case t of
    MarkerType.Circle:   Result := new CircleMarker;
    MarkerType.Box:      Result := new BoxMarker;
    MarkerType.Triangle: Result := new TriangleMarker;
    MarkerType.Diamond:  Result := new DiamondMarker;
    MarkerType.Cross:    Result := new CrossMarker;
  end;
end;
  
function NextRootColor: ColorWPF;
begin
  var c := currentPalette.Colors[
    rootPaletteIndex mod currentPalette.Colors.Length
  ];

  rootPaletteIndex += 1;
  Result := c;
end;

function MakeHistogram(data: array of real; bins: integer): (array of real, array of real);
begin
  var xmin := data.Min;
  var xmax := data.Max;
  
  var h := (xmax-xmin)/bins;
  
  var counts := new real[bins];
  
  foreach var v in data do
  begin
    var k := integer((v-xmin)/h);
    if k=bins then k := bins-1;
    counts[k] += 1;
  end;
  
  var xs := ArrGen(bins, i -> xmin + (i+0.5)*h);
  
  Result := (xs,counts);
end;

procedure InitPalettes;
begin
  paletteDict := new Dictionary<string,Palette>;

  paletteDict['default'] := new Palette(
    Color.FromRgb($1f,$77,$b4),
    Color.FromRgb($ff,$7f,$0e),
    Color.FromRgb($2c,$a0,$2c),
    Color.FromRgb($d6,$27,$28),
    Color.FromRgb($94,$67,$bd),
    Color.FromRgb($8c,$56,$4b),
    Color.FromRgb($e3,$77,$c2),
    Color.FromRgb($7f,$7f,$7f),
    Color.FromRgb($bc,$bd,$22),
    Color.FromRgb($17,$be,$cf)
  );

  paletteDict['pastel'] := new Palette(
    Color.FromRgb(141,211,199),
    Color.FromRgb(255,255,179),
    Color.FromRgb(190,186,218),
    Color.FromRgb(251,128,114),
    Color.FromRgb(128,177,211),
    Color.FromRgb(253,180,98),
    Color.FromRgb(179,222,105),
    Color.FromRgb(252,205,229)
  );

  paletteDict['dark'] := new Palette(
    Color.FromRgb(27,158,119),
    Color.FromRgb(217,95,2),
    Color.FromRgb(117,112,179),
    Color.FromRgb(231,41,138),
    Color.FromRgb(102,166,30),
    Color.FromRgb(230,171,2),
    Color.FromRgb(166,118,29),
    Color.FromRgb(102,102,102)
  );

  paletteDict['bright'] := new Palette(
    Color.FromRgb(0,114,178),
    Color.FromRgb(230,159,0),
    Color.FromRgb(0,158,115),
    Color.FromRgb(213,94,0),
    Color.FromRgb(204,121,167),
    Color.FromRgb(86,180,233)
  );
  
  paletteDict['muted'] := new Palette(
    Color.FromRgb(76,114,176),
    Color.FromRgb(221,132,82),
    Color.FromRgb(85,168,104),
    Color.FromRgb(196,78,82),
    Color.FromRgb(129,114,179),
    Color.FromRgb(147,120,96)
  );
  
  currentPalette := paletteDict['default'];
end;

procedure InitUI;
begin
  uiThread := new Thread(() ->
  begin
    app := new ApplicationWPF;

    app.Dispatcher.UnhandledException += (o,e) ->
    begin
      Println(e.Exception.Message);
      if e.Exception.InnerException <> nil then
        Println(e.Exception.InnerException.Message);
      halt;
    end;

    rootChart := new ChartWPF;

    win := new WindowWPF;
    win.Title := 'PlotML';
    win.Width := 800;
    win.Height := 600;
    win.Content := rootChart;

    win.Closed += (o,e) ->
      Dispatcher.CurrentDispatcher.BeginInvokeShutdown(
        DispatcherPriority.Normal);

    uiDispatcher := Dispatcher.CurrentDispatcher;

    InitPalettes;

    win.Show;

    Dispatcher.Run;
  end);

  uiThread.SetApartmentState(ApartmentState.STA);
  uiThread.Start;

  while uiDispatcher = nil do
    Sleep(10);
end;

constructor Palette.Create(params c: array of Color);
begin
  Colors := c;
end;

constructor Cell.Create(g: GridWPF; r,c: integer);
begin
  parentGrid := g;
  row := r;
  col := c;
  palette := currentPalette;
end;

function Cell.NextColor: ColorWPF;
begin
  var c := palette.Colors[
    paletteIndex mod palette.Colors.Length
  ];

  paletteIndex += 1;

  Result := c;
end;

procedure Cell.SetPalette(name: string);
begin
  if paletteDict.ContainsKey(name) then
  begin
    palette := paletteDict[name];
    paletteIndex := 0;
  end;
end;

procedure Cell.Title(s: string);
begin
  Plot.RunUI(() ->
  begin
    EnsureChart;
    chart.Title := s;
  end);
end;

procedure Cell.XLabel(s: string);
begin
  Plot.RunUI(() ->
  begin
    EnsureChart;
    chart.BottomTitle := s;
  end);
end;

procedure Cell.YLabel(s: string);
begin
  Plot.RunUI(() ->
  begin
    EnsureChart;
    chart.LeftTitle := s;
  end);
end;

procedure Cell.Limits(xmin,xmax,ymin,ymax: real);
begin
  Plot.RunUI(() ->
  begin
    EnsureChart;

    chart.PlotOriginX := xmin;
    chart.PlotOriginY := ymin;

    chart.PlotWidth := xmax - xmin;
    chart.PlotHeight := ymax - ymin;
  end);
end;

procedure Cell.XLim(xmin,xmax: real);
begin
  Plot.RunUI(() ->
  begin
    EnsureChart;

    chart.PlotOriginX := xmin;
    chart.PlotWidth := xmax - xmin;
  end);
end;

procedure Cell.YLim(ymin,ymax: real);
begin
  Plot.RunUI(() ->
  begin
    EnsureChart;

    chart.PlotOriginY := ymin;
    chart.PlotHeight := ymax - ymin;
  end);
end;

procedure Cell.Clear;
begin
  Plot.RunUI(() ->
  begin
    if chart = nil then exit;

    var container := chart.Content as GridWPF;
    if container <> nil then
      container.Children.Clear;

    paletteIndex := 0;
  end);
end;

procedure Cell.EnsureChart;
begin
  if chart <> nil then exit;

  chart := new ChartWPF;
  chart.LegendVisibility := Visibility.Hidden;

  var container := new GridWPF;
  chart.Content := container;

  GridWPF.SetRow(chart,row);
  GridWPF.SetColumn(chart,col);

  parentGrid.Children.Add(chart);
end;

procedure Cell.LineGraph(x, y: array of real; color: ColorWPF; 
  thickness: real; legend: string);
begin
  Plot.RunUI(() ->
  begin
    EnsureChart;

    var clr := if color<>DefaultColor then color else NextColor;

    Plot.DrawLine(chart, x, y, clr, thickness, legend);
  end);
end;

procedure Cell.Points(x, y: array of real; color: ColorWPF;
  size: real; marker: MarkerType; legend: string);
begin
  Plot.RunUI(() ->
  begin
    EnsureChart;

    var clr := if color<>DefaultColor then color else NextColor;

    Plot.DrawPoints(chart, x, y, clr, size, marker, legend);
  end);
end;

procedure Cell.Points(x, y: array of real; labels: array of integer;
  size: real; marker: MarkerType);
begin
  if (x = nil) or (y = nil) or (labels = nil) then
    raise new System.ArgumentNullException;

  if (x.Length <> y.Length) or (x.Length <> labels.Length) then
    raise new System.ArgumentException('Points: array sizes mismatch');

  var classes := labels.Distinct.ToArray;
  &Array.Sort(classes);

  var pal := CurrentPalette;

  foreach var c in classes do
  begin
    var ind := labels.Indices(v -> v = c).ToArray;

    var xs := ind.ConvertAll(i -> x[i]);
    var ys := ind.ConvertAll(i -> y[i]);

    var clr := pal.Colors[c mod pal.Colors.Length];

    self.Points(xs, ys, clr, size, marker, nil);
  end;
end;

procedure Cell.Heatmap(m: array[,] of real);
begin
  Plot.RunUI(() ->
  begin
    EnsureChart;
    Plot.DrawHeatmap(chart, m);
  end);
end;

procedure Cell.Text(s: string; x: real; y: real);
begin
  Plot.RunUI(() ->
  begin
    EnsureChart;

    Plot.DrawText(chart, s, x, y);
  end);
end;

constructor Figure.Create(rows,cols: integer);
begin
  grid := new GridWPF;

  for var i:=0 to rows-1 do
    grid.RowDefinitions.Add(new RowDefinition);

  for var j:=0 to cols-1 do
    grid.ColumnDefinitions.Add(new ColumnDefinition);

  cells := new Cell[rows,cols];

  for var i:=0 to rows-1 do
    for var j:=0 to cols-1 do
      cells[i,j] := new Cell(grid,i,j);
end;

static procedure Plot.RunUI(a: procedure);
begin
  if Dispatcher.CurrentDispatcher = uiDispatcher then
    a()
  else
    uiDispatcher.Invoke(a);
end;

static procedure Plot.Limits(xmin,xmax,ymin,ymax: real);
begin
  RunUI(() ->
  begin
    rootChart.PlotOriginX := xmin;
    rootChart.PlotOriginY := ymin;

    rootChart.PlotWidth := xmax - xmin;
    rootChart.PlotHeight := ymax - ymin;
  end);
end;

static procedure Plot.XLim(xmin,xmax: real);
begin
  RunUI(() ->
  begin
    rootChart.PlotOriginX := xmin;
    rootChart.PlotWidth := xmax - xmin;
  end);
end;

static procedure Plot.YLim(ymin,ymax: real);
begin
  RunUI(() ->
  begin
    rootChart.PlotOriginY := ymin;
    rootChart.PlotHeight := ymax - ymin;
  end);
end;

static procedure Plot.Title(s: string);
begin
  RunUI(() ->
  begin
    rootChart.Title := s;
  end);
end;

static procedure Plot.XLabel(s: string);
begin
  RunUI(() ->
  begin
    rootChart.BottomTitle := s;
  end);
end;

static procedure Plot.YLabel(s: string);
begin
  RunUI(() ->
  begin
    rootChart.LeftTitle := s;
  end);
end;

static procedure Plot.SetLabels(title: string; xlabel: string; ylabel: string);
begin
  RunUI(() ->
  begin
    if title <> '' then
      rootChart.Title := title;

    if xlabel <> '' then
      rootChart.BottomTitle := xlabel;

    if ylabel <> '' then
      rootChart.LeftTitle := ylabel;
  end);
end;

static procedure Plot.Clear;
begin
  RunUI(() ->
  begin
    var container := rootChart.Content as GridWPF;
    if container <> nil then
      container.Children.Clear;
  end);
end;

static procedure Plot.Save(filename: string);
begin
  RunUI(() ->
  begin
    var rtb := new System.Windows.Media.Imaging.RenderTargetBitmap(
      integer(win.Width),
      integer(win.Height),
      96, 96,
      PixelFormats.Pbgra32
    );

    rtb.Render(win);

    var encoder := new System.Windows.Media.Imaging.PngBitmapEncoder;
    encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(rtb));

    var fs := new System.IO.FileStream(filename, System.IO.FileMode.Create);
    encoder.Save(fs);
    fs.Close;
  end);
end;

static function Plot.CreateLineSeries(x,y: array of real; c: Color): LineGraphWPF;
begin
  var g := new LineGraphWPF;

  g.Stroke := new BrushWPF(c);
  g.StrokeThickness := 2;

  g.Plot(x,y);

  Result := g;
end;

static function Plot.CreatePointSeries(x, y: array of real; 
  color: ColorWPF; size: real; marker: MarkerType): MarkerGraphWPF;
begin
  var g := new MarkerGraphWPF;

  {var alpha := 0.5;
  var a := round(alpha * 255);
  var c := ColorWPF.FromArgb(a, color.R, color.G, color.B);
  
  g.Color := c;}
  
  g.Color := color;
  g.Size := size;
  g.MarkerType := CreateMarker(marker);
  g.StrokeThickness := 0;
  
  g.Plot(x,y);

  Result := g;
end;

static procedure Plot.DrawLine(chart: ChartWPF; x, y: array of real;
  color: ColorWPF; thickness: real; legend: string);
begin
  var g := CreateLineSeries(x, y, color);

  g.StrokeThickness := thickness;

  if legend <> nil then
  begin  
    g.Description := legend;
    chart.LegendVisibility := Visibility.Visible;
  end;  

  AddSeries(chart, g);
end;

static procedure Plot.DrawText(chart: ChartWPF; s: string; x, y: real);
begin
  var tb := new System.Windows.Controls.TextBlock;
  tb.Text := s;
  tb.FontSize := 14;
  tb.FontWeight := System.Windows.FontWeights.Bold;

  tb.HorizontalAlignment := System.Windows.HorizontalAlignment.Center;
  tb.VerticalAlignment := System.Windows.VerticalAlignment.Center;

  var grid := chart.Parent as System.Windows.Controls.Grid;
  if grid <> nil then
  begin
    tb.HorizontalAlignment := System.Windows.HorizontalAlignment.Center;
    tb.VerticalAlignment := System.Windows.VerticalAlignment.Center;
    grid.Children.Add(tb);
  end;
end;

static procedure Plot.DrawPoints(chart: ChartWPF; x, y: array of real;
  color: ColorWPF; size: real; marker: MarkerType; legend: string);
begin
  var g := CreatePointSeries(x, y, color, size, marker);

  if legend <> nil then
  begin  
    g.Description := legend;
    chart.LegendVisibility := Visibility.Visible;
  end;

  AddSeries(chart, g);
end;

static procedure Plot.DrawHeatmap(chart: ChartWPF; m: array[,] of real);
begin
  var rows := m.GetLength(0);
  var cols := m.GetLength(1);

  var x := ArrGen(cols, i -> i);
  var y := ArrGen(rows, i -> i);

  var g := new HeatmapGraph;
  
  g.Plot(m, x, y);

  AddSeries(chart, g);
end;

class procedure Plot.AddSeries(chart: ChartWPF; series: UIElement);
begin
  var container := chart.Content as GridWPF;

  if container = nil then
  begin
    container := new GridWPF;
    chart.Content := container;
  end;

  container.Children.Add(series);
end;

class procedure Plot.LineGraph(x, y: array of real;
  color: ColorWPF; thickness: real; legend: string);
begin
  RunUI(() ->
  begin
    var clr := if color<>DefaultColor then color else NextRootColor;

    DrawLine(rootChart, x, y, clr, thickness, legend);
  end);
end;

static procedure Plot.Points(x, y: array of real; 
  color: ColorWPF; size: real; marker: MarkerType; legend: string);
begin
  RunUI(() ->
  begin
    var clr := if color<>DefaultColor then color else NextRootColor;

    DrawPoints(rootChart, x, y, clr, size, marker, legend);
  end);
end;

static procedure Plot.Points(x, y: array of real; labels: array of integer;
  color: ColorWPF; size: real; marker: MarkerType);
begin
  if (x = nil) or (y = nil) or (labels = nil) then
    raise new System.ArgumentNullException;

  if (x.Length <> y.Length) or (x.Length <> labels.Length) then
    raise new System.ArgumentException('Points: array sizes mismatch');

  var classes := labels.Distinct.ToArray;
  &Array.Sort(classes);

  var pal := CurrentPalette;

  foreach var c in classes do
  begin
    var ind := labels.Indices(v -> v = c).ToArray;

    var xs := ind.ConvertAll(i -> x[i]);
    var ys := ind.ConvertAll(i -> y[i]);

    var clr :=
      if color<>DefaultColor
      then color
      else pal.Colors[c mod pal.Colors.Length];

    Points(xs, ys, clr, size, marker, nil);
  end;
end;

static procedure Plot.Heatmap(m: array[,] of real);
begin
  RunUI(() ->
  begin
    DrawHeatmap(rootChart, m);
  end);
end;

static function Plot.Grid(rows,cols: integer): Figure;
begin
  var fig: Figure;

  RunUI(() ->
  begin
    fig := new Figure(rows,cols);
    win.Content := fig.grid;
  end);

  Result := fig;
end;

static procedure Plot.SetPalette(name: string);
begin
  RunUI(() ->
  begin
    if name in paletteDict then
      currentPalette := paletteDict[name];
  end);
end;

initialization
  InitUI;

end.