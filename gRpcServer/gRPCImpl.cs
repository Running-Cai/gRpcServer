using AIServer;
using Grpc.Core;
using OpenCvSharp;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace gRpcServer
{
    class gRPCImpl : AIGrpcService.AIGrpcServiceBase
    {
        //Utilities.YoloV5.Inference inference;
        public gRPCImpl()
        {
            //this.inference = inference;
        }
        public override Task<ProcessReply> ProcessImage(ProcessRequest request, ServerCallContext context)
        {
            var data = request.Iamges.ToArray();
            string ALLResult = "";
            var images = AITools.ImageConvert.ConvertByteArrayToMatList(data);
            for (int i = 0; i < images.Count; i++)
            {
                var colorImage = new Mat();
                Cv2.CvtColor(images[i], colorImage, ColorConversionCodes.GRAY2BGR);
                colorImage.SaveImage($"D:\\temp\\{DateTime.Now.ToString("HHmmss_fff")}.jpg");
                //var result3 = inference.RunInference(colorImage);
                //ALLResult += "|" + result3;
                colorImage.Dispose();
                images[i].Dispose();
            }
         
            Console.WriteLine("传输完成 收到图片");


            return Task.FromResult(new ProcessReply { Message= "传输完成cjt9527" });

        }

    }
}
