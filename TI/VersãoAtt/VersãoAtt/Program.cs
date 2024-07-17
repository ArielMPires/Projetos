using System;
using System.IO;
using System.Net;

namespace Atualizar
{
    class Program
    {

        public static void Main()
        {
            Console.WriteLine("Atualizando");

            string sf = @"WCS.dll";
            string df = @"backup\WCS.dll";
            if (System.IO.File.Exists(df))
            {
                // Use a try block to catch IOExceptions, to
                // handle the case of the file already being
                // opened by another process.
                try
                {
                    System.IO.File.Delete(df);
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }

            System.IO.File.Move(sf, df);

            string sf2 = @"WCS.pdb";
            string df2 = @"backup\WCS.pdb";
            if (System.IO.File.Exists(df2))
            {
                // Use a try block to catch IOExceptions, to
                // handle the case of the file already being
                // opened by another process.
                try
                {
                    System.IO.File.Delete(df2);
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }

            System.IO.File.Move(sf2, df2);

            string sf3 = @"WCS.exe";
            string df3 = @"backup\WCS.exe";
            if (System.IO.File.Exists(df3))
            {
                // Use a try block to catch IOExceptions, to
                // handle the case of the file already being
                // opened by another process.
                try
                {
                    System.IO.File.Delete(df3);
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }

            System.IO.File.Move(sf3, df3);


            string usuario = "AdmFTP";
                string senha = "@Sys#1256A";
                string url = "ftp://192.168.1.68/WCS.exe";
                string url2 = "ftp://192.168.1.68/WCS.pdb";
                string url3 = "ftp://192.168.1.68/WCS.dll";

                try
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(url2));
                    request.Method = WebRequestMethods.Ftp.DownloadFile;
                    request.Credentials = new NetworkCredential(usuario, senha);
                    request.UseBinary = true;
                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        using (Stream rs = response.GetResponseStream())
                        {
                            using (FileStream ws = new FileStream(@"WCS.pdb", FileMode.Create))
                            {
                                byte[] buffer = new byte[2048];
                                int bytesRead = rs.Read(buffer, 0, buffer.Length);
                                while (bytesRead > 0)
                                {
                                    ws.Write(buffer, 0, bytesRead);
                                    bytesRead = rs.Read(buffer, 0, buffer.Length);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    throw;
                }
                try
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(url3));
                    request.Method = WebRequestMethods.Ftp.DownloadFile;
                    request.Credentials = new NetworkCredential(usuario, senha);
                    request.UseBinary = true;
                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        using (Stream rs = response.GetResponseStream())
                        {
                            using (FileStream ws = new FileStream(@"WCS.dll", FileMode.Create))
                            {
                                byte[] buffer = new byte[2048];
                                int bytesRead = rs.Read(buffer, 0, buffer.Length);
                                while (bytesRead > 0)
                                {
                                    ws.Write(buffer, 0, bytesRead);
                                    bytesRead = rs.Read(buffer, 0, buffer.Length);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    throw;
                }

            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(url));
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(usuario, senha);
                request.UseBinary = true;
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    using (Stream rs = response.GetResponseStream())
                    {
                        using (FileStream ws = new FileStream(@"WCS.exe", FileMode.Create))
                        {
                            byte[] buffer = new byte[2048];
                            int bytesRead = rs.Read(buffer, 0, buffer.Length);
                            while (bytesRead > 0)
                            {
                                ws.Write(buffer, 0, bytesRead);
                                bytesRead = rs.Read(buffer, 0, buffer.Length);
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            Console.WriteLine("Atualizado");

            System.Diagnostics.Process.Start(@"WCS.exe");

            return;

        }
    }
}
