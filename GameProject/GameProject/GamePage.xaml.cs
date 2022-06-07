using GameProject.Model;
using GameProject.Persistence;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {

        // Fields
        private readonly GameProjectJSONFilePersistence _persistence;
        private readonly GameProjectModel _model;


        // Constructor
        public GamePage(GameProjectModel model, GameProjectJSONFilePersistence persistence)
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
        private async void Reset_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Restart Game", "Are you sure?", "Yes", "No"))
                _model.ResetGame();
        }
        private void UP_Clicked(object sender, EventArgs e)
        {
            _model.RunGameLoop(MoveEnum.UP);
        }
        private void LEFT_Clicked(object sender, EventArgs e)
        {
            _model.RunGameLoop(MoveEnum.LEFT);
        }
        private void DOWN_Clicked(object sender, EventArgs e)
        {
            _model.RunGameLoop(MoveEnum.DOWN);
        }
        private void RIGHT_Clicked(object sender, EventArgs e)
        {
            _model.RunGameLoop(MoveEnum.RIGHT);
        }


        // Model Event Handlers
        private void View_GameStateChanged(object sender, BlockPositionEventArgs e)
        {
            UpdateUI(e);
        }
        private async void View_GameOver(object sender, EventArgs e)
        {
            await DisplayAlert("Game Over", "", "Okay");
            _model.ResetGame();
        }
        private async void View_GameWon(object sender, EventArgs e)
        {
            await DisplayAlert("Game Won", "", "Okay");
            _model.ResetGame();
        }


        // Private Methods - UI
        private void UpdateUI(BlockPositionEventArgs e)
        {
            // Update Block
            Image[] labels = new Image[] {Block00, Block01, Block02, Block03, Block04, Block05, Block06, Block07,
                                              Block08, Block09, Block10, Block11, Block12, Block13, Block14, Block15};

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].Source = CreateImageSource(e.BlockPositions[i]);
            }

            _scoreLabel.Text = "Score: " + e.Score.ToString();
            _highScoreLabel.Text = e.Score > e.HighScore ? "High Score: " + e.Score.ToString() : "High Score: " + e.HighScore.ToString();
        }
        private string CreateImageSource(int blockValue)
        {
            if (blockValue < 10)
                return "block0" + blockValue.ToString() + ".png";
            else
                return "block" + blockValue.ToString() + ".png";
        }
        private void UpdateUI()
        {
            _model.RequestUIUpdate();
        }


        // Protected Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _model.GameStateChanged += View_GameStateChanged;
            _model.GameOver += View_GameOver;
            _model.GameWon += View_GameWon;
            UpdateUI();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _model.GameStateChanged -= View_GameStateChanged;
            _model.GameOver -= View_GameOver;
            _model.GameWon -= View_GameWon;
        }
    }
}