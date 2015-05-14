type
  TVec3 = record
    x, y, z: integer;
    
    constructor Create;
    begin
      self.x := 1;
      self.y := 2;
      self.z := 3;
      var v: TVec3 := Self;
      var p: ^TVec3 := @Self;
      assert(v.x = 1);
      assert(p^.x = 1);
      Test(self);
      Test2(self);
    end;
    
    procedure Test(var r: TVec3);
    begin
      assert(r.x = 1);
    end;
    
    procedure Test2(r: TVec3);
    begin
      assert(r.x = 1);
    end;
  end;

begin
  var x := new TVec3;
end.