{$resource header_part1}
{$resource header_part2}
{$resource res_end}

{$reference System.Drawing.dll}

begin
  try
    var icon_str := System.IO.File.OpenRead(CommandLineArgs[0]);
    var icon := new System.Drawing.Icon(icon_str);
    icon_str.Position := 0;
    if icon.Width <> icon.Height then Halt(1);
    case icon.Width of
      1,2,4,8,16,32: ;
      else Halt(2);
    end;
    
    var v: word := icon_str.Length-22;
    var otp := System.Console.OpenStandardOutput;
    //var otp := System.IO.File.Create('test.res');
    var bw := new System.IO.BinaryWriter(otp);
    
    GetResourceStream('header_part1').CopyTo(otp);
    bw.Write(v);
    GetResourceStream('header_part2').CopyTo(otp);
    
    var br := new System.IO.BinaryReader(System.IO.File.OpenRead(CommandLineArgs[0]));
    br.BaseStream.Position := 19;
    bw.Write(br.ReadBytes(br.BaseStream.Length-19));
    
    GetResourceStream('res_end').CopyTo(otp);
    bw.Write(byte(icon.Width));
    bw.Write(byte(icon.Width));
    bw.Write(byte($10));
    if icon.Width = 1 then
      bw.Write(byte($FF)) else
      bw.Write(byte($01));
    bw.Write(byte($01));
    bw.Write(byte($00));
    bw.Write(byte($04));
    bw.Write(byte($00));
    bw.Write(v);
    bw.Write(byte($00));
    bw.Write(byte($00));
    bw.Write(byte($01));
    bw.Write(byte($00));
    bw.Flush;
    
  except
    on e: Exception do
    begin
      //WriteAllText('RC error log.txt', _ObjectToString(e));
      //Exec('RC error log.txt');
      Halt(e.HResult);
    end;
  end;
end.