using System;
using System.IO;

namespace NetProjector.Core
{
    public class ProjectorServer : HttpServer
    {
        private static readonly int RefershInterval = 500;
        private static readonly string ImageFormat = "jpeg";
        private static readonly string page = @"<!Doctype html><html xmlns=http://www.w3.org/1999/xhtml><head><script language='javascript'>window.onload = function() {var image = document.getElementById('screen');function refreshImage() {image.src = 'screen/" + ImageFormat + "/'+Math.random();}setInterval(refreshImage, " + RefershInterval + ");}</script></head><body><img src='screen/" + ImageFormat + "/temp' id='screen' style='position: absolute;margin: auto;left: 0;right: 0;top: 0;bottom: 0;'></body></html>";

        private readonly Func<string, byte[]> CreateScreenImage;
        private readonly object SyncObj = new object();
        private volatile byte[] image = null;
        private DateTime lastTime = DateTime.Now.AddMilliseconds(-RefershInterval);

        private byte[] GetImage(string parameter)
        {
            if ((DateTime.Now - lastTime).TotalMilliseconds > RefershInterval)
            {
                lastTime = DateTime.Now;
                lock (SyncObj)
                {
                    if (CreateScreenImage != null)
                        image = CreateScreenImage(parameter);
                    else
                        image = null;
                }
            }
            return image;
        }

        public ProjectorServer(int port, Func<string, byte[]> getResponse)
            : base(port)
        {
            this.CreateScreenImage = getResponse;
        }

        public override void handleGETRequest(HttpProcessor p)
        {
            Console.WriteLine("request: {0}", p.http_url);

            if (p.http_url.StartsWith("/screen"))
            {
                var type = p.http_url.Split('/')[2];
                p.writeSuccess("image/" + type);
                var buffer = GetImage(type);
                if (buffer != null)
                {
                    p.outputStream.BaseStream.Write(buffer, 0, buffer.Length);
                    p.outputStream.BaseStream.Flush();
                }
                else
                {
                    p.writeFailure();
                }
            }
            else
            {
                p.writeSuccess();
                p.outputStream.WriteLine(page);
            }
        }

        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            Console.WriteLine("POST request: {0}", p.http_url);

            p.writeFailure();

            //string data = inputData.ReadToEnd();

            //p.writeSuccess();
            //p.outputStream.WriteLine("<html><body><h1>test server</h1>");
            //p.outputStream.WriteLine("<a href=/test>return</a><p>");
            //p.outputStream.WriteLine("postbody: <pre>{0}</pre>", data);
        }
    }
}
