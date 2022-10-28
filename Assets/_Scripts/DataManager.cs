using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public static int bestScore;

    public bool isRingYellow;

    public bool isRingBlack;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadMaterial();
        }
        else
        {
            Destroy(gameObject);
        }
    }

   

    [System.Serializable]

    public class SaveData
    {
        public int d_bestScore;

        public bool d_isRingYellow;

        public bool d_isRingBlack;
    }



    public void SaveBestScore(int score)
    {
        if (score > bestScore)
        {
            SaveData data = new SaveData();

            bestScore = score;

            data.d_bestScore = score;

            string json = JsonUtility.ToJson(data);

            string path = Application.persistentDataPath + "savefile.json";

            File.WriteAllText(path, json);
        }
    }

    public void LoadBestScore()
    {
        string path = Application.persistentDataPath + "savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestScore = data.d_bestScore;
        }
    }


    public void SaveMaterial()
    {
        SaveData data = new SaveData();

        data.d_isRingBlack = isRingBlack;

        data.d_isRingYellow = isRingYellow;

        string path = Application.persistentDataPath + "savefile.json";

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(path, json);
    }

    public void LoadMaterial()
    {

        string path = Application.persistentDataPath + "savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            isRingBlack = data.d_isRingBlack;

            isRingYellow = data.d_isRingYellow;
        }
    }

}
