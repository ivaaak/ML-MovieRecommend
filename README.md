# ML-MovieRecommend:
## Predicting Movie Ratings by using [Matrix](https://en.wikipedia.org/wiki/Matrix_factorization_(recommender_systems)) [Factorization](https://developers.google.com/machine-learning/recommendation/collaborative/matrix):

## A full stack web project built with:
#### - Frontend - React (:3000)
#### - Backend - .NET Web API (:7033)
#### - Machine Learning - [ML.NET Library](https://dotnet.microsoft.com/en-us/apps/machinelearning-ai/ml-dotnet)
#### - Dataset - MovieLens Movie Database 

## Steps used by the Matrix Factorization Algorythm in the [MLService](https://github.com/ivaaak/ML-MovieRecommend/blob/main/MovieRecommend.API/ML/MLService.cs):
1.	The **LoadDataFromDb** method reads the training data from a CSV file and returns an **IDataView**. The schema for the training data is defined by type **MovieRating**.
2.	The **TransformData** method encodes the two features **userId** and **movieId**. These encoded features will be provided as input to the **MatrixFactorizationTrainer**.
3.	The **GenerateOptionsObject** method specifies the options for the **MatrixFactorizationTrainer**.
4.	The **GeneratePipeline** method creates the training pipeline.
5.	The **TrainAndReturnModel** method creates the training pipeline by appending the data processing pipeline and **MatrixFactorizationTrainer**. It fits the model to the training dataset and returns the trained model.
6.	The **EvaluateModelPerformance** method loads the test dataset, performs the prediction, and evaluates the model's performance. The model evaluation metrics include **RootMeanSquaredError**.
7.	The **GetMovieRatingPrediction** method makes a single movie rating prediction for a specific user. It uses the trained model to predict the rating for a particular movie for a user.



## Screenshots:

<img src="https://raw.githubusercontent.com/ivaaak/ML-MovieRecommend/813c4c2158eabf0c3c48bf8508ef24929a9256e4/MovieRecommend.Web/screenshots/screen1.png" width="80%"></img> 

<img src="https://raw.githubusercontent.com/ivaaak/ML-MovieRecommend/813c4c2158eabf0c3c48bf8508ef24929a9256e4/MovieRecommend.Web/screenshots/screen2.png" width="80%"></img> 

<img src="https://raw.githubusercontent.com/ivaaak/ML-MovieRecommend/813c4c2158eabf0c3c48bf8508ef24929a9256e4/MovieRecommend.Web/screenshots/screen3.png" width="80%"></img> 

## Examples of the Datasets Used:
recommendation-movies.csv (9743 movies)

| movieId | title           | genres |
|---------|-----------------|--------|
| 1       | Toy Story(1995) |Adventure Animation Children Comedy Fantasy|
| 2       |Jumanji (1995)|Adventure Children Fantasy|
| 3       | Grumpier Old Men (1995)|Comedy Romance|


recommendation-ratings-test.csv (20 records – just used as test values)

| userId | movieId | rating          | timestamp |
|--------|---------|-----------------|-----------|
| 1      |1097| 5 |964981680|
| 1      |1127| 4 |964982513|
| 1      |1136| 4 |964981327|


recommendation-ratings-train.csv (62369 records – used to train the model)

| userId | movieId | rating          | timestamp |
|--------|---------|-----------------|-----------|
| 1      |1| 4 |964982703|
| 2      |3| 3 |964981247|
| 3      |4| 5 |964982224|

