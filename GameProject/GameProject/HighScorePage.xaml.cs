using GameProject.Model;
using GameProject.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HighScorePage : ContentPage
    {
        // Fields
        private readonly GameProjectJSONFilePersistence _persistence;
        private readonly GameProjectModel _model;


        // Constructor
        public HighScorePage(GameProjectModel model, GameProjectJSONFilePersistence persistence)
        {
            InitializeComponent();

            _persistence = persistence;
            _model = model;
        }


        // Control Event Handlers
        private void MainMenu_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainPage(_model, _persistence);
        }
        private void ShowHighScores_Clicked(object sender, EventArgs e)
        {
            _model.OnShowHighScores();
        }


        // Model Event Handlers
        private void View_ShowHighScores(object sender, HighScoresEventArgs e)
        {
            _highScore01.Text = "1. " + e.TopHighScores[0].ToString();
            _highScore02.Text = "2. " + e.TopHighScores[1].ToString();
            _highScore03.Text = "3. " + e.TopHighScores[2].ToString();
            _highScore04.Text = "4. " + e.TopHighScores[3].ToString();
            _highScore05.Text = "5. " + e.TopHighScores[4].ToString();
        }


        // Protected Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _model.ShowHighScores += View_ShowHighScores;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _model.ShowHighScores -= View_ShowHighScores;
        }
    }
}