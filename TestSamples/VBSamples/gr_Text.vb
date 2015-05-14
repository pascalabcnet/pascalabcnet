Imports ABCObjects, GraphABC

Module gr_Text

Sub Main()
  Dim bt As TextABC
  Dim x As Integer
  
  x = 224
  bt = New TextABC(60, 110, 110, "Hello!", RGB(x,x,x))
  
  While x > 32
    Sleep(40)
    x = x - 32
    bt = CType(bt.Clone(), TextABC)
    bt.Color = RGB(x, x, x)
    bt.MoveOn(7, 7)
  End While
End Sub

End Module
