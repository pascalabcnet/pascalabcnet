uses MLABC, PlotML;

begin
  var ds := Datasets.Iris;

  var (X, labels) := ds.ToXYInt;

  var names := ds.Features;
  var n := names.Length;

  var fig := Plot.Grid(n, n);

  for var i := 0 to n-1 do
  for var j := 0 to n-1 do
  begin
    var ax := fig[i,j];
    
    ax.Points(X.Col(j), X.Col(i), labels, size := 3);

    {if i = n-1 then
      ax.XLabel(names[j]);

    if j = 0 then
      ax.YLabel(names[i]);}
  end;

  Plot.Title('Iris: пары признаков');
end.