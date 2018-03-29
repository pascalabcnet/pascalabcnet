// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
///--
unit GraphWPFBase;

{$reference 'PresentationFramework.dll'}
{$reference 'WindowsBase.dll'}
{$reference 'PresentationCore.dll'}

{$apptype windows}

uses System.Windows; 
uses System.Windows.Controls;

type 
  GWindow = System.Windows.Window;
  GMainWindow = class(GWindow)
  public
    function CreateContent: DockPanel; virtual;
    begin
      var g := new DockPanel;
      g.LastChildFill := True;
      Result := g;
    end;
    
    procedure InitMainGraphControl; virtual;
    begin
    end;
    procedure InitWindowProperties; virtual;
    begin
    end;
    procedure InitGlobals; virtual;
    begin
    end;
    procedure InitHandlers; virtual;
    begin
    end;
    
    constructor Create;
    begin
      Content := CreateContent; 
      InitMainGraphControl;
      InitGlobals;
      InitWindowProperties;
      InitHandlers;
    end;
    
    function MainPanel: DockPanel := Content as DockPanel;
  end;

var 
  app: Application;
  MainWindow: GMainWindow;

procedure Invoke(d: System.Delegate; params args: array of object) := app.Dispatcher.Invoke(d, args);

procedure Invoke(d: ()->()) := app.Dispatcher.Invoke(d);

function Invoke<T>(d: Func0<T>): T := app.Dispatcher.Invoke&<T>(d);
function InvokeReal(f: ()->real): real := Invoke&<Real>(f);
function InvokeString(f: ()->string): string := Invoke&<string>(f);
function InvokeBoolean(d: Func0<boolean>): boolean := Invoke&<boolean>(d);
function InvokeInteger(d: Func0<integer>): integer := Invoke&<integer>(d);
function Inv<T>(p: ()->T): T := Invoke&<T>(p); // Теперь это работает!

function MainDockPanel: DockPanel := MainWindow.MainPanel;
  
var __initialized: boolean;

procedure __InitModule;
begin
end;

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    __InitModule;
  end;
end;

begin
  __InitModule;
end.