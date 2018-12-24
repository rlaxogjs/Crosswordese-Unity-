using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
    public Text exp;
    public LevelControl level_control;
    public Text[] answer_exp;
    public string[] random;

	// Use this for initialization
	void Start () {
        random = new string[] { "병", "진", "삼", "역", "삭", "숨", "열", "평", "골", "사",
            "화", "수", "목", "금", "일", "월", "재", "제", "걱", "석",
            "이", "현", "진", "김", "헌", "이", "민", "결", "정", "회",
            "경", "락", "강", "남", "북", "동", "농", "바", "안", "정"
        };
	}
	
	// Update is called once per frame
	void Update () {

	}

    public LevelData changeExpWithIpos(Block.iPosition i_pos, Block.GO go) {
        List<LevelData> level_datas = level_control.level_datas;
        LevelData ret = new LevelData();

        string exp_text = "";
        foreach (LevelData level_data in level_datas) {
            if (i_pos.x != level_data.i_pos.x || i_pos.y != level_data.i_pos.y || exp_text != "")
                continue;
            if (go == Block.GO.NONE) {
                /*
                if (level_data.go == Block.GO.HORIZONTAL)
                    exp_text += "<가로>\n";
                else
                    exp_text += "<세로>\n";
                    */
                exp_text += level_data.exp;
            }else if (go == Block.GO.HORIZONTAL) {
                if (level_data.go != Block.GO.HORIZONTAL)
                    continue;
                exp_text += level_data.exp;
            } else {
                if (level_data.go != Block.GO.VERTICAL)
                    continue;
                exp_text += level_data.exp;
            }
            ret = level_data;

            int[] a = new int[16];
            for (int i = 0; i < a.Length; i++)
                a[i] = 0;
            for (int i = 0; i < level_data.answer.Length; i++) {
                int ai = Random.Range(0, a.Length - 1);
               while(a[ai] == 1)
                    ai = Random.Range(0, a.Length - 1);
                a[ai] = 1;
                answer_exp[ai].text = level_data.answer[i].ToString();
            }
            for(int i = 0; i < a.Length; i++) {
                if (a[i] == 1)
                    continue;

                bool rr;
                string aa = null;
                do {
                    aa = random[Random.Range(0, random.Length)];
                    rr = false;

                    for(int j = 0; j <  level_data.answer.Length; j++) {
                        if (level_data.answer[j].Equals(aa))
                            rr = true;
                    }

                    for (int j = 0; j < i; j++) {
                        if (answer_exp[j].text.ToString().Equals(aa))
                            rr = true;
                    }
                } while (rr);
                answer_exp[i].text = aa;
            }
        }
        exp.text = exp_text;

        return ret;
    }

    public bool checkLevelData() {
        bool ret = false;


        return ret;
    }

}
