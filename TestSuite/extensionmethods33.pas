uses System.Windows.Forms;
{$reference System.Windows.Forms.dll}

var i: integer;

procedure p1(self: Control); extensionmethod;
begin
  Inc(i);
end;

type
  
  t1 = abstract class(Panel)
    
  end;
  
  t2 = sealed class(t1)
    
    constructor;
    begin
      t1(self).p1;
      self.p1;
    end;
    
  end;
  
begin 
  var o := new t2;
  assert(i = 2);
end.