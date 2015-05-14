'Графика. Pie
Imports GraphABC

Module Gr5

Const r As Integer = 200

Sub Main()
  Window.Title = "Круговая гистограмма"
  Dim x As Integer = Window.Center.X
  Dim y As Integer = Window.Center.Y
  Brush.Color = clRandom
  Pie(x,y,r,0,30)  
  Brush.Color = clRandom
  Pie(x,y,r,30,110) 
  Brush.Color = clRandom
  Pie(x,y,r,110,160) 
  Brush.Color = clRandom
  Pie(x,y,r,160,280) 
  Brush.Color = clRandom
  Pie(x,y,r,280,360)  
End Sub

End Module