using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;

namespace Yunash.Data
{
    public interface IDataService
    {
        void SaveData<T>(T dataObject, string fileName) where T : class;
        bool TryLoadData<T>(string fileName, out T dataObject) where T : class;
        AudioData AudioData { get; }
    }

    public class DataManager : MonoBehaviour, IDataService
    {
        public static DataManager Instance;

        [SerializeField] private AudioData audioData;

        [SerializeField] private Text[] coinTexts;
        private int coins;

        public AudioData AudioData => audioData;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Optional if you want DataManager to persist between scenes
            }
            else
            {
                Destroy(gameObject); // Prevent duplicate DataManager instances
            }

            if (audioData == null)
                throw new NullReferenceException("One or more Data File(s) missing.");

            coins = PlayerPrefs.GetInt("coins", 0);
        }


        public void SaveData<T>(T dataObject, string fileName) where T : class
        {
            string json = JsonConvert.SerializeObject(dataObject, Newtonsoft.Json.Formatting.Indented);

            string path = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllText(path, json);

            Debug.Log($"Saved {fileName} JSON to path{path}");
        }

        public bool TryLoadData<T>(string fileName, out T dataObject) where T : class
        {
            dataObject = null;

            string path = Path.Combine(Application.persistentDataPath, fileName);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                dataObject = JsonConvert.DeserializeObject<T>(json);

                return true;
            }
            else
                return false;
        }

        void Start()
        {
            UpdateCoinsTexts();


        }

        void Update()
        {

        }

        private void UpdateCoinsTexts()
        {
            foreach (Text coinText in coinTexts)
            {
                coinText.text = coins.ToString();
            }
        }

        public void AddCoins(int amount)
        {
            coins += amount;

            UpdateCoinsTexts();

            PlayerPrefs.SetInt("coins", coins);
        }

    }

}
