using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public static class JsonLoader
    {
        public static List<MainManager.DialogPart> GetJsonArrayFromFile(string jsonFilename)
        {
            var jsonRelativeFilename = Application.dataPath + "/Dialogs/" + jsonFilename;
            if (File.Exists(jsonRelativeFilename))
            {
                Debug.Log("File found: " + jsonRelativeFilename + "!!!");
                using (var reader = new StreamReader(jsonRelativeFilename))
                {
                    string jsonString = reader.ReadToEnd();
                    var dialogArray = JsonUtility.FromJson<CharacterScript.DialogArray>(jsonString);
                    if (dialogArray != null)
                    {
                        return dialogArray.array.ToList();
                    }
                    else
                    {
                        Debug.Log("Json is NOT PARSED, it's null!!!");
                    }
                }
            
            }
            else
            {
                Debug.Log("No dialog file found: " + jsonRelativeFilename);
                //throw new FileNotFoundException();
            }

            return null;
        }
    }
    
}