using FluentFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TI
{
    class ConexaoPFtp
    {

            public static void EnviarArquivoFTP(string newfile, string protocolo)
            {
                string usuario = "protocolo";
                string senha = "";
                string url = "ftp://192.168.1.68/" + protocolo + ".pdf";
                try
                {
                    FileInfo arquivoInfo = new FileInfo(newfile);
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(url));
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential(usuario, senha);
                    request.UseBinary = true;
                    request.ContentLength = arquivoInfo.Length;
                    using (FileStream fs = arquivoInfo.OpenRead())
                    {
                        byte[] buffer = new byte[2048];
                        int bytesSent = 0;
                        int bytes = 0;
                        using (Stream stream = request.GetRequestStream())
                        {
                            while (bytesSent < arquivoInfo.Length)
                            {
                                bytes = fs.Read(buffer, 0, buffer.Length);
                                stream.Write(buffer, 0, bytes);
                                bytesSent += bytes;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static void BaixarArquivoFTP(string arquivo)
            {
                string usuario = "protocolo";
                string senha = "";
                string url = "ftp://192.168.1.68/" + arquivo + ".pdf";
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
                            using (FileStream ws = new FileStream(@"Protocolos\" + arquivo + ".pdf", FileMode.Create))
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
            }
    }
}

