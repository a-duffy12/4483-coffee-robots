using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoad
{
    // function to save game data
    public static void SaveData()
    {
        string path = Path.Combine(Application.persistentDataPath, "CoffeClashGameData.acd"); // save data file location

        BinaryFormatter bf = new BinaryFormatter(); // create a formatter
        FileStream fs = new FileStream(path, FileMode.Create); // create the save data file

        GameData data = new GameData(); // get the game save data

        bf.Serialize(fs, data); // serialize the data into binary at the specified location
        fs.Close(); // close the file stream
    }

    // function to load game data
    public static GameData LoadData()
    {
        string path = Path.Combine(Application.persistentDataPath, "CoffeClashGameData.acd"); // save data file location

        if (File.Exists(path)) // if there is existing save data
        {
            BinaryFormatter bf = new BinaryFormatter(); // create a formatter
            FileStream fs = new FileStream(path, FileMode.Open); // open existing game save file

            GameData data = bf.Deserialize(fs) as GameData; // deserialize the data
            fs.Close(); // close the file stream

            return data;
        }
        else // if there is no existing save data
        {
            Debug.Log("Coffee Clash game data file not found at " + path); 
            SaveData(); // create a new save file
            Debug.Log("New Coffee Clash game data file created at " + path);
            return null;
        }
    }
}