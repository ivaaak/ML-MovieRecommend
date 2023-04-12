﻿using Microsoft.ML;
using Microsoft.ML.Trainers;
using MovieRecommend.API.Data.Structures;
using System.Text;

namespace MovieRecommend.API.ML
{
    public class MLService : IMLService
    {
        private static string ModelsRelativePath = @"C:/.NET/ML-MovieRecommend/MovieRecommend.API/Data/Structures";
        public static string DatasetsRelativePath = @"C:/.NET/ML-MovieRecommend/MovieRecommend.API/Data/Sets";

        private static string TrainingDataRelativePath = $"{DatasetsRelativePath}/recommendation-ratings-train.csv";
        private static string TrainingDataPath = $"C:/.NET/ML-MovieRecommend/MovieRecommend.API/Data/Sets/recommendation-ratings-train.csv";
        private static string TestDataRelativePath = $"{DatasetsRelativePath}/recommendation-ratings-test.csv";
        private static string TestDataPath = $"C:/.NET/ML-MovieRecommend/MovieRecommend.API/Data/Sets/recommendation-ratings-test.csv";

        private static string TrainingDataLocation = GetAbsolutePath(TrainingDataRelativePath);
        
        private static string TestDataLocation = GetAbsolutePath(TestDataRelativePath);

        private static string ModelPath = GetAbsolutePath(ModelsRelativePath);

        private const float predictionuserId = 6;
        private const int predictionmovieId = 10;

        public string CalculateThings()
        {
            //STEP 1: Create MLContext to be shared across the model creation workflow objects 
            MLContext mlcontext = new MLContext();
            StringBuilder resultString = new StringBuilder();


            //STEP 2: Read the training data which will be used to train the movie recommendation model    
            //The schema for training data is defined by type 'TInput' in LoadFromTextFile<TInput>() method.
            IDataView trainingDataView = mlcontext.Data.LoadFromTextFile<MovieRating>(TrainingDataPath, hasHeader: true, separatorChar: ',');


            //STEP 3: Transform your data by encoding the two features userId and movieID. These encoded features will be provided as input
            //        to our MatrixFactorizationTrainer.
            var dataProcessingPipeline = mlcontext.Transforms.Conversion.MapValueToKey(outputColumnName: "userIdEncoded", inputColumnName: nameof(MovieRating.userId))
                           .Append(mlcontext.Transforms.Conversion.MapValueToKey(outputColumnName: "movieIdEncoded", inputColumnName: nameof(MovieRating.movieId)));

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
            // Get the output in a variable:
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            ITransformer model = trainingPipeLine.Fit(trainingDataView);
            stringWriter.Flush();
            string output = stringWriter.ToString();
            resultString.AppendLine(output);
            Console.SetOut(Console.Out);
            Console.WriteLine("the output is: ......" + output);


            //STEP 6: Evaluate the model performance 
            Console.WriteLine("=============== Evaluating the model ===============");
            resultString.AppendLine("=============== Evaluating the model ===============");
            IDataView testDataView = mlcontext.Data.LoadFromTextFile<MovieRating>(TestDataPath, hasHeader: true, separatorChar: ',');
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
                {
                    //Example rating prediction for userId = 6, movieId = 10 (GoldenEye)
                    userId = predictionuserId,
                    movieId = predictionmovieId
                }
            );

            Movie movieService = new Movie();
            Console.WriteLine("For userId:" + predictionuserId + " movie rating prediction (1 - 5 stars) for movie:" 
                + movieService.Get(predictionmovieId).movieTitle + " is:" + Math.Round(movieratingprediction.Score, 1));
            Console.WriteLine("=============== End of process, hit any key to finish ===============");

            resultString.AppendLine("For userId:" + predictionuserId + " movie rating prediction (1 - 5 stars) for movie:"
                + movieService.Get(predictionmovieId).movieTitle + " is:" + Math.Round(movieratingprediction.Score, 1));
            resultString.AppendLine("=============== End of process, hit any key to finish ===============");
            return resultString.ToString();
        }

        public static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }
    }
}
