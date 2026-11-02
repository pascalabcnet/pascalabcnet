uses MLABC, PlotML;

begin
  var df := DataFrame.FromCsvText('''
    Год,A,B,C,D
    2022,120,80,45,30
    2023,95,110,65,50
    2024,70,55,100,75
    2025,40,30,25,90
  ''');

  var fig := Plot.Grid(2, 2);

  fig[0, 0].Bar(df, 'Год', 'A');
  fig[0, 0].Title := 'Модель A';

  fig[0, 1].Bar(df, 'Год', 'B');
  fig[0, 1].Title := 'Модель B';

  fig[1, 0].Bar(df, 'Год', 'C');
  fig[1, 0].Title := 'Модель C';

  fig[1, 1].Bar(df, 'Год', 'D');
  fig[1, 1].Title := 'Модель D';

  Plot.Title := 'Продажи по годам';
end.
