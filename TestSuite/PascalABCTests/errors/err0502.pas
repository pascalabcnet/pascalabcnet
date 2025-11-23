//!Нельзя обратиться к нестатическому методу f1 из инициализатора статического поля
type
  t1 = class
    
    public function f1 := 0;
    
    public static x := f1;
  end;
  
begin
  t1.x.Println;
end.