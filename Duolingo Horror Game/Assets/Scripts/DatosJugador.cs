using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class DatosJugador
{
    public static bool fileExists = false;
    public static void SaveData(GameOver gameOver)
    {
        DatosJuego datosJuego = new DatosJuego(gameOver);
        string dataPath = Application.persistentDataPath + "/DHG_data.save";
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        if (!File.Exists(dataPath))
        {
            FileStream fileStream = new FileStream(dataPath, FileMode.Create);
            binaryFormatter.Serialize(fileStream, datosJuego);
            fileStream.Close();
        }
        else if (File.Exists(dataPath))
        {
            FileStream fileStream = new FileStream(dataPath, FileMode.Open, FileAccess.Write);
            binaryFormatter.Serialize(fileStream, datosJuego);
            fileStream.Close();
        }
    }
    public static DatosJuego LoadPlayerData()
    {
        string dataPath = Application.persistentDataPath + "/DHG_data.save";
        if (File.Exists(dataPath))
        {
            FileStream fileStream = new FileStream(dataPath, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            DatosJuego datosJuego = (DatosJuego) binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            fileExists = true;
            return datosJuego;
            Debug.Log("Arhivo de guardado encontrado");
        }
        else
        {
            Debug.Log("No se encontro archivo de guardado");
            Debug.Log(dataPath);
            fileExists = false;
            return null;
        }
    }
    /*public static DatosJuego DeleteData(bool elimDatos)
    {
        string dataPath = Application.persistentDataPath + "/DHG_data.save";
        if (File.Exists(dataPath) && elimDatos)
        {
            FileStream fileStream = new FileStream(dataPath, FileMode.Truncate);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            DatosJuego datosJuego = (DatosJuego)binaryFormatter.Deserialize(fileStream);
            File.Delete(dataPath);
            Debug.Log("Guardado eliminado");
            fileStream.Close();
            return datosJuego;
        }
        else
        {
            return null;
        }
    }*/
}
