var o:object;
    e:Exception;
    r:real;
begin
  o:=new Exception('xxx');
  writeln((o as Exception).Message);
  writeln(Exception(o).Message);
  e:=Exception(o);writeln(e);
  e:=(o as Exception);writeln(e);
  
  //o:=real(o);  //invalid cast exception
  o:=o as System.Console;//null
  
  
  o:=1.1;
  //writeln((o as real).tostring);
  writeln(real(o).tostring);
  //r:=(o as real);writeln(r);
  r:=real(o);writeln(r);
   
   
  readln;
end.