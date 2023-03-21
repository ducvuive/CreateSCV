using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Microsoft.AspNetCore.Mvc;

namespace CreateCSV.Controllers
{
    public class UploadFileToDriver : ControllerBase
    {
        private const string PathToServiceAccountKeyFile = @"E:\UIT\TestCopilot\CreateCSV\testcreatecsv-ad1adbce2299.json";
        private const string ServiceAccountEmail = "testcreatecsv1@testcreatecsv.iam.gserviceaccount.com";
        private const string UploadFileName = "Test hello12.txt";
        private const string DirectoryId = "1BXAwnZQJz3V3vartHdY9562aNPxUuVUD";

        // Load the Service account credentials and define the scope of its access.
        [HttpGet]
        [Route("upload")]
        public async Task<IActionResult> Upload()
        {
            // Load the Service account credentials and define the scope of its access.
            var credential = GoogleCredential.FromFile(PathToServiceAccountKeyFile)
                            .CreateScoped(DriveService.ScopeConstants.Drive);
            // Create the  Drive service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });
            // Upload file Metadata
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = UploadFileName,
                Parents = new List<string>() { DirectoryId }
            };
            // find the file we uploaded by name without delete


            var files = await service.Files.List().ExecuteAsync(CancellationToken.None);
            var uploadedFile = files.Files.FirstOrDefault(f => f.Name == UploadFileName);
            if (uploadedFile == null)
            {
                //Create a new file on Google Drive
                await using (var fsSource = new FileStream(UploadFileName, FileMode.Open, FileAccess.Read))
                {
                    // Create a new file, with metadata and stream.
                    var request = service.Files.Create(fileMetadata, fsSource, "txt/plain");
                    request.Fields = "*";
                    var results = await request.UploadAsync(CancellationToken.None);

                    if (results.Status == UploadStatus.Failed)
                    {
                        //Console.WriteLine($"Error uploading file: {results.Exception.Message}");
                        BadRequest(results.Exception.Message);
                    }
                }
            }
            else
            {
                //// Note: not all fields are writeable watch out, you cant just send uploadedFile back.
                var updateFileBody = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = "Test hello 123.txt",
                };

                //// Let's add some text to our file.
                var updateFileContent = "Hello World 123 123!";
                await System.IO.File.WriteAllTextAsync(UploadFileName, updateFileContent);


                //// Then upload the file again with a new name and new data.
                await using (var uploadStream = new FileStream(UploadFileName, FileMode.Open, FileAccess.Read))
                {
                    // Update the file id, with new metadata and stream, don't update file name.

                    var updateRequest = service.Files.Update(updateFileBody, uploadedFile.Id, uploadStream, "text/plain");
                    var result = await updateRequest.UploadAsync(CancellationToken.None);

                    if (result.Status == UploadStatus.Failed)
                    {
                        //Console.WriteLine($"Error uploading file: {result.Exception.Message}");
                        BadRequest(result.Exception.Message);
                    }
                }
            }
            return Ok();
        }
    }
}
