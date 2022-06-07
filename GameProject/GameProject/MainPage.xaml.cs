using GameProject.Model;
using GameProject.Persistence;
using System;
using Xamarin.Forms;

namespace GameProject
{
    public partial class MainPage : ContentPage
    {
        // Fields
        private readonly GameProjectJSONFilePersistence _persistence;
        private readonly GameProjectModel _model;


        // Constructor
        public MainPage(GameProjectModel model, GameProjectJSONFilePersistence  persistence)
        {
            InitializeComponent();

            _model = model;
            _persistence = persistence;
        }


        // Control Event Handlers
        private void PlayGame_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new GamePage(_model, _persistence);
        }
        private void HighScores_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new HighScorePage(_model, _persistence);
        }
    }
}
