// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
///--
unit PABCExtensions;

uses PABCSystem;

function OpenBinary<T>(fname: string): file of T;
begin
  PABCSystem.Reset(Result, fname);
end;

function CreateBinary<T>(fname: string): file of T;
begin
  PABCSystem.Rewrite(Result, fname);
end;

function Elements<T>(Self: file of T): sequence of T; extensionmethod;
begin
  while not Self.Eof do
  begin
    var x: T;
    read(Self,x);
    yield x;
  end;
end;

function ReadElement<T>(Self: file of T): T; extensionmethod;
begin
  Read(Self, Result);
end;

function ReadElements<T>(fname: string): sequence of T;
begin
  var f := OpenBinary&<T>(fname);
  while not f.Eof do
  begin
    var x := f.ReadElement;
    yield x;
  end;
  f.Close
end;

procedure Write<T>(Self: file of T; params vals: array of T); extensionmethod;
begin
  foreach var x in vals do
    PABCSystem.Write(Self, x);
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
    __InitPABCSystem;
    __InitModule;
  end;
end;

begin
  __InitModule;
end.