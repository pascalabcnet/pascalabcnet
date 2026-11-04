uses PlotML;

begin
  var x := [37.62, 30.31, 82.92];
  var y := [55.75, 59.94, 55.03];
  var cities := ['Москва', 'Санкт-Петербург', 'Хабаровск'];

  Plot.Title := 'Города';
  Plot.XLabel := 'Долгота';
  Plot.YLabel := 'Широта';
  Plot.Points(x, y, tooltips := cities, size := 8);
end.
