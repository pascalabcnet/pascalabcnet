// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
unit Utils;

interface

uses System;

function RecordToByteArray(obj : object): array of byte;
procedure ShowMessage(msg: string);
procedure ShowMessage(msg,capt: string);
function GetFunctionPointer(fnc : Delegate) : integer;

implementation

uses System.Runtime.InteropServices;

function _MessageBox(h: integer; m,c: string; t: integer): integer;
external 'User32.dll' name 'MessageBox';

procedure ShowMessage(msg:string);
begin
  ShowMessage(msg,'Сообщение');
end;

procedure ShowMessage(msg,capt:string);
begin
  _MessageBox(0,msg,capt,0);
end;

function RecordToByteArray(obj : object): array of byte;
begin
  var len := Marshal.SizeOf(obj);
  Result := new byte[len];
  var ptr := Marshal.AllocHGlobal(len);
  Marshal.StructureToPtr(obj, ptr, false);
  Marshal.Copy(ptr, Result, 0, len);
  Marshal.FreeHGlobal(ptr); 
end;

function GetFunctionPointer(fnc : Delegate) : integer;
begin
  Result := System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(fnc).ToInt32;  
end;

begin
end.