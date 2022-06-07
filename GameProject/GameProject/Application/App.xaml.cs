using GameProject.Model;
using GameProject.Persistence;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace GameProject
{
    public partial class App : Application
    {
        // Fields
        private readonly GameProjectJSONFilePersistence _persistence;
        private readonly GameProjectModel _model;


        // Constructor
        public App()
        {
            InitializeComponent();

            _persistence = new GameProjectJSONFilePersistence();
            _model = new GameProjectModel(_persistence);

            MainPage = new MainPage(_model, _persistence);
        }


        // Protected Methods
        protected override async void OnStart()
        {
            await _model.LoadGameAsync();
        }
        protected override void OnSleep()
        {
            Task.Run(async () => await _model.SaveGameAsync());
        }
        protected override void OnResume()
        {
            Task.Run(async () => await _model.LoadGameAsync());
        }
    }
}
