using System;

namespace SlackClientTest
{
    public static class Helpers
    {
        public static string Random
        {
            get
            {
                var guidSplit = Guid.NewGuid().ToString().Split('-');
                return guidSplit[new Random().Next(0, 4)];
            }
        }
        public static Uri RandomUri
        {
            get
            {
                return new Uri("http://" + Random + ".com/");
            }
        }

        public static Uri RandomUriFrom(string randomString)
        {
            return new Uri("http://" + randomString + ".com/");
        }
        public static string RandomJson
        {
            get { return string.Format("\"{0}\":\"{1}\"", Random, Random); }
        }
        public static int RandomNumber(int min = 0, int max = 100000)
        {
            return new Random().Next(min, max);
        }
    }
}
