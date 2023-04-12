namespace MovieRecommend.API.Data
{
    public static class GlobalConstants
    {
        public static string ModelsRelativePath = @"C:/.NET/ML-MovieRecommend/MovieRecommend.API/Data/Structures";
        public static string DatasetsRelativePath = @"C:/.NET/ML-MovieRecommend/MovieRecommend.API/Data/Sets";

        public static string TrainingDataRelativePath = $"{DatasetsRelativePath}/recommendation-ratings-train.csv";
        public static string TrainingDataPath = $"C:/.NET/ML-MovieRecommend/MovieRecommend.API/Data/Sets/recommendation-ratings-train.csv";

        public static string TestDataRelativePath = $"{DatasetsRelativePath}/recommendation-ratings-test.csv";
        public static string TestDataPath = $"C:/.NET/ML-MovieRecommend/MovieRecommend.API/Data/Sets/recommendation-ratings-test.csv";


        public static string TrainingDataLocation = GetAbsolutePath(TrainingDataRelativePath);

        public static string TestDataLocation = GetAbsolutePath(TestDataRelativePath);

        public static string ModelPath = GetAbsolutePath(ModelsRelativePath);

        // predictor variables
        public const float predictionuserId = 6;
        public const int predictionmovieId = 10;



        public static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }
    }
}
