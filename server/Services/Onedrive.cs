// using Microsoft.Graph;
// using Microsoft.Graph.Communications.Common;
// using Microsoft.Graph.Models;
// using Microsoft.Identity.Client;
// using ConfigurationManager = System.Configuration.ConfigurationManager;
//
// namespace server.Services;
//
// public class Onedrive(string clientId, string clientSecret, string scope)
// {
//     private const string _tenantId = "4f765ca1-9603-4f8d-b1b5-ee9ca5668c0e";
//     private string _clientId = clientId;
//     private string _clientSecret = clientSecret;
//     private readonly string[] scopes = [scope];
//
//
//     private async Task<string> GetAccessToken()
//     {
//         var client = ConfidentialClientApplicationBuilder.Create(clientId).WithClientSecret(clientSecret)
//             .WithAuthority(new Uri($"https://login.microsoftonline.com/{_tenantId}")).Build();
//         var authResult = await client.AcquireTokenForClient(scopes).ExecuteAsync();
//         return authResult.AccessToken;
//     }
//     
//     public async Task UploadFileToOneDrive(string filePath)
//     {
//         string accessToken = await GetAccessToken();
//         var graphClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
//         {
//             requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
//             return Task.CompletedTask;
//         }));
//
//         using (var fileStream = new FileStream(filePath, FileMode.Open))
//         {
//             var driveItem = await graphClient.Me.Drive. Root.ItemWithPath(Path.GetFileName(filePath)).Content.Request().PutAsync<DriveItem>(fileStream);
//             Console.WriteLine($"File uploaded to OneDrive with ID: {driveItem.Id}");
//         }
//     }
// }

