
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CocktailAppVS.Models;

namespace CocktailAppVS.Views
{
    public partial class MainWindow : Window
    {
        private TextBox userInputTextBox;
        private TextBlock resultTextBlock;
        public MainWindow()
        {
            InitializeComponent();

            userInputTextBox = this.FindControl<TextBox>("UserInputTextBox");
            resultTextBlock = this.FindControl<TextBlock>("ResultTextBlock");
        }

        private async void LookUpByIdClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            string apiKey = "1";

            var inputDialog = new InputDialog("Enter Drink ID:");
            await inputDialog.ShowDialog(this);
            string cocktailId = inputDialog.Result;

            if (!string.IsNullOrEmpty(cocktailId))
            {
                await GetCocktailById(apiKey, cocktailId);
            }
        }

        


        private async void FetchRandomClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            string apiKey = "1";
            await GetCocktailRandom(apiKey);
        }

        private async Task GetCocktailById(string apiKey, string id)
        {
            using (HttpClient client = new HttpClient())
            {
                string endpoint = $"https://www.thecocktaildb.com/api/json/v1/{apiKey}/lookup.php?i={id}";
                HttpResponseMessage response = await client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var cocktailData = JsonConvert.DeserializeObject<CocktailData>(responseData);

                    if (cocktailData.Drinks != null && cocktailData.Drinks.Count > 0)
                    {
                        var firstCocktail = cocktailData.Drinks[0];
                        UpdateResult($"Cocktail Name: {firstCocktail.StrDrink}\n" +
                                     $"Category: {firstCocktail.StrCategory}\n" +
                                     $"Glass Type: {firstCocktail.StrGlass}\n" +
                                     $"Instructions: {firstCocktail.StrInstructions}");
                    }
                    else
                    {
                        UpdateResult($"No information found for cocktail ID {id}.");
                    }
                }
                else
                {
                    UpdateResult($"Error: {response.StatusCode}");
                }
            }
        }
        private void ExitClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Close();
        }

        private async Task GetCocktailRandom(string apiKey)
        {
            using (HttpClient client = new HttpClient())
            {
                string apiEndpoint = $"https://www.thecocktaildb.com/api/json/v1/{apiKey}/random.php";
                HttpResponseMessage response = await client.GetAsync(apiEndpoint);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var cocktailData = JsonConvert.DeserializeObject<CocktailData>(responseData);

                    if (cocktailData.Drinks != null && cocktailData.Drinks.Count > 0)
                    {
                        var randomCocktail = cocktailData.Drinks[0];
                        UpdateResult($"Random Cocktail Name: {randomCocktail.StrDrink}\n" +
                                     $"Category: {randomCocktail.StrCategory}\n" +
                                     $"Glass Type: {randomCocktail.StrGlass}\n" +
                                     $"Instructions: {randomCocktail.StrInstructions}");
                    }
                    else
                    {
                        UpdateResult($"No information found for a random cocktail.");
                    }
                }
                else
                {
                    UpdateResult($"Error: {response.StatusCode}");
                }
            }
        }
        private void UpdateResult(string result)
        {
            resultTextBlock.Text = result;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}