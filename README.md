# Invoke Lambda

Multiplatform mobile tool to invoke arbitrary AWS Lambda functions. Useful for debugging serverless applications. If you don't know what AWS Lambda is then you don't need this project.

This is a pretty simple project, and I have no plans to extend it much further since it does what I want. I just needed something to quickly and easily invoke AWS Lambda functions for testing. It currently only allows unauthenticated AWS Cognito profiles (authentication is about the only thing I plan to add).

## Quick start

Open in Visual Studio for PC or (untested) Xamarin Studio for Mac, build and deploy to your chosen device. Enter your AWS Cognito pool ID in the top box, and your Lambda function name in the next box. Optionally specify the payload in the large text box (must be JSON or the AWS SDK complains). Set the regions for your Cognito pool and the function (these can be different, it depends what you set up in AWS). Press "Send" and you should get the response or the error message.

## Overview

You could recreate this project quite easily:

1. Start with project template "Visual C# -> Cross-Platform -> Cross Platform App (Xamarin.Forms or Native)".
2. Select "Xamarin.Forms" for UI Technology, and "Shared Project" for Code Sharing Strategy (as far as I can tell the AWS SDK doesn't work with "Portable Class Library").
3. Use NuGet to add "AWSSDK.CognitoIdentity" and "AWSSDK.Lambda".
4. Copy the code additions in "InvokeLambda/InvokeLambda/MainPage.xaml" and "InvokeLambda/InvokeLambda/MainPage.xaml.cs". These are the only files of note.

I've only tested on Windows and Android, but there's no reason why iOS or Windows Phone shouldn't work.

