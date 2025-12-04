type
  r1 = record
    
    public X, Y: integer;
    
    public constructor create(X, Y: integer);
    begin
      self.X := X;
      self.Y := Y;
    end;
    
    public function f1 := self.X;
    
  end;

begin
  var o:object := new r1(1,2);
  assert(r1(o).f1 = 1);
  assert(r1(o).ToString() = 'records11.r1');
  o := DateTime.Now;
  assert(DateTime(o).Kind = DateTime.Now.Kind);
end.