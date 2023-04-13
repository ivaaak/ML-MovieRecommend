using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using MovieRecommend.API.Data;
using MovieRecommend.API.Data.Structures;
using System.Reflection;
using System.Text;

namespace MovieRecommend.API.ML
{
    public class MLService : IMLService
    {
        private readonly MLContext mlcontext;

        private EstimatorChain<ValueToKeyMappingTransformer> dataProcessingPipelineResult;
        private EstimatorChain<Microsoft.ML.Trainers.Recommender.MatrixFactorizationPredictionTransformer> trainingPipeLine;

        private IDataView? trainingDataView { get; set; }
        private StringBuilder resultString { get; set; }

        public MLService()
        {
            mlcontext = new MLContext();

            this.resultString = new StringBuilder();
        }

        public string RunModel()
        {

            this.trainingDataView = LoadDataFromDb();

            this.dataProcessingPipelineResult = TransformData();

            var model = TrainAndReturnModel();

            var evaluationResultString = EvaluateModelPerformance(model);

            GetMovieRatingPrediction(model, 1, 1);

            return resultString.ToString();
        }

        public APIResultDTO RunModelWithParams(float movieID, float userID)
        {
            var resultObject = new APIResultDTO();

            this.trainingDataView = LoadDataFromDb();
            this.dataProcessingPipelineResult = TransformData();
            var model = TrainAndReturnModel();
            var evaluationResultString = EvaluateModelPerformance(model);

            resultObject = GetMovieRatingPrediction(model, movieID, userID);
            // make this return a DTO

            return resultObject;
        }

        public IDataView LoadDataFromDb()
        {
            //STEP 2: Read the training data which will be used to train the movie recommendation model    
            //The schema for training data is defined by type 'TInput' in LoadFromTextFile<TInput>() method.

            var trainingDataView = mlcontext.Data
                .LoadFromTextFile<MovieRating>(
                    GlobalConstants.TrainingDataPath,
                    hasHeader: true,
                    separatorChar: ',');

            return trainingDataView;
        }

        public EstimatorChain<ValueToKeyMappingTransformer> TransformData()
        {
            //STEP 3: Transform your data by encoding the two features userId and movieID.
            // These encoded features will be provided as input to our MatrixFactorizationTrainer.
            var dataProcessingPipeline = mlcontext
                .Transforms.Conversion
                .MapValueToKey(
                    outputColumnName: "userIdEncoded",
                    inputColumnName: nameof(MovieRating.userId))
                .Append(mlcontext.Transforms.Conversion
                    .MapValueToKey(
                    outputColumnName: "movieIdEncoded",
                    inputColumnName: nameof(MovieRating.movieId))
                 );

            return dataProcessingPipeline;
        }

        public MatrixFactorizationTrainer.Options GenerateOptionsObject()
        {
            //Specify the options for MatrixFactorization trainer            
            MatrixFactorizationTrainer.Options options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "userIdEncoded",
                MatrixRowIndexColumnName = "movieIdEncoded",
                LabelColumnName = "Label",
                NumberOfIterations = 50,
                ApproximationRank = 100
            };

            return options;
        }

        public void GeneratePipeline() // currently unused
        {
            //STEP 4: Create the training pipeline 
            //var trainingPipeLine = dataProcessingPipelineResult
               // .Append(mlcontext.Recommendation().Trainers.MatrixFactorization(options));

        }

        public ITransformer TrainAndReturnModel()
        {
            var options = GenerateOptionsObject(); // ADD VARIABLES?

            //STEP 4: Create the training pipeline 
            this.trainingPipeLine = dataProcessingPipelineResult
                .Append(mlcontext.Recommendation().Trainers.MatrixFactorization(options));


            //STEP 5: Train the model fitting to the DataSet
            Console.WriteLine("=============== Training the model ===============");
            this.resultString.AppendLine("=============== Training the model ===============");

            // HERE ITERATIONS GET PRINTED OUT
            ITransformer model = trainingPipeLine.Fit(trainingDataView);

            return model;
        }

        public string EvaluateModelPerformance(ITransformer model)
        {
            //STEP 6: Evaluate the model performance 
            string evaluateTemplateLine = "=============== Evaluating the model ===============";
            Console.WriteLine(evaluateTemplateLine);
            resultString.AppendLine(evaluateTemplateLine);


            IDataView testDataView = mlcontext.Data.LoadFromTextFile<MovieRating>(GlobalConstants.TestDataPath, hasHeader: true, separatorChar: ',');
            var prediction = model.Transform(testDataView);
            var metrics = mlcontext.Regression.Evaluate(prediction, labelColumnName: "Label", scoreColumnName: "Score");

            var evalResultString = ("The model evaluation metrics RootMeanSquaredError:" + metrics.RootMeanSquaredError);
            Console.WriteLine(evalResultString);
            resultString.AppendLine(evalResultString);

            return resultString.ToString();
        }

        public APIResultDTO GetMovieRatingPrediction(ITransformer model, float movieID, float userID)
        {
            var result = new APIResultDTO();
            //STEP 7:  Try/test a single prediction by predicting a single movie rating for a specific user
            var PredictionEngine = mlcontext.Model.CreatePredictionEngine<MovieRating, MovieRatingPrediction>(model);
            /* Make a single movie rating prediction, the scores are for a particular user and will range from 1 - 5. 
               The higher the score the higher the likelyhood of a user liking a particular movie.
               You can recommend a movie to a user if say rating > 3.5.
            var movieRatingPrediction = PredictionEngine.Predict(
                new MovieRating()
                {   //Example rating prediction for userId = 6, movieId = 10 (GoldenEye)
                    userId = GlobalConstants.predictionuserId,
                    movieId = GlobalConstants.predictionmovieId
                }
            );*/

            var parametrizedMovieRatingPrediction = PredictionEngine.Predict(
                new MovieRating()
                {   // prediction based off the params
                    userId = userID,
                    movieId = movieID
                }
            );

            Movie movieService = new Movie();
            var movieTitleResult = movieService.Get((int)movieID).movieTitle;

            string resultStringTemplate = (
                "For userId: " + GlobalConstants.predictionuserId
                + Environment.NewLine 
                + "Movie rating prediction (1 - 5 stars) for movie: "
                + Environment.NewLine
                + movieTitleResult
                + " : " + Math.Round(parametrizedMovieRatingPrediction.Score, 1));

            Console.WriteLine(resultStringTemplate);
            resultString.AppendLine(resultStringTemplate);
            resultString.AppendLine("=============== End of process ===============");

            result.movieTitleInputted = movieTitleResult;
            result.predictedRatingResult = Math.Round(parametrizedMovieRatingPrediction.Score, 1).ToString();
            result.userID = userID.ToString();
            result.evaluationMetricsError = parametrizedMovieRatingPrediction.ToString();

            //return resultString.ToString();
            return result;
        }
    }
}
