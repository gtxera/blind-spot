using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DialogueModificationProcessor : AssetModificationProcessor
{
    private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
    {
        var projectDirectory = Directory.GetCurrentDirectory();
        var linesDirectory = Path.Combine(projectDirectory, "Assets/Dialogue/Lines");

        if (assetPath.Contains("Full Dialogues"))
        {
            var dialogueName = assetPath.Substring(assetPath.LastIndexOf("/") + 1);
            dialogueName = dialogueName.Substring(0, dialogueName.LastIndexOf("."));

            var dialogueLines = Path.Combine(linesDirectory, dialogueName);
            
            if (Directory.Exists(dialogueLines))
            {
                DirectoryInfo linesInfo = new DirectoryInfo(dialogueLines);

                foreach (FileInfo file in linesInfo.GetFiles())
                {
                    file.Delete();
                }
                
                linesInfo.Delete();
                DirectoryInfo linesFolderInfo = new DirectoryInfo(linesDirectory);
                
                foreach (FileInfo file in linesFolderInfo.GetFiles())
                {
                    if (file.Name.Contains($"{dialogueName}.meta"))
                    {
                        file.Delete();
                    }
                }
            }
        }

        return AssetDeleteResult.DidNotDelete;
    }
}
