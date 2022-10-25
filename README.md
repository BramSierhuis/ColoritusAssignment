# ColoritusAssignment
This is the repository for a project I worked on for my IT study. 
I tried to structure the code the best I could and implemented the Design Patterns I know, even though that was far from neccesary for the actual assignment.
All code has been deployed to Azure using CI/CD Pipelines.

## The original Assignment was as follows:
Using azure functions create an http endpoint that accepts an image and starts a background job that will edit the image using the EditImage function from the ImageHelper_2022. After editing the image and getting the primary colors, use the api from thecolorapi.com to get one or more of its color names. Next use any other public api to get a text related to these names and use add the text on top of the image. 
The initial http endpoint will return a unique id or link to a different endpoint that either list that the job is still in progress or a link to all the generated image served from the blob storage.
A sample of code for writing text on an image can be found here.
https://github.com/wearetriple/InHolland-CCD/tree/master/ImageEditor_2022

### Must
- Expose publicly accessible API for uploading an image.
-	Employ QueueTrigger to start the job in a background so the initial call stays fast.
-	Employ Blob Storage to store all generated images and to expose the files.
-	Employ the color api to color names (thecolorapi.com)
-	Employ any public api for retrieving a text to write on top of the image related the color names. 
-	Expose a publicly accessible API for fetching the generated images using a HttpTrigger.
-	Provide exported Postman Collection as API documentation.
-	Create a fitting Bicep template (try to include the queues as well so you don’t have to use CreateQueueIfNotExists in your code).
-	Deploy code and have a working endpoint.
-	Deploy both the resources and code automatically from Repo using Azure Devops pipelines.
 
### Could
-	Use SAS token instead of publicly accessible blob storage for fetching finished image directly from Blob.
-	Deploy code using script (Azure CLI) and include the script in your repo.
-	Use authentication on request API. (Be sure to provide me with credentials)
-	Use Azure AD authentication on request API.
-	Employ multiple queues for the different workloads.
-	Provide status API for fetching processing status and saving status in a azure storage Table.
-	Other language than C# is allowed
