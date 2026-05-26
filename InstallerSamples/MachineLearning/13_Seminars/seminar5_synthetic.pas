uses MLABC, PlotML;

function ToIntArray(v: Vector): array of integer;
begin
  Result := v.Data.Select(t -> integer(t)).ToArray;
end;

function BinLabels(v: Vector; bins: integer := 8): array of integer;
begin
  Result := new integer[v.Length];

  var vmin := v.Min;
  var vmax := v.Max;

  if vmax = vmin then
    exit;

  var w := (vmax - vmin) / bins;

  for var i := 0 to v.Length - 1 do
  begin
    var k := trunc((v[i] - vmin) / w);
    if k >= bins then k := bins - 1;
    if k < 0 then k := 0;
    Result[i] := k;
  end;
end;

procedure DrawDataset(cell: Cell; X: Matrix; labels: array of integer; title: string);
begin
  var x1 := X.Col(0);
  var x2 := X.Col(1);

  // защита от рассинхронизации
  if labels.Length <> X.RowCount then
    raise new Exception('Labels length mismatch');

  cell.SetPalette(Palettes.Bright);
  cell.Points(x1, x2, labels, size := 5);
  cell.Title := title;
end;

begin
  var fig := Plot.Grid(2, 3);

  // --- Blobs
  var (X1, y1) := Datasets.MakeBlobs(
    n := 300,
    centers := 3,
    nFeatures := 2,
    clusterStd := 0.7,
    clusterStdVar := 0.4,
    centerBox := 5.0,
    classBalance := 1.0,
    noisePoints := 20,
    shuffle := True,
    seed := 1
  );
  DrawDataset(fig[0,0], X1, ToIntArray(y1), 'MakeBlobs');

  // --- Moons
  var (X2, y2) := Datasets.MakeMoons(
    n := 300,
    noise := 0.1,
    shuffle := True,
    seed := 2
  );
  DrawDataset(fig[0,1], X2, ToIntArray(y2), 'MakeMoons');

  // --- Circles
  var (X3, y3) := Datasets.MakeCircles(
    n := 300,
    noise := 0.08,
    factor := 0.45,
    classBalance := 0.5,
    flipProb := 0.0,
    scale := 3.0,
    shuffle := True,
    seed := 3
  );
  DrawDataset(fig[0,2], X3, ToIntArray(y3), 'MakeCircles');

  // --- Spiral
  var (X4, y4) := Datasets.MakeSpiral(
    n := 300,
    noise := 0.03,
    turns := 2.5,
    shuffle := True,
    seed := 4
  );
  DrawDataset(fig[1,0], X4, ToIntArray(y4), 'MakeSpiral');

  // --- Regression (биннинг)
  var (X5, y5) := Datasets.MakeRegression(
    n := 300,
    nFeatures := 2,
    nInformative := 2,
    noise := 0.02,
    coefScale := 1.0,
    bias := 0.0,
    nonlinearStrength := 3.0,
    shuffle := True,
    seed := 5
  );
  var labels := ArrFill(X5.RowCount, 0);
  DrawDataset(fig[1,1], X5, labels, 'MakeRegression');

  // --- Classification
  var (X6, y6) := Datasets.MakeClassification(
    n := 300,
    nFeatures := 2,
    nInformative := 2,
    nRedundant := 0,
    noise := 0.2,
    classSep := 2.5,
    flipProb := 0.05,
    classBalance := 0.5,
    shuffle := True,
    seed := 6
  );
  DrawDataset(fig[1,2], X6, ToIntArray(y6), 'MakeClassification');
end.