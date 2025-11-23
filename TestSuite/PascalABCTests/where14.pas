type
  
  Fixer<TFixer> = class
  where TFixer: Fixer<TFixer>;
    
    procedure p1;
    begin
      var a: TFixer;
      assert(a = nil);
      var o := self as object;
    end;
    
  end;
  
  DerFixer = class(Fixer<DerFixer>)
  end;
  
begin 
 var obj := new DerFixer();
 obj.p1;
end.