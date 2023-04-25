using MovieRecommend.API.Data.Structures;

namespace MovieRecommend.API.ML
{
    public interface IMLService
    {
        public APIResultDTO RunModel();

        public APIResultDTO RunModelWithParams(float userID, float movieID);

        //public string GetAbsolutePath(string relativePath);
    }
}
