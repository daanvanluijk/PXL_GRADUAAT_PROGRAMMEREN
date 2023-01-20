using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CMProject.Services
{
    public class AudioService
    {
        private readonly string[] filesToUse =
        {
            "add_page",
            "delete",
            "download",
            "image",
            "move",
            "paragraph",
            "pencil",
            "save",
            "upload",
        };

        private List<IAudioPlayer> audioPlayers = new List<IAudioPlayer>();
        /// <summary>
        /// A lookup table to translate a sound file name to the corresponding index in audioPlayers
        /// </summary>
        private Dictionary<string, int> lookup = new Dictionary<string, int>();

        public AudioService()
        {
            RegisterSoundFiles();
        }

        public void PlaySound(string sound)
        {
            if (!lookup.ContainsKey(sound))
            {
                throw new Exception("Sound does not exist!");
            }
            audioPlayers[lookup[sound]].Play();
        }

        private async void RegisterSoundFiles()
        {
            for (int i = 0; i < filesToUse.Length; i++)
            {
                string fileName = filesToUse[i];
                if (!await FileSystem.AppPackageFileExistsAsync($"{fileName}.wav"))
                {
                    break;
                }
                Stream file = await FileSystem.OpenAppPackageFileAsync($"{fileName}.wav");
                IAudioPlayer audioPlayer = AudioManager.Current.CreatePlayer(file);
                audioPlayers.Add(audioPlayer);
                lookup[fileName] = i;
            }
        }
    }
}
