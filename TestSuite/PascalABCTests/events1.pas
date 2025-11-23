type
  TProc = procedure(sender: object);
  TProc2 = procedure;

var
  i: integer;

type
  IInt = interface
    event OnClick: System.EventHandler;
  end;
  
  TClass1 = class
    event OnClick: procedure(sender: object);
    class event OnClick_s: TProc2;
    
    procedure MyProc(sender: object);
    begin
      i := 9;
    end;
    
    constructor Create;
    begin
      OnClick += MyProc;
      OnClick(self);
      assert(i = 9);
      OnClick -= MyProc;
    end;
    
    procedure RaiseMethod;
    begin
      if OnClick <> nil then
        OnClick(self);
    end;
    
    class procedure RaiseMethod2;
    begin
      OnClick_s;
    end;
  end;
  
  TClass2 = class(IInt)
    event OnClick: System.EventHandler;//procedure(sender: object);
    class event OnClick_s: TProc2;
    
    procedure MyProc(sender: object; ev: System.EventArgs);
    begin
      i := 9;
    end;
    
    constructor Create;
    begin
      OnClick += MyProc;
      OnClick(self, nil);
      assert(i = 9);
      OnClick -= MyProc;
    end;
    
    procedure RaiseMethod;
    begin
      if OnClick <> nil then
        OnClick(self, nil);
    end;
    
    class procedure RaiseMethod2;
    begin
      OnClick_s;
    end;
  end;
  
procedure MyProc(sender: object);
begin
  i := 8;
end;

procedure MyProc(sender: object; ev: System.EventArgs);
begin
  i := 8;
end;

procedure MyProc2;
begin
  i := 7;
end;

var
  t: TClass1;
  t2: TClass2;
  p: procedure (sender: object);
  
begin
  t := new TClass1;
  t.OnClick += MyProc;
  t.RaiseMethod;
  assert(i = 8);
  TClass1.OnClick_s += MyProc2;
  TClass1.RaiseMethod2;
  assert(i = 7);
  
  i := 0;
  t2 := new TClass2;
  t2.OnClick += MyProc;
  t2.RaiseMethod;
  assert(i = 8);
  TClass2.OnClick_s += MyProc2;
  TClass2.RaiseMethod2;
  assert(i = 7);
end.