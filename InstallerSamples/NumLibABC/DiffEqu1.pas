uses NumLibABC;

// Ğåøåíèå çàäà÷è Êîøè

procedure Orbit(t:real; y,yp:array of real);
// äëÿ íåçàâèñèìîãî àğãóìåíòà å âîçâğàùàşòñÿ
// çíà÷åíèÿ ôóíêöèè y[] è å¸ ïåğâîé ïğîèçâîäíîé yp[]
begin
  var alpha:=Sqr(ArcTan(1.0));
  var r:=y[0]*y[0]+y[1]*y[1]; r:=r*Sqrt(r)/alpha;
  yp[0]:=y[2]; yp[1]:=y[3]; yp[2]:=-y[0]/r; yp[3]:=-y[1]/r
end;

begin
  var e:=0.25;
  var y:=Arr(1.0-e,0.0,0.0,ArcTan(1)*Sqrt((1.0+e)/(1.0-e)));
  var (abserr,relerr):=(0.0,0.3e-6);
  var oL:=new RKF45(Orbit, y, abserr, relerr);
  var (t,tb,th):=(0.0,12.0,0.5);
  var t_out:=t;
  repeat
    oL.Solve(t,t_out);
    Writeln(t:5:1,oL.y[0]:15:9,oL.y[1]:15:9);
    case oL.flag of
    -3,-2,-1,1,8:begin Writeln('Flag=',oL.flag); Exit end;
    2:t_out:=t+th;
    end
  until t>=tb
end.
