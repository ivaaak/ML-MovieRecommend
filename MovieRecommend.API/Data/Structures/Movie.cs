using MovieRecommend.API.ML;

namespace MovieRecommend.API.Data.Structures
{
    public class Movie
    {
        public int movieId;

        public String movieTitle;

        public Lazy<List<Movie>> _movies = new Lazy<List<Movie>>(() => LoadMovieData(GlobalConstants.MoviesRelativePath));

        public Movie() { }

        public Movie Get(int id)
        {
            Movie result;
            try
            {
                 result = _movies.Value.Single(m => m.movieId == id);
            } 
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"No movie with the Id {movieId} was found");
            }

            return result;
        }

        private static List<Movie> LoadMovieData(String moviesdatasetpath)
        {
            var result = new List<Movie>();
            Stream fileReader = File.OpenRead(moviesdatasetpath);
            StreamReader reader = new StreamReader(fileReader);
            try
            {
                bool header = true;
                int index = 0;
                var line = "";
                while (!reader.EndOfStream)
                {
                    if (header)
                    {
                        line = reader.ReadLine();
                        header = false;
                    }
                    line = reader.ReadLine();
                    string[] fields = line.Split(',');
                    int movieId = Int32.Parse(fields[0].ToString().TrimStart(new char[] { '0' }));
                    string movieTitle = fields[1].ToString();
                    result.Add(new Movie() { movieId = movieId, movieTitle = movieTitle });
                    index++;
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
            }

            return result;
        }
    }
}
