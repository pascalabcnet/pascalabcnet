Option Strict

Imports System
Imports Microsoft.VisualBasic

Module VBSystem
  
  Public Interface IOSystem
    Function read_symbol() As Char
  End Interface
  
  Public Class IOStandardSystem
                    Implements IOSystem
  
  Public Overridable Function read_symbol() As Char Implements IOSystem.read_symbol
    Return Convert.ToChar(Console.Read())
  End Function
  
  End Class
  
  Public Dim CurrentIOSystem As IOSystem
  
  '''- WriteLine(a,b,...)
  '''Выводит на экран а, b,...
  Public Sub WriteLine(ParamArray arg() As Object)
    For i As Integer = 0 To arg.Length-1
      Console.Write(arg(i))
    Next i
    Console.WriteLine()
  End Sub
  
  Public Sub Write(ParamArray arg() As Object)
    For i As Integer = 0 To arg.Length-1
      Console.Write(arg(i))
    Next i
  End Sub
  
  Private Function read_lexem() As String 
    Dim c As Char
    Dim sb As System.Text.StringBuilder
    Do While char.IsWhiteSpace(c)
      c = CurrentIOSystem.read_symbol()
    Loop
    sb = New System.Text.StringBuilder()
    Do While Not char.IsWhiteSpace(c) 'And c <> Convert.ToChar(-1)
      sb.Append(c)
      c = CurrentIOSystem.read_symbol()
    Loop
    Return sb.ToString()
  End Function

  Public Function ReadLine() As String
    If CurrentIOSystem Is Nothing Then
      Return Console.ReadLine()
    Else
      Return read_lexem()
    End If
  End Function
  
  Public Function Sin(x As Double) As Double
    Return Math.Sin(x)
  End Function
  
  Public Function Cos(x As Double) As Double
    Return Math.Cos(x)
  End Function
  
  Public Function Tan(x As Double) As Double
    Return Math.Tan(x)
  End Function
  
  Public Sub Sleep(time As Integer)
    System.Threading.Thread.Sleep(time)
  End Sub
  
End Module