'Sluzhebnyj fajl, otdelno ne kompilirovat!
Imports System
Imports System.Reflection

Module _RedirectIOMode
  Const RedirectIOModeArg As String = "[REDIRECTIOMODE]"
  Const ReadlnSignalCommand As String = "[READLNSIGNAL]"
  Const CodePageCommandTemplate As String = "[CODEPAGE{0}]"
  Const RuntimeExceptionCommandTemplate As String = "[EXCEPTION]{0}[MESSAGE]{1}[STACK]{2}[END]"
  Dim R As Char = Convert.ToChar(10)
  Dim N As Char = Convert.ToChar(13)
  
  Dim ReadlnSignalSendet As Boolean = False
  Dim LastReadSymbol As Char = Convert.ToChar(0)
  
  Private Class __ReadSignalOISystem
                         Inherits IOStandardSystem
                               
    Public Overrides Function read_symbol() As Char
      Dim c As Char
      If Not ReadlnSignalSendet Then
        SendReadlnRequest()
      End If
      c = MyBase.read_symbol()
      If LastReadSymbol = N And c = R Then
        ReadlnSignalSendet = False
      End If
      LastReadSymbol = c
      Return c
    End Function
  
  End Class
  
  Sub SendReadlnRequest()
    ReadlnSignalSendet = True
    WriteToProcessErrorStream(ReadlnSignalCommand)
  End Sub

  Private Sub WriteToProcessErrorStream(text As String)
    Console.Error.Write(text)
    Console.Error.Flush
  End Sub
  
  Private Sub SendExceptionToProcessErrorStream(e As Exception)
    WriteToProcessErrorStream(string.Format(RuntimeExceptionCommandTemplate, e.GetType().ToString(), e.Message, e.StackTrace))
  End Sub

  Private Sub DbgExceptionHandler(sender As Object, args As UnhandledExceptionEventArgs)
    Dim e As Exception
    e = CType(args.ExceptionObject, Exception)
    SendExceptionToProcessErrorStream(e)
    If args.IsTerminating Then
      System.Diagnostics.Process.GetCurrentProcess().Kill()
    End If
  End Sub

  Private Sub Application_ThreadException(sender As object, args As System.Threading.ThreadExceptionEventArgs) 
    SendExceptionToProcessErrorStream(args.Exception)
  End Sub
  
  Sub Main()
    Dim args() As String
    args = System.Environment.GetCommandLineArgs()
    If (args.Length > 1) AndAlso (args(1) = RedirectIOModeArg) Then
      Console.ReadLine()
      If CurrentIOSystem Is Nothing OrElse CType(CurrentIOSystem, IOStandardSystem).GetType() Is GetType(IOStandardSystem) Then 'SSM 30.04.06 - не менять! Влияет на PT4!
        CurrentIOSystem = New __ReadSignalOISystem()
      End If
      AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf DbgExceptionHandler
      AddHandler System.Windows.Forms.Application.ThreadException, AddressOf Application_ThreadException
      WriteToProcessErrorStream(string.Format(CodePageCommandTemplate,65001))
      Console.OutputEncoding = System.Text.Encoding.UTF8
      Console.InputEncoding = System.Text.Encoding.UTF8
    End If
    %MAIN%
  End Sub
  
End Module
