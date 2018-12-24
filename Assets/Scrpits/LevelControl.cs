using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class LevelData {
    public int num;
    public  Block.iPosition i_pos;
    public Block.GO go;
    public int len;
    public string answer;
    public string exp;
    public LevelData() {
        exp = "";
    }
}

public class LevelControl : MonoBehaviour {
    public List<LevelData> level_datas = null;

    public void loadLevelData(TextAsset level_data_text) {
        this.level_datas = new List<LevelData>();

        string level_texts = level_data_text.text;
        string[] lines = level_texts.Split('\n');
        foreach (var line in lines) {
            if (line == "") {
                continue;
            }
           
            string[] words = line.Split();
            int n = 0;

            LevelData level_data = new LevelData();
            foreach (var word in words) {
                if (word.StartsWith("#"))
                    break;
                if (word == "")
                    continue;
                switch (n) {
                    case 0:
                        level_data.num = int.Parse(word);break;
                    case 1:
                        level_data.i_pos.x = int.Parse(word); break;
                    case 2:
                        level_data.i_pos.y = int.Parse(word); break;
                    case 3:
                        level_data.go = (Block.GO)int.Parse(word); break;
                    case 4:
                        level_data.len = int.Parse(word); break;
                    case 5:
                        level_data.answer = word; break;
                    default:
                        level_data.exp += " " + word; break;
                }
                n++;
            }
            if(n >= 7) {
                level_datas.Add(level_data);
            }
        }
        Debug.Log(level_datas.Count);
    }

}
