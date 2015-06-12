// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
uses System,
     System.Resources,
     System.Reflection,
     System.Globalization,
     System.Threading,
     System.IO,
     System.IO.Compression;

procedure Make(filename,resname,outputfilename: string);
begin
  var fs:FileStream;
  var br:BinaryReader;
  var w:ResourceWriter;
  try
    var infile := new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
    var data := new byte[infile.Length];
    infile.Read(data, 0, data.Length);
    infile.Close();
    var ms := new MemoryStream();
    var compressedStream := new GZipStream(ms, CompressionMode.Compress, true);
    compressedStream.Write(data, 0, data.Length);
    compressedStream.Close;
    ms.Position := 0;
    br := new BinaryReader(ms);
    var arr := br.ReadBytes(integer(br.BaseStream.Length));
    w := new ResourceWriter(outputfilename);
    w.AddResource(resname, arr);
    w.Generate;
    Console.WriteLine('Input file size={0}byte', data.Length);
    Console.WriteLine('Output file size={0}byte', arr.Length);
    var c: real := data.Length; 
    c := c / 100; 
    c := 100 - arr.Length / c;
    Console.WriteLine('Compression={0}%', Convert.ToInt32(c));
    except
       on e:Exception do Console.WriteLine(e);
  end;
  if fs <> nil then begin
    fs.Close;
    w.Close;
  end;
end;

begin
  if  CommandLineArgs.Length < 3 then
    Console.WriteLine('use: ResXMaker filename resname outputfilename')
  else begin
    Console.WriteLine('ResMaker: FILE->RESX  (c) DarkStar 2008 ');
    Make(CommandLineArgs[0],CommandLineArgs[1],CommandLineArgs[2]);
  end;  
end.