using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;

namespace DesktopOrganizer
{
    public class Organizer : BackgroundService
    {
        private readonly ILogger<Organizer> _logger;

        private string directoryPath = @"C:\Users\User\Desktop";

        public Organizer(ILogger<Organizer> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Organizer running at: {time}", DateTimeOffset.Now);
                //get list of files in desktop
                string[] fileList = GetFileList(directoryPath);

                //loop files and action per file type
                ProcessFiles(fileList);

                _logger.LogInformation("Organizer finnished at: {time}", DateTimeOffset.Now);
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); //run once every 24 hours
            }
        }

        private string[] GetFileList(string directory)
        {
            string[] fileList;

            fileList = Directory.GetFiles(directory);

            return fileList;
        }

        private void ProcessFiles(string[] fileList)
        {
            foreach(string file in fileList)
            {
                if(file.Contains("mp4") || file.Contains("mkv"))
                {
                    _logger.LogInformation($"MOVED -> {file}", DateTimeOffset.Now);
                    //FileSystem.DeleteFile(file, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin); //sends to recycle bin permenently deletes sometimes

                    // Construct the full destination file path
                    string destinationFile = Path.Combine(@"C:\Users\User\Videos\Unorganized", Path.GetFileName(file));

                    // Move the file to the destination directory
                    File.Move(file, destinationFile);

                }
            }
        }
    }
}