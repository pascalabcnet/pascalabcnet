var i: integer;
type
  c1 = class
    en1: (const1, const2);
      
    procedure pr1;
    begin
      var v1: c1 := self;
      en1 := const2;
      var f: ()-> ();
      f := ()-> begin
        assert(v1.en1 = const2);
        i := 1;
      end;
      f;
    end;
  end;
  
begin 
  var o := new c1;
  o.pr1;
  assert(i = 1);
end.