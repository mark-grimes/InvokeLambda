using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InvokeLambda
{
	public partial class MainPage : ContentPage
	{
		private Amazon.CognitoIdentity.CognitoAWSCredentials credentials = null;

		private void getCredentials()
		{
			Amazon.RegionEndpoint region = endpointFromName(input_IdentityPoolRegion.Items[input_IdentityPoolRegion.SelectedIndex]);
			credentials = new Amazon.CognitoIdentity.CognitoAWSCredentials(region.SystemName + ":" + input_identityPool.Text, region);
		}

		private Amazon.RegionEndpoint endpointFromName(string regionName)
		{
			foreach (var endpoint in Amazon.RegionEndpoint.EnumerableAllRegions)
			{
				if (endpoint.SystemName == regionName) return endpoint;
			}

			// regionName not found
			return null;
		}

		public MainPage()
		{
			InitializeComponent();
		}

		private void onButtonClicked(object sender, EventArgs e)
		{
			if (credentials == null) getCredentials();
			Amazon.RegionEndpoint endpoint = endpointFromName(input_region.Items[input_region.SelectedIndex]);
			Amazon.Lambda.AmazonLambdaClient client = new Amazon.Lambda.AmazonLambdaClient(credentials, endpoint);

			var invokeRequest = new Amazon.Lambda.Model.InvokeRequest { FunctionName = input_functionName.Text, InvocationType = "RequestResponse", Payload = input_payload.Text };
			System.Threading.Tasks.Task<Amazon.Lambda.Model.InvokeResponse> responseTask = client.InvokeAsync(invokeRequest);

			responseTask.ContinueWith((response) =>
			{
				string statusText = "";
				string informationalText = "Internal error: Unhandled branch point"; // Should always be set in the next "if"
				Xamarin.Forms.Color resultColour = Xamarin.Forms.Color.Red;

				if (response.IsCanceled) { statusText = "Cancelled"; informationalText = ""; resultColour = Xamarin.Forms.Color.Black; }
				else if (response.IsFaulted)
				{
					statusText = "Faulted";
					informationalText = response.Exception.Message;
					foreach (var exception in response.Exception.InnerExceptions)
					{
						informationalText += "\n" + exception.Message;
					}
				}
				else if (response.IsCompleted)
				{
					statusText = "Finished";
					var responseReader = new System.IO.StreamReader(response.Result.Payload);
					informationalText = responseReader.ReadToEnd();
					resultColour = Xamarin.Forms.Color.Black;
				}

				// This continuation is not run on the main UI thread, so need to set up
				// another task perform the UI changes on the correct thread.
				Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
				{
					output_status.Text = statusText;
					output_information.Text = informationalText;
					output_status.TextColor = resultColour;
					output_information.TextColor = resultColour;
				});
			});

			output_status.Text = "Running...";
			output_status.TextColor = Xamarin.Forms.Color.Black;
			output_information.Text = "";
		}

		private void onIdentityPoolChanged(object sender, TextChangedEventArgs e)
		{
			credentials = null;
			input_identityPool.TextColor = Xamarin.Forms.Color.Black; // In case some earlier try turned it red
		}

		private void onIdentityPoolRegionChanged(object sender, EventArgs e)
		{
			credentials = null;
		}
	}
}
