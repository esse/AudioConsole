using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using Nancy.Hosting.Self;
using CoreAudioApi;


namespace AudioConsole
{

    public class MainModule : NancyModule
    {

        public MMDevice defaultDevice;

        public static float validateVol(float val) {
            if (val > 1) { return (float)1; }
            if (val < 0) { return (float)0; }
            return val;
        }


        public MainModule()
        {
            MMDeviceEnumerator devEnum = new MMDeviceEnumerator();
            defaultDevice = devEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);

            string html1 = "<html><body>Current volume: ";
            string html2 = "%.<br /><br /><a href='/p5'>+5</a><br /><a href='/p10'>+10</a><br /><a href='/m5'>-5</a><br /><a href='/m10'>-10</a>";



            Get["/"] = parameters =>
            {
                Console.WriteLine("/");
                return html1 + (defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100).ToString() + html2;
            };

            Get["/p5"] = parameters =>
                {
                    Console.WriteLine("+5");
                    defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar = validateVol(defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar + (float)0.05);
                    return html1 + (defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100).ToString() + html2;
                };

            Get["/p10"] = parameters =>
            {
                Console.WriteLine("+10");
                defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar = validateVol(defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar + (float)0.1);
                return html1 + (defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100).ToString() + html2;
            };

            Get["/m5"] = parameters =>
            {
                Console.WriteLine("-5");
                defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar = validateVol(defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar - (float)0.05);
                return html1 + (defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100).ToString() + html2;
            };

            Get["/m10"] = parameters =>
            {
                Console.WriteLine("-10");
                defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar = validateVol(defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar - (float)0.1);
                return html1 + (defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100).ToString() + html2;
            };
        }
    }

    class Program : NancyModule
    {
        static void Main(string[] args)
        {
            
            System.Console.WriteLine("Starting AudioConsole");
            Uri url;
            if (args.Length < 1)
            {
                url = new Uri("http://" +
                               "localhost" +
                               ":" + "12340");
            }
            else
            {
                url = new Uri("http://" +
                              "localhost" +
                              ":" + args[0]);
            }
            using (var host = new NancyHost(url))
            {
                host.Start();
                System.Console.WriteLine("Started HTTP Server on " + url);
                while (true) { }
            }
        }
    }
}
