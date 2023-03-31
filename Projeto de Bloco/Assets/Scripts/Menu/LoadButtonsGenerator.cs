using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadButtonsGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _loadButtonPrefab;
    
    void Start()
    {
        var mainPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Blind Spot");
        
        if (!Directory.Exists(mainPath)) return;

        var files = Directory.EnumerateFiles(mainPath);

        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file).Split('.')[0];
            
            var loadButton = Instantiate(_loadButtonPrefab, transform);

            loadButton.GetComponentInChildren<TextMeshProUGUI>().text = fileName;
            loadButton.GetComponent<LoadButton>().SetSaveId(fileName);
        }
    }
}
