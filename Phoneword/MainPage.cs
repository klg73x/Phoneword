using System;
using Xamarin.Forms;

namespace Phoneword
{
	public class MainPage : ContentPage
	{
		Entry txtPhoneNumber = new Entry();
		Button bttnTranslate = new Button();
		Button bttnCall = new Button();
		string translatedNumber = "";

		public MainPage()
		{
			StackLayout mainStack = new StackLayout
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Padding = new Thickness(20, Device.OnPlatform(40, 20, 20), 20, 20),
				Spacing = 15
			};
			Label lblTitle = new Label
			{
				Text = "Enter a Phone Word:"
			};

			txtPhoneNumber = new Entry
			{
				Text = "1-855-XAMARIN"
			};

			bttnTranslate = new Button
			{
				Text = "Translate"
			};
			bttnTranslate.Clicked += BttnTranslate_Clicked;

			bttnCall = new Button
			{
				Text = "Call",
				IsEnabled = false
			};
			bttnCall.Clicked += bttnCall_Clicked;

			mainStack.Children.Add(lblTitle);
			mainStack.Children.Add(txtPhoneNumber);
			mainStack.Children.Add(bttnTranslate);
			mainStack.Children.Add(bttnCall);
			this.Content = mainStack;
		}

		void BttnTranslate_Clicked(object sender, EventArgs e)
		{
			string phonenumber = txtPhoneNumber.Text;
			translatedNumber = Core.PhonewordTranslator.ToNumber(phonenumber);
			if (!string.IsNullOrEmpty(translatedNumber))
			{
				bttnCall.IsEnabled = true;
				bttnCall.Text = "Call: " + translatedNumber;
			}
			else { bttnCall.IsEnabled = false; bttnCall.Text = "Call"; }
		}

		async void bttnCall_Clicked(object sender, EventArgs e)
		{
			string callmessage = "Would you like to call " + translatedNumber;
			if (await DisplayAlert("Dial a Number", callmessage, "Yes", "No"))
			{
				//Dial the number
				var dialer = DependencyService.Get<IDialer>();
				if (dialer != null)
				{
					await dialer.DialAsync(translatedNumber);
				}
			}
		}
	}
}

