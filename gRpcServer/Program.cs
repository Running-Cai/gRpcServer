using AIServer;
using Grpc.Core;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace gRpcServer
{
    internal class Program
    {
        static int Port = 9007;
        static string address = "127.0.0.1";
        private const int GRPC_MAX_RECEIVE_MESSAGE_LENGTH = (20 * 1024 * 1024);
        static void Main(string[] args)
        {
            try
            {
                if (File.Exists("system.txt"))
                {
                    var readr = File.ReadAllLines("system.txt");
                    address = readr[0];
                    Port = Convert.ToInt32(readr[1]);
                }
                else
                {
                    using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(@"system.txt", false))
                    {
                        file.WriteLine(address);
                        file.WriteLine(Port);
                    }
                }
                //GetNetAndClass("AUO_Foreign_v11", out string modelpath, out List<string> classNames,
                //   out double train_Width, out double train_Height, out string dirpath, out string net_name, out string ouput_name);


                //inference = new Inference(modelpath, new OpenCvSharp.Size(train_Width, train_Height), 0.8f, classNames);

                var channelOptions = new List<ChannelOption>();
                channelOptions.Add(new ChannelOption(ChannelOptions.MaxReceiveMessageLength, GRPC_MAX_RECEIVE_MESSAGE_LENGTH));
                gRPCImpl gRPCImpl = new gRPCImpl();
                Server server = new Server(channelOptions)
                {
                    Services = { AIGrpcService.BindService(gRPCImpl) },
                    Ports = { new ServerPort(address, Port, ServerCredentials.Insecure) },
                };

                server.Start();
                Console.WriteLine("gRPC server listening on IP " + address + " Rort:" + Port);
                Console.ReadKey();
                server.ShutdownAsync().Wait();
            }
            catch (Exception ex)
            {

                Console.WriteLine( ex);
                Console.ReadKey();
            }

            
        }

        private static void GetNetAndClass(string netpath, out string modelpath,
          out List<string> classNames, out double train_Width, out double train_Height, out string dirpath,
          out string net_name, out string ouput_name)
        {
            modelpath = $"{Environment.CurrentDirectory}\\{netpath}\\best.onnx";
            var classFile = $"{Environment.CurrentDirectory}\\{netpath}\\class_names.txt";

            var classText = File.ReadAllText(classFile);
            var lines = classText.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            classNames = lines[0].Split(',').ToList();

            train_Width = double.Parse(lines[1].Split(',')[0]);
            train_Height = double.Parse(lines[1].Split(',')[1]);
            dirpath = lines[2];
            net_name = lines[3].Split(':')[1];
            ouput_name = lines[4].Split(':')[1];
        }

    }
}
