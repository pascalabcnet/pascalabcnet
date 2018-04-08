type
  t1=class t1_member:byte end;
  t2=class t2_member:byte end;
  
begin
  var thr{@var thr: Thread;@} := new System.Threading.Thread(()->begin
    
    var a1 := new t1;
    var a2 := new t2;
    
  end);
  
end.
