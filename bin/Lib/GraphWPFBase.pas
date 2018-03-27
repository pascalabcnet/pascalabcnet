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

var 
  app: Application;
  MainWindow: Window;

type 
  GWindow = System.Windows.Window;
  GMainWindow = class(GWindow)
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
  end;
  
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