using Microsoft.ML;
using Microsoft.ML.Trainers;
using MovieRecommend.API.Data.Structures;
using System.Text;
using MovieRecommend.API.Data;
using static Microsoft.ML.Data.DataDebuggerPreview;
using System.Diagnostics;

namespace MovieRecommend.API.ML
{
    public class MLService : IMLService
    {
        private readonly MLContext mlcontext;

        public MLService()
        {
            mlcontext = new MLContext();
        }
        public string CalculateThings()
        {
            //STEP 1: Create MLContext to be shared across the model creation workflow objects 
            StringBuilder resultString = new StringBuilder();


            //STEP 2: Read the training data which will be used to train the movie recommendation model    
            //The schema for training data is defined by type 'TInput' in LoadFromTextFile<TInput>() method.
            IDataView trainingDataView = mlcontext.Data.LoadFromTextFile<MovieRating>(GlobalConstants.TrainingDataPath, hasHeader: true, separatorChar: ',');


            //STEP 3: Transform your data by encoding the two features userId and movieID. These encoded features will be provided as input
            //        to our MatrixFactorizationTrainer.
            var dataProcessingPipeline = mlcontext
                .Transforms.Conversion
                .MapValueToKey(
                    outputColumnName: "userIdEncoded", 
                    inputColumnName: nameof(MovieRating.userId))
                .Append(mlcontext.Transforms.Conversion
                    .MapValueToKey(
                    outputColumnName: "movieIdEncoded", 
                    inputColumnName: nameof(MovieRating.movieId)));

            //Specify the options for MatrixFactorization trainer            
            MatrixFactorizationTrainer.Options options = new MatrixFactorizationTrainer.Options();
            options.MatrixColumnIndexColumnName = "userIdEncoded";
            options.MatrixRowIndexColumnName = "movieIdEncoded";
            options.LabelColumnName = "Label";
            options.NumberOfIterations = 50;
            options.ApproximationRank = 100;


            //STEP 4: Create the training pipeline 
            var trainingPipeLine = dataProcessingPipeline.Append(mlcontext.Recommendation().Trainers.MatrixFactorization(options));


            //STEP 5: Train the model fitting to the DataSet
            Console.WriteLine("=============== Training the model ===============");
            resultString.AppendLine("=============== Training the model ===============");

            // HERE ITERATIONS GET PRINTED OUT
            ITransformer model = trainingPipeLine.Fit(trainingDataView);


            //STEP 6: Evaluate the model performance 
            Console.WriteLine("=============== Evaluating the model ===============");
            resultString.AppendLine("=============== Evaluating the model ===============");


            IDataView testDataView = mlcontext.Data.LoadFromTextFile<MovieRating>(GlobalConstants.TestDataPath, hasHeader: true, separatorChar: ',');
            var prediction = model.Transform(testDataView);
            var metrics = mlcontext.Regression.Evaluate(prediction, labelColumnName: "Label", scoreColumnName: "Score");

            Console.WriteLine("The model evaluation metrics RootMeanSquaredError:" + metrics.RootMeanSquaredError);
            resultString.AppendLine("The model evaluation metrics RootMeanSquaredError:" + metrics.RootMeanSquaredError);



            //STEP 7:  Try/test a single prediction by predicting a single movie rating for a specific user
            var predictionengine = mlcontext.Model.CreatePredictionEngine<MovieRating, MovieRatingPrediction>(model);
            /* Make a single movie rating prediction, the scores are for a particular user and will range from 1 - 5. 
               The higher the score the higher the likelyhood of a user liking a particular movie.
               You can recommend a movie to a user if say rating > 3.5.*/
            var movieratingprediction = predictionengine.Predict(
                new MovieRating()
                {   //Example rating prediction for userId = 6, movieId = 10 (GoldenEye)
                    userId = GlobalConstants.predictionuserId,
                    movieId = GlobalConstants.predictionmovieId
                }
            );

            Movie movieService = new Movie();
            Console.WriteLine("For userId:" + GlobalConstants.predictionuserId 
                + " movie rating prediction (1 - 5 stars) for movie:" 
                + movieService.Get(GlobalConstants.predictionmovieId).movieTitle 
                + " is:" + Math.Round(movieratingprediction.Score, 1));
            Console.WriteLine("=============== End of process ===============");
            resultString.AppendLine(
                "For userId:" + GlobalConstants.predictionuserId 
                + " movie rating prediction (1 - 5 stars) for movie:"
                + movieService.Get(GlobalConstants.predictionmovieId).movieTitle 
                + " is:" + Math.Round(movieratingprediction.Score, 1));


            resultString.AppendLine("=============== End of process ===============");
            return resultString.ToString();
        }
    }
}
