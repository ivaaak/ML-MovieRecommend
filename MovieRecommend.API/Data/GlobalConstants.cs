#nullable disable
using System.Diagnostics;

namespace MovieRecommend.API.Data
{
    public static class GlobalConstants
    {
        public static string TestData = Path.GetDirectoryName(
            Process.GetCurrentProcess().MainModule.FileName) + @"\Data\Sets\recommendation-ratings-test.csv";
        public static string TrainingData = Path.GetDirectoryName(
            Process.GetCurrentProcess().MainModule.FileName) + @"\Data\Sets\recommendation-ratings-train.csv";

        public static string MoviesRelativePath = Path.GetDirectoryName(
            Process.GetCurrentProcess().MainModule.FileName) + @"\Data\Sets\recommendation-movies.csv";
        public static string DataStructuresRelativePath = Path.GetDirectoryName(
            Process.GetCurrentProcess().MainModule.FileName) + @"\Data\Structures\";


        // predictor variables
        public const float predictionuserId = 6;
        public const int predictionmovieId = 10;



        public static string GetAbsolutePath(string relativePath)
        {

            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            Console.WriteLine($"assemblyFolderPath {assemblyFolderPath}");
            Console.WriteLine($"_dataRoot {_dataRoot}");
            Console.WriteLine($"fullPath {fullPath}");

            return fullPath;
        }
    }
}
