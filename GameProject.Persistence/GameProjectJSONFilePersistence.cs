using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GameProject.Persistence
{
    public class GameProjectJSONFilePersistence
    {
        public async Task SaveGameStateAsync(GameProjectState state)
        {
            try
            {
                string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "save.dat");
                string json = JsonConvert.SerializeObject(state);
                await Task.Run(() => File.WriteAllText(fileName, json));
            }
            catch { }
        }
        public async Task<GameProjectState> LoadGameStateAsync()
        {
            try
            {
                string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "save.dat");
                GameProjectState state = null;
                await Task.Run(() =>
                {
                    if (File.Exists(fileName))
                    {
                        string json = File.ReadAllText(fileName);
                        state = JsonConvert.DeserializeObject<GameProjectState>(json);
                    }
                });
                return state;
            }
            catch
            {
                return null;
            }
        }
    }
}
