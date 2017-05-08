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
		public MainPage()
		{
			InitializeComponent();
		}

		private void onButtonClicked(object sender, EventArgs e)
		{
			output_information.Text = "Lambda invocation requested";
		}

		private void onAcquireIdentity(object sender, EventArgs e)
		{
			output_information.Text = "Identity pool changed to " + input_identityPool.Text;
		}
	}
}
