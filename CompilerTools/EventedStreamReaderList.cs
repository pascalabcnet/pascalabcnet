// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Timers;

namespace PascalABCCompiler
{
    public class EventedStreamReaderList
    {
        Dictionary<StreamReader, string> StreamReaders = new Dictionary<StreamReader, string>();
        Dictionary<StreamReader, string> Buffers = new Dictionary<StreamReader, string>();
        Dictionary<string, Encoding> StreamEncodings = new Dictionary<string, Encoding>();
        StringRecivedDelegate StringRecived;

        string dataSeparator = null;
        public string DataSeparator
        {
            get
            {
                return dataSeparator;
            }
            set
            {
                dataSeparator = value;
            }
        }

        class StreamReaderObject
        {
            public const int BUFFER_SIZE = 1024;
            public byte[] buffer = new byte[BUFFER_SIZE];
            public string Ident;
            public int byteoffset = 0;

            public StreamReader StreamReader;
            public StreamReaderObject(string Ident, StreamReader StreamReader)
            {
                this.Ident = Ident;
                this.StreamReader = StreamReader;
            }
        }

        public delegate void StringRecivedDelegate(string Id, string Data);

        public EventedStreamReaderList(StringRecivedDelegate StringRecived)
        {
            this.StringRecived = StringRecived;
        }


        private void Read_Callback1(IAsyncResult ar)
        {
            if (ar == null)
                return;
            StreamReaderObject so = (StreamReaderObject)ar.AsyncState;
            if (!StreamReaders.ContainsKey(so.StreamReader))
                return;
            Stream s = so.StreamReader.BaseStream;
            string buffer = Buffers[so.StreamReader];
            try
            {
                int read = s.EndRead(ar);
                if (read > 0)
                {
                    string str = StreamEncodings[so.Ident].GetString(so.buffer, 0, read);
                    if (dataSeparator != null)
                    {
                        buffer += str;
                        int i;
                        do
                        {
                            i = buffer.IndexOf(dataSeparator);
                            if (i > 0)
                            {
                                StringRecived(so.Ident, buffer.Substring(0, i));
                                buffer = buffer.Remove(0, i + dataSeparator.Length);
                            }
                        } while (i > 0);
                    }
                    else
                        StringRecived(so.Ident, str);
                    Buffers[so.StreamReader] = buffer;
                    s.BeginRead(so.buffer, 0, StreamReaderObject.BUFFER_SIZE, Read_Callback, so);
                }
            }
            catch
            {
                Remove(so.Ident);
            }
        }

        private void Read_Callback(IAsyncResult ar)
        {
            if (ar == null)
                return;
            StreamReaderObject so = (StreamReaderObject)ar.AsyncState;
            if (!StreamReaders.ContainsKey(so.StreamReader))
                return;
            Stream s = so.StreamReader.BaseStream;
            string buffer = Buffers[so.StreamReader];
            try
            {
                int read = s.EndRead(ar);

                // Новое поле byteoffset говорит о том, что с прошлого раза мы учли дополнительный байт
                read += so.byteoffset;
                if (read > 0)
                {
                    Encoding en = StreamEncodings[so.Ident];
                    Decoder d = en.GetDecoder();
                    char []cc = new char[read];
                    int bytesUsed;
                    int charsUsed;
                    bool completed;

                    d.Convert(so.buffer, 0, read, cc, 0, read, false, out bytesUsed, out charsUsed, out completed);
                    string newstr = new string(cc, 0, charsUsed);

                    string str = en.GetString(so.buffer, 0, read);

                    // К сожалению, ошибочный байт устанавливается за счет разницы длин str и newstr
                    // Надо бы оптимизировать

                    byte prevbyte = 0;
                    if (str.Length != newstr.Length)
                        prevbyte = so.buffer[bytesUsed - 1];

                    if (dataSeparator != null)
                    {
                        buffer += newstr;
                        int i;
                        do
                        {
                            i = buffer.IndexOf(dataSeparator);
                            if (i > 0)
                            {
                                StringRecived(so.Ident, buffer.Substring(0, i));
                                buffer = buffer.Remove(0, i + dataSeparator.Length);
                            }
                        } while (i > 0);
                    }
                    else
                        StringRecived(so.Ident, newstr);
                    Buffers[so.StreamReader] = buffer;
                    if (prevbyte == 0)
                    {
                        so.byteoffset = 0;
                        s.BeginRead(so.buffer, 0, StreamReaderObject.BUFFER_SIZE, Read_Callback, so);
                    }
                    else
                    {
                        so.byteoffset = 1;
                        so.buffer[0] = prevbyte;
                        s.BeginRead(so.buffer, 1, StreamReaderObject.BUFFER_SIZE - 1, Read_Callback, so);
                    }

                }
            }
            catch
            {
                Remove(so.Ident);
            }
        }

        StreamReader getReader(string id)
        {
            foreach (StreamReader sr in StreamReaders.Keys)
                if (StreamReaders[sr] == id)
                    return sr;
            return null;
        }

        /*public string ReadString(string id, int size_in_bytes)
        {
            StreamReader sr = getReader(id);
            Stream s = sr.BaseStream;
            s.EndRead(null);
            byte[] buffer = new byte[size_in_bytes];
            s.Read(buffer, 0, size_in_bytes);
            string str = StreamEncodings[id].GetString(buffer, 0, size_in_bytes);
            StreamReaderObject o = new StreamReaderObject(id, sr);
            sr.BaseStream.BeginRead(o.buffer, 0, StreamReaderObject.BUFFER_SIZE, Read_Callback, o);
            return str;
        }*/

        public int RB(StreamReader sr, char[] cc, int from, int count)
        {
            //sr.ReadLine();
            return 0;
        }

        public void Add(StreamReader sr, string id, Encoding Encoding)
        {
            StreamReaders.Add(sr, id);
            Buffers.Add(sr, "");
            if(StreamEncodings.ContainsKey(id))
                StreamEncodings[id] = Encoding;
            else
                StreamEncodings.Add(id, Encoding);
            StreamReaderObject o = new StreamReaderObject(id, sr);
            sr.BaseStream.BeginRead(o.buffer, 0, StreamReaderObject.BUFFER_SIZE, Read_Callback, o);
        }

        public void SetEncoding(string id, Encoding Encoding)
        {
            StreamEncodings[id] = Encoding;
        }

        public Encoding GetEncoding(string id)
        {
            return StreamEncodings[id];
        }

        public void RemoveAll()
        {
            StreamReaders.Clear();
            Buffers.Clear();
        }
        public void Remove(string id)
        {
            StreamEncodings.Remove(id);
            List<StreamReader> to_delete = new List<StreamReader>();
            foreach (StreamReader sr in StreamReaders.Keys)
                if (StreamReaders[sr] == id)
                    to_delete.Add(sr);
            foreach (StreamReader sr in to_delete)
            {
                Buffers.Remove(sr);
                StreamReaders.Remove(sr);
            }
        }

    }
}
