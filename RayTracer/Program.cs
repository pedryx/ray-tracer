using System;

using Utils;

namespace RayTracer;

class Program
{
    private const int width = 600;
    private const int height = 450;
    private const string outputFile = "demo.pfm";

    private static void Main(string[] args)
    {
        var floatImage = new FloatImage(width, height, 3);
        floatImage.SavePFM(outputFile);

        Console.WriteLine("HDR image is finished.");
    }
}