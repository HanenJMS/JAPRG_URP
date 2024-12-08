using GameLab.GridSystem;
using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLab.UISystem
{
    public class SaveLoadMenu : MonoBehaviour
    {
        public HexGridVisualSystem hexGrid;

        bool saveMode;
        public TextMeshProUGUI menuLabel, actionButtonLabel;
        public TMP_InputField nameInput;
        public SaveLoadItem itemPrefab;
        public Transform listContent;
        string GetSelectedPath()
        {
            string mapName = nameInput.text;
            if (mapName.Length == 0)
            {
                return null;
            }
            return Path.Combine(Application.persistentDataPath, mapName + ".map");
        }
        public void Open(bool saveMode)
        {
            this.saveMode = saveMode;
            if (saveMode)
            {
                menuLabel.text = "Save Map";
                actionButtonLabel.text = "Save";
            }
            else
            {
                menuLabel.text = "Load Map";
                actionButtonLabel.text = "Load";
            }
            FillList();
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
        public void Action()
        {
            string path = GetSelectedPath();
            if (path == null)
            {
                return;
            }
            if (saveMode)
            {
                Save(path);
            }
            else
            {
                Load(path);
            }
            Close();
        }
        public void SelectItem(string name)
        {
            nameInput.text = name;
        }
        public void Save(string path)
        {
            using (BinaryWriter writer = new BinaryWriter(
                File.Open(path, FileMode.Create)))
            {
                writer.Write(1);
                HexGridVisualSystem.Instance.Save(writer);
            }
            Debug.Log(Application.persistentDataPath);
        }

        public void Load(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError("File does not exist " + path);
                return;
            }
            using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
            {
                int header = reader.ReadInt32();
                if (header <= 1)
                {
                    HexGridVisualSystem.Instance.Load(reader, header);
                }
                else
                {
                    Debug.LogWarning("Unknown map format " + header);
                }
            }
        }
        public void Delete()
        {
            string path = GetSelectedPath();
            if (path == null)
            {
                return;
            }
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            nameInput.text = "";
            FillList();
        }
        void FillList()
        {
            for (int i = 0; i < listContent.childCount; i++)
            {
                Destroy(listContent.GetChild(i).gameObject);
            }
            string[] paths =
                Directory.GetFiles(Application.persistentDataPath, "*.map");

            Array.Sort(paths);
            for (int i = 0; i < paths.Length; i++)
            {
                SaveLoadItem item = Instantiate(itemPrefab);
                item.menu = this;
                item.MapName = Path.GetFileNameWithoutExtension(paths[i]);
                item.transform.SetParent(listContent, false);
            }

        }
    }
}

