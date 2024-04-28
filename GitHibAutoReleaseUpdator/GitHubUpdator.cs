using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GitHibAutoReleaseUpdator
{
    internal class GitHubUpdator
    {
        private static Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

        private static HttpClient client = new HttpClient(new HttpClientHandler()
        {
            AllowAutoRedirect = false
        });

        public static async Task CheckUpdates(string[] args, string username,string repo)
        {
            Console.WriteLine(currentVersion.ToString());

            Console.WriteLine(currentVersion.Major);
            Console.WriteLine(currentVersion.Minor);
            Console.WriteLine(currentVersion.Build);
            Console.WriteLine(currentVersion.MinorRevision);


            Console.WriteLine(currentVersion.MajorRevision);
            Console.WriteLine(currentVersion.Revision);

            using (HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get,$"https://github.com/{username}/{repo}/releases/latest"))
            {
                using (HttpResponseMessage res = await client.SendAsync(req))
                {
                    string gitVersion = res.Headers.Location.ToString().Split('/').Last();

                    Version remoteVersion = ParseVersion(gitVersion);

                    
                    Console.WriteLine(res.Headers.Location);


                    string[] attachements = await GetReleaseAssets(username, repo, gitVersion);

                    foreach (var asset in attachements)
                    {
                        Console.WriteLine(asset);
                    }
                }
            }
        }

        public static Version ParseVersion(string input)
        {
            short[] version = new short[4];
            short index = 0;
            foreach (var chunk in input.Split('.')) version[index++] = short.Parse(chunk);
            return Version.Parse(string.Join(".", version));
        }

        private static async Task<string[]> GetReleaseAssets(string username,string repo, string version)
        {
            using (HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, $"https://github.com/{username}/{repo}/releases/expanded_assets/{version}"))
            {
                using (HttpResponseMessage res = await client.SendAsync(req))
                {
                    string assetsData = await res.Content.ReadAsStringAsync();

                    return assetsData.Split('\n')
                        .Select(l => l.TrimStart())
                        .Where(l => l.StartsWith("<a href=\"", StringComparison.InvariantCultureIgnoreCase))
                        .Select(l => "https://github.com" + l.Split('\"')[1]).ToArray();
                }
            }
        }
    }
}
