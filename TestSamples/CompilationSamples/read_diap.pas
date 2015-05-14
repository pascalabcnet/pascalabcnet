type TEnum = (one, two, three);

var f : file of TEnum;
    a : TEnum;
    
begin
Assign(f,'enum.dat');
Rewrite(f);
Write(f,one);
Write(f,three);
Seek(f,0);
read(f,a);

end.