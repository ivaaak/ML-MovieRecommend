namespace MovieRecommend.API.Data.Structures
{
    public class APIResultDTO
    {
        public string? evaluationMetricsError { get; set; }
        public string? movieTitleInputted { get; set; }
        public string? userID { get; set; }
        public string? predictedRatingResult { get; set; }

    }
}
