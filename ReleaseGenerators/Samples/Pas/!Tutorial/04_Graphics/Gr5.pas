// Графика. Pie
uses GraphABC;

const r = 200;

begin
  Window.Title := 'Круговая гистограмма';
  var x := Window.Center.X;
  var y := Window.Center.Y;
  Brush.Color := clRandom;
  Pie(x,y,r,0,30);  
  Brush.Color := clRandom;
  Pie(x,y,r,30,110);  
  Brush.Color := clRandom;
  Pie(x,y,r,110,160);  
  Brush.Color := clRandom;
  Pie(x,y,r,160,280);  
  Brush.Color := clRandom;
  Pie(x,y,r,280,360);  
end.