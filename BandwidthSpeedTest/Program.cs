using System.Diagnostics;

Console.WriteLine("Testing download speed...");
double downloadSpeed = TestDownloadSpeed();
Console.WriteLine("Testing upload speed...");
//double uploadSpeed = TestUploadSpeed();
double uploadSpeed = 0;

AppendResultsToCSV(downloadSpeed, uploadSpeed);


static double TestDownloadSpeed()
{
    using (HttpClient client = new HttpClient())
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        var response = client.GetByteArrayAsync("http://ipv4.download.thinkbroadband.com/5MB.zip").Result;
        stopwatch.Stop();

        double bytesPerSecond = 5000000 / stopwatch.Elapsed.TotalSeconds;
        double speed = bytesPerSecond * 8 / 1000000;
        Console.WriteLine("Download speed: {0:0.00} Mbps", speed);
        return speed;
    }
}

static double TestUploadSpeed()
{
    using (HttpClient client = new HttpClient())
    {
        byte[] data = new byte[5000000];
        Stopwatch stopwatch = Stopwatch.StartNew();
        var content = new ByteArrayContent(data);
        var response = client.PostAsync("http://ipv4.upload.thinkbroadband.com/", content).Result;
        stopwatch.Stop();

        double bytesPerSecond = 5000000 / stopwatch.Elapsed.TotalSeconds;
        double speed = bytesPerSecond * 8 / 1000000;
        Console.WriteLine("Upload speed: {0:0.00} Mbps", speed);
        return speed;
    }
}

static void AppendResultsToCSV(double downloadSpeed, double uploadSpeed)
{
    string filePath = "BandwidthTestResults.csv";

    if (!File.Exists(filePath))
    {
        File.WriteAllText(filePath, "Date,Download Speed (Mbps),Upload Speed (Mbps)\n");
    }

    string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    string line = string.Format("{0},{1:0.00},{2:0.00}\n", date, downloadSpeed, uploadSpeed);
    File.AppendAllText(filePath, line);
}
